using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Logging;
using log4net;
using log4net.Config;
using System.IO;

namespace Hangfire
{
    public class HangfireService
    {
        private IHost host;
        public bool Start()
        {
            Console.WriteLine("Start console app!");
             host = Host.CreateDefaultBuilder()
                    .ConfigureServices((hostContext, services) =>
                    {
                        // Add Startup class for service configuration
                        //    var startup = new Startup(hostContext.Configuration);
                        //   startup.ConfigureServices(services);
                    })
                     .ConfigureWebHostDefaults(webBuilder =>
                     {
                         webBuilder.UseKestrel(options =>
                         {
                             // Optional: Configure Kestrel settings, such as ports or SSL
                             // options.Limits.MaxRequestBodySize = 10 * 1024; // Example config for max request body size
                         })
                         .UseStartup<Startup>();
                     })
                      .ConfigureLogging((context, logging) =>
                      {
                          logging.ClearProviders();

                          // Configure log4net from app.config
                          var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
                          XmlConfigurator.Configure(logRepository, new FileInfo("app.config"));

                          // Add log4net as a logging provider
                          logging.AddLog4Net("app.config");
                      })
              .Build();



            host.Run();
            Console.WriteLine("Hangfire Server started. Press any key to exit...");
            Console.ReadKey();
            return true;
        }

        public bool Stop()
        {
            host?.Dispose();
            Console.WriteLine("Hangfire Service stopped.");
            return true;
        }

    }
}
