using Microsoft.Extensions.Hosting;
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
            });

        }
    }
}
