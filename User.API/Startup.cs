using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User.API.Data;

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
           services.AddDbContext<AppUserDbContext>(options =>
    options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //lifetime.ApplicationStarted.Register(OnStart);
            //lifetime.ApplicationStopped.Register(OnStopped);

            app.UseMvc();
        }

        public void OnStart()
        {
            var client = new ConsulClient();// uses default host:port which is localhost:8500
            var httpcheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10),
                Interval = TimeSpan.FromSeconds(30),
                HTTP = $"http://127.0.0.1:55745/HealthCheck"
            };

            var agentReg = new AgentServiceRegistration()
            {
                ID = "servicename:55745",
                Address = "127.0.0.1",
                Check = httpcheck,
                Name = "servicename",
                Port = 55745
            };

            client.Agent.ServiceRegister(agentReg).ConfigureAwait(false);
        }

        public void OnStopped()
        {
            var client = new ConsulClient();
            client.Agent.ServiceDeregister("servicename:55745");
        }
    }
}
