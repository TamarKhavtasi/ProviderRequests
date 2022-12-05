using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Winton.Extensions.Configuration.Consul;

namespace ProviderRequests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(webBuilder =>
            {
                var consulAddress = Environment.GetEnvironmentVariable("CONSUL_SERVICE_URL");
                var consulKey = "core-platform";

                webBuilder.AddConsul(
                    $"{consulKey}/shared/core-system",
                    options =>
                    {
                        options.ReloadOnChange = true;
                        options.ConsulConfigurationOptions =
                            cco => cco.Address = new Uri(consulAddress);
                    }
                );
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
