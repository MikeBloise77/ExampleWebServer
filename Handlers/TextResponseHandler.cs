using System;
using System.Net;
using ExampleWebServer.Logger;

namespace ExampleWebServer.Handlers
{
    internal class TextResponseHandler : BaseResponseHandler
    {
        public TextResponseHandler(ILogger logger) : base(logger){}

        public override string Handle(HttpListenerRequest request)
        {
            LogRequest(request);
            return  $"<HTML><BODY>Hello world {DateTime.Now}</BODY></HTML>";
        }
    }
}
