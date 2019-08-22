using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace SmartHouseGatewayApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                    config.AddJsonFile("appsettings.json", false)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .UseSerilog((hostingConext, config) => config.ReadFrom.Configuration(hostingConext.Configuration))
                .UseStartup<Startup>();
    }
}
