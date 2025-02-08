using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire
{

    public interface ISchedular
    {
        void WriteMessage();
    }

    public class Schedular : ISchedular
    {
        private readonly ILogger<Schedular> _logger;

        public Schedular(ILogger<Schedular> logger)
        {
            _logger = logger;
        }


        public void WriteMessage()
        {
            _logger.LogInformation($"Recurring job executed: {DateTime.Now}");
            _logger.LogInformation("Method: WriteMessage");
        }
    }
}
