using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BuildingBlocks.Resilience.Http;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Recommend.API.Data;
using Recommend.Infrastructure;
using Recommend.API.Services;
using Recommend.API.IntegrationEventHandler;

namespace Recommend.API
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
            services.AddDbContext<RecommendDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")
                //, builder=>
                //builder.MigrationsAssembly(typeof(ProjectContext).Assembly.GetName().Name)
                //, b => b.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name)

                );
            }
);
            services.AddScoped<ProjectCreatedIntegrationEventHandler>();
            services.AddScoped<IUserService, UserServcie>();
            services.AddScoped<IContactService, ContactService>();

            #region consul 服务发现  和 polly 重试，熔断...
            services.AddSingleton<IDnsQuery>(p =>
            {
                return new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
            });


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

            #region
            services.AddCap(options =>
            {
                options.UseEntityFramework<RecommendDbContext>()
                    .UseRabbitMQ("hostname")//TBD
                    .UseDashboard();
                options.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 4338;
                    d.NodeId = 10;
                    d.NodeName = "CAP RecommendAPI Node";
                });
            });
            #endregion

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCap();
            app.UseMvc();
        }
    }
}
