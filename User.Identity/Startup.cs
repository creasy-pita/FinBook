using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.API.Identity.Dto;
using User.Identity.Authentication;
using User.Identity.Services;
using User.Identity.Test;

namespace User.Identity
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddIdentityServer()
                    .AddExtensionGrantValidator<SmsAuthCodeValidator>()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryApiResources(Config.GetResource())
                    .AddInMemoryClients(Config.GetClient())
                    .AddInMemoryIdentityResources(Config.GetIdentityResources());


            services.AddSingleton<HttpClient>(new HttpClient());

            services.AddOptions();
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));


            services.AddSingleton<IDnsQuery>(p =>
            {
                return new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
            });

            services.AddScoped<IAuthCodeService, TestAuthCodeService>();
            services.AddScoped<IUserService, UserServcie>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
