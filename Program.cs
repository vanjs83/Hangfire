using log4net.Config;
using log4net;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using Topshelf;

namespace Hangfire
{
    class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("app.config"));
            var logger = LogManager.GetLogger(typeof(Program));


          var exitCode = HostFactory.Run(x =>
            {
                x.Service<HangfireService>(s =>
                {
                    s.ConstructUsing(() => new HangfireService());
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                });

                x.RunAsLocalSystem();

                x.SetServiceName("HangfireService");
                x.SetDisplayName("Hangfire Service");
                x.SetDescription("A Hangfire-based Windows Service managed by Topshelf.");
                // Increase timeout to 60 seconds

                x.StartAutomatically();
                x.EnablePauseAndContinue();
                x.SetStartTimeout(TimeSpan.FromMinutes(2)); 
                x.OnException(ex =>
                {
                    logger.Error("Service encountered an error", ex);
                });
            });

            Environment.ExitCode = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
        }
    }
}