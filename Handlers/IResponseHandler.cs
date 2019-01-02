using System.Net;

namespace ExampleWebServer.Handlers
{
    internal interface IResponseHandler
    {
        string Handle(HttpListenerRequest request);
    }
}
