using Microsoft.Extensions.Hosting;
using System;
using Topshelf;

namespace Hangfire
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
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
                x.SetStartTimeout(TimeSpan.FromMinutes(2)); // Adjust timeout as needed
            });

        }
    }
}
