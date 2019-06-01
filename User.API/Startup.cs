using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Consul;
using DnsClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User.API.Data;
using User.API.Data.POCO;
using User.API.Filters;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace User.API
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
                    options.Audience = "user_api";
                    options.RequireHttpsMetadata = false;
                });
            #endregion

            services.AddOptions();
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
            services.AddDbContext<AppUserDbContext>(options =>
    options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc(options =>
                options.Filters.Add(typeof(GlobalExceptionFilter))
            );
            #region
            services.AddCap(options =>
            {
                options
                    .UseEntityFramework<AppUserDbContext>()
                    //.UseRabbitMQ("localhost");//TBD
                    .UseRabbitMQ("localhost");//TBD
                options.UseDashboard();
                options.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 56688;
                    d.NodeId = 11;
                    d.NodeName = "CAP UserAPI Node";
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IApplicationLifetime lifetime,
            ILoggerFactory loggerFactory,
            IOptions<ServiceDisvoveryOptions> serviceDisvoveryOptions,
            IConsulClient consul)
        {

            //var serviceId = "servicename:56688";

            //var httpCheck = new AgentServiceCheck()
            //{
            //    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10),
            //    Interval = TimeSpan.FromSeconds(30),
            //    HTTP = $"http://127.0.0.1:56688/HealthCheck"
            //};

            //var registration = new AgentServiceRegistration()
            //{
            //    ID = "servicename:56688",
            //    Address = "127.0.0.1",
            //    Check = httpCheck,
            //    Name = "servicename1",
            //    Port = 56688
            //};

            //consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

            //appLife.ApplicationStopping.Register(() =>
            //{
            //    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            //});

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

                lifetime.ApplicationStopping.Register(() =>
                {
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                });
            }
            app.UseCap();
            app.UseAuthentication();
            app.UseMvc();

            RegisterZipkinTrace(app, loggerFactory, lifetime);
        }

        public void RegisterZipkinTrace(IApplicationBuilder app
            , ILoggerFactory loggerFactory
            , IApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(() => {
                TraceManager.SamplingRate = 1.0f;
                var logger = new TracingLogger(loggerFactory, "zipkin4net");
                var httpSender = new HttpZipkinSender("http://192.168.11.83:9411", "application/json");
                var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer(), new Statistics());
                var consoleTracer = new zipkin4net.Tracers.ConsoleTracer();
                TraceManager.RegisterTracer(tracer);
                TraceManager.RegisterTracer(consoleTracer);
                TraceManager.Start(logger);

            });

            lifetime.ApplicationStarted.Register(() => TraceManager.Stop());
            app.UseTracing("user_api");
        }

    }
}
