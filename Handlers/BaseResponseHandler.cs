using System;
using System.Net;
using ExampleWebServer.Logger;

namespace ExampleWebServer.Handlers
{
    internal class BaseResponseHandler : IResponseHandler
    {
        private readonly ILogger _logger;

        public BaseResponseHandler(ILogger logger)
        {
            _logger = logger;
        }

        protected void LogRequest(HttpListenerRequest request)
        {
            _logger.Info("Uri requested - " + request.Url);
        }

        public virtual string Handle(HttpListenerRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
