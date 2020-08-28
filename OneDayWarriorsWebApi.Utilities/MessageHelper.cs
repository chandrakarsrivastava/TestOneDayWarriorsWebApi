using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneDayWarriorsWebApi.Utilities
{
    public class MessageHelper : IMessageHelper
    {
        private readonly IConfiguration _config;
        public MessageHelper(IConfiguration config)
        {
            _config = config;
        }
        public string GetMessage(string messageKey)
        {
            return _config["ApplicationMessage:" + messageKey];
        }
    }
}
