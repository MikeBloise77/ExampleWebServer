using ExampleWebServer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebServer.Services
{
    public class RequestLoggerService
    {
        public static void Log(string message)
        {
            IRequestLogger requestLogger = new RequestLogger();
            requestLogger.Log(message);
        }
    }
}
