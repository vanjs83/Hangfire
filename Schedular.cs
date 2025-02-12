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


        [Queue("critical")]
        public void WriteMessage()
        {
            _logger.Log(LogLevel.Information, $"Recurring job executed: {DateTime.Now}");
        }
    }
}
