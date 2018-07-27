using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BuildingBlocks.Resilience.Http;
using Consul;
using Contact.API.Data;
using Contact.API.Dto;
using Contact.API.Infrastructure;
using Contact.API.IntegrationEvents;
using Contact.API.Repositories;
using Contact.API.Services;
using DnsClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Contact.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //AddAuthentication 则 identity 会帮我们装载 认证token中的claims 到 System.Security.Claims.ClaimsPrincipal User
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
               {
                   //options.Authority = "http://localhost:50255";
                   options.Authority = "http://localhost";//发送到网关进行，再由网关进行转发到配置的认证服务器
                   options.Audience = "contact_api";
                   options.RequireHttpsMetadata = false;
               });
            #endregion

            #region  Consul and  Consul Service Disvovery
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            //config the discoveryservice (consulservice ) host address
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;

                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
            #endregion
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<IDnsQuery>(p =>
            {
                //TBD 可动态配置
                return new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
            });
            #region polly register
            if (Configuration.GetValue<string>("UseResilientHttp") == bool.TrueString)
            {
                services.AddSingleton<IResilientHttpClientFactory, ResilientHttpClientFactory>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<ResilientHttpClient>>();
                    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

                    var retryCount = 6;
                    if (!string.IsNullOrEmpty(Configuration["HttpClientRetryCount"]))
                    {
                        retryCount = int.Parse(Configuration["HttpClientRetryCount"]);
                    }

                    var exceptionsAllowedBeforeBreaking = 5;
                    if (!string.IsNullOrEmpty(Configuration["HttpClientExceptionsAllowedBeforeBreaking"]))
                    {
                        exceptionsAllowedBeforeBreaking = int.Parse(Configuration["HttpClientExceptionsAllowedBeforeBreaking"]);
                    }

                    return new ResilientHttpClientFactory(logger, httpContextAccessor, exceptionsAllowedBeforeBreaking, retryCount);
                });
                services.AddSingleton<IHttpClient, ResilientHttpClient>(sp => sp.GetService<IResilientHttpClientFactory>().CreateResilientHttpClient());
            }
            else
            {
                services.AddSingleton<IHttpClient, StandardHttpClient>();
            }
            #endregion

            #region custom service injection
            services.AddScoped<IUserService, UserServcie>();
            services.AddScoped(typeof(ContactContext));
                //.AddScoped<IProjectQueries, ProjectQueries>(sp =>
                //{ return new ProjectQueries(Configuration.GetConnectionString("DefaultConnection")); }
                //)
                ;
            services.AddScoped<IContactRepository, MongoContactRepository>();
            services.AddScoped<IContactApplyRequestRepository, MongoContactApplyRequestRepository>();
            services.AddScoped<UserProfileChangedEventHandler>();
            #endregion

            services.AddMvc();
            #region cap
            services.AddCap(options =>
            {
                options
                    //.UseMySql(op=> op.ConnectionString= "Server=192.168.11.83;Database=finbook_beta_contact;Uid=root;Pwd=root;Encrypt=true;SslMode=none")
                    .UseMySql("Server=192.168.11.83;Database=finbook_beta_contact;Uid=root;Pwd=root;Encrypt=true;SslMode=none")
                    .UseRabbitMQ("localhost");//TBD
                //options.UseDashboard();
                //options.UseDiscovery(d =>
                //{
                //    d.DiscoveryServerHostName = "localhost";
                //    d.DiscoveryServerPort = 8500;
                //    d.CurrentNodeHostName = "localhost";
                //    d.CurrentNodePort = 5800;
                //    d.NodeId = 12;
                //    d.NodeName = "CAP ContactAPI Node";
                //});
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseCap();
            app.UseMvc();
        }
    }
}
