using System.IO;
using System.Net;
using ExampleWebServer.Logger;

namespace ExampleWebServer.Handlers
{
    internal class FileResponseHandler : BaseResponseHandler
    {
        public FileResponseHandler(ILogger logger) : base(logger){}

        public override string Handle(HttpListenerRequest request)
        {
            LogRequest(request);

            var fileName = request.RawUrl.TrimEnd('/');

            if (string.IsNullOrEmpty(fileName) || fileName == "/") fileName = "/index";

            var path = $"root{fileName}.html";

            if (!File.Exists(path)) path = "root/404.html";

            return File.ReadAllText(path);
        }
    }
}
