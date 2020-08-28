using System;
using System.Collections.Generic;
using System.Text;

namespace OneDayWarriorsWebApi.Logging
{
    public interface INLogger
    {
        void LogInformation(string errorMessage);
        void LogError(string informationMessage);
    }
}
