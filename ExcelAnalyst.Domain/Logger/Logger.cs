using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ExcelAnalyst.Domain.Logger
{
    public class Logger<T> : ILogger<T>
    {
        private readonly Microsoft.Extensions.Logging.ILogger<T> _logger;
        public Logger(Microsoft.Extensions.Logging.ILogger<T> logger)
        {
            _logger = logger;
        }
        public void LogCritical(string message, Exception exception = null, params object[] args)
        {

           _logger.LogCritical(message, exception, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _logger.LogDebug(message, args);
            }
        }

        public void LogError(string message, Exception exception = null, params object[] args)
        {
            _logger.LogError(message, exception, args);
        }


        public void LogInformation(string message, params object[] args)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _logger.LogInformation(message, args);
            }
        }

        public void LogWarning(string message, params object[] args)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _logger.LogWarning(message, args);
            }
        }
    }
}
