
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;

namespace Hangfire
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["sql"]?.ConnectionString;

            // Add Hangfire services
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseSqlServerStorage(connectionString,  new SqlServerStorageOptions // Replace with your DB connection string
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }
            );
          });


            services.AddSingleton<ISchedular, Schedular>();
            services.AddTransient<HangFireJobMethod>();


            // Add Hangfire Server with a custom queue named "critical"
            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { "critical", "default" };  // Multiple queues can be defined here
            });

            // Add controllers and other ASP.NET Core services
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
      

            // You can customize the dashboard's URL path here
            app.UseHangfireDashboard("/hangfire"); // Hangfire dashboard at http://localhost:5000/hangfire

            // Enabling routing is necessary for the web server to function
            app.UseRouting();

            // Define endpoints for the application
            app.UseEndpoints(endpoints =>
            {
                // Map Hangfire dashboard route
                endpoints.MapHangfireDashboard(); // Map Hangfire dashboard route
            });


            // Schedule a Hangfire job
            RecurringJob.AddOrUpdate<HangFireJobMethod>("example-job", x => x.JobMethod(), Cron.Minutely, new RecurringJobOptions { TimeZone = TimeZoneInfo.Local});

        }

       public class HangFireJobMethod
       {
            private readonly ILogger<HangFireJobMethod> _logger;
            public HangFireJobMethod(ILogger<HangFireJobMethod> logger)
            {
                _logger = logger;
            }

            public  void JobMethod()
            {
                _logger.LogInformation($"CreateJob Method {DateTime.Now}");
                BackgroundJob.Schedule<Schedular>(job => job.WriteMessage(),DateTime.Now.AddMinutes(1));
            }
        }

    }

}

