using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MediatR;
using Project.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Project.API.Application.Service;
using Project.API.Application.Queries;
using Project.API.Dto;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Project.Domain.AggregatesModel;
using Project.Infrastructure.Repositories;

namespace Project.API
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

            #region
            //services.AddAuthentication()
            //    .AddIdentityServerAuthentication(authenticationProviderKey, options =>
            //    {
            //                    //options.Authority = "https://localhost:50255";
            //                    options.Authority = "http://localhost:50255";
            //        options.ApiName = "gateway_api";
            //        options.SupportedTokens = SupportedTokens.Both;
            //        options.ApiSecret = "secret";
            //        options.RequireHttpsMetadata = false;
            //    });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
               {
                   options.Authority = "http://localhost:50255";
                   options.Audience = "project_api";
                   options.RequireHttpsMetadata = false;
               });

            //services.AddAuthentication("Bearer")
            //.AddIdentityServerAuthentication(options => {
            //    options.Authority = "http://localhost:50255" ;
            //    options.ApiName = "project_api";
            //    options.RequireHttpsMetadata = false;
            //}
            //);
            #endregion
            services.AddDbContext<ProjectContext>(options =>
                {
                    options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")
                    , builder => builder.MigrationsAssembly(typeof(ProjectContext).Assembly.GetName().Name)
                    //, b => b.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name)
                    );
                }
            );
            #region
            services.AddScoped<IRecommendService, RecommendService>()
                .AddScoped<IProjectQueries, ProjectQueries>(sp =>
                { return new ProjectQueries(Configuration.GetConnectionString("DefaultConnection")); }
                );
            services.AddScoped<IProjectRepository, ProjectRepository>();
            #endregion

            #region
            services.AddCap(options =>
            {
                options.UseEntityFramework<ProjectContext>()
                    .UseRabbitMQ("hostname")//TBD
                    .UseDashboard();
                options.UseDiscovery(d=>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 4313;
                    d.NodeId = 11;
                    d.NodeName = "CAP ProjectAPI Node";
                });
            });
            #endregion

            //AddMediatR 其实是在 Microsoft.Extension.DependencyInjection.MediatR中，但是是对 IServiceCollection services的扩展，所以需要引入 Microsoft.Extension.DependencyInjection.MediatR
            services.AddMediatR();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime appLife,
            ILoggerFactory loggerFactory,
            IOptions<ServiceDisvoveryOptions> serviceDisvoveryOptions,
            IConsulClient consul)
        {
            //register deregister  healthycheck
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceDisvoveryOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(30),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(address, "HealthCheck").OriginalString
                };

                var registration = new AgentServiceRegistration()
                {
                    Check = httpCheck,
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceDisvoveryOptions.Value.ServiceName,
                    Port = address.Port
                };

                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                appLife.ApplicationStopping.Register(() =>
                {
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                });
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCap();
            //app.UseAuthentication();
            app.UseMvc();
        }
    }
}
