using System;
using System.Collections.Generic;
using System.Text;

namespace OneDayWarriorsWebApi.Logging
{
    public interface INLogger
    {
        public void LogInformation(string errorMessage);
        public void LogError(string informationMessage);
    }
}
