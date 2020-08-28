using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneDayWarriorsWebApi.Logging
{
    public class NLogger : INLogger
    {
        private readonly ILogger<NLogger> _logger;
        public NLogger(ILogger<NLogger> logger)
        {
            _logger = logger;
        }
        public void LogError(string errorMessage)
        {
            _logger.LogError(errorMessage);
        }

        public void LogInformation(string informationMessage)
        {
            _logger.LogInformation(informationMessage);
        }
    }
}
