using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExampleWebServer.Handlers;
using ExampleWebServer.Logger;

namespace ExampleWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ws = new WebServer(new FileResponseHandler(new ConsoleLogger()), GetServerRoutes().ToArray())) //todo: use IOC here
            {
                ws.Run();

                Console.WriteLine("Webserver started. Press a key to quit.");
                Console.ReadKey();
                ws.Stop();
            }
        }

        static IEnumerable<string> GetServerRoutes()
        {
            const string htmlFolder = "root";
            const string baseUrl = "http://localhost:8083/"; //todo: move to config

            foreach (var file in Directory.GetFiles(htmlFolder))
            {
                yield return baseUrl + file.Replace(htmlFolder + "\\", "").Replace(".html", "/");
            }

            yield return baseUrl;
        }
    }
}
