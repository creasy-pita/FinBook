using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace User.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();
            var host = BuildWebHost(args);
            host.Start();

            var client = new ConsulClient();

            var name = Assembly.GetEntryAssembly().GetName().Name;
            var port = 56688;
            var id = $"{name}:{port}";

            var tcpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(0.5),
                Interval = TimeSpan.FromSeconds(30),
                TCP = $"127.0.0.1:{port}"
            };

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                Interval = TimeSpan.FromSeconds(30),
                HTTP = $"http://127.0.0.1:{port}/HealthCheck"
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { tcpCheck, httpCheck },
                Address = "127.0.0.1",
                ID = id,
                Name = name,
                Port = port
            };

            client.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

            Console.WriteLine("applicationname...");
            Console.WriteLine("DataService started...");
            Console.WriteLine("Press ESC to exit");

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
            }

            client.Agent.ServiceDeregister(id).GetAwaiter().GetResult();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .Build();
    }
}
