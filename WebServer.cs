using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleWebServer
{
    public class WebServer
    {
        private readonly string[] _indexFiles = {
            "hello.html"
        };

        private static readonly IDictionary<string, string> _mimeTypeMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {".htm", "text/html"},
            {".html", "text/html"}
        };

        private Thread _serverThread;
        private string _rootDirectory;
        private HttpListener _listener;
        private int _port;

        public WebServer(string path, int port)
        {
            this.Initialize(path, port);
        }

        private void Initialize(string path, int port)
        {
            this._rootDirectory = path;
            this._port = port;
            _serverThread = new Thread(this.Listen);
            _serverThread.Start();
        }

        private void Listen()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:" + _port.ToString() + "/");
            _listener.Start();

            while (true)
            {
                try
                {
                    var context = _listener.GetContext();
                    ProcessRequest(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _listener.Stop();
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var filename = context.Request.Url.AbsolutePath;
            filename = filename.Substring(1);

            if (string.IsNullOrEmpty(filename))
            {
                foreach (var indexFile in _indexFiles)
                {
                    if (!File.Exists(Path.Combine(_rootDirectory, indexFile)))
                    {
                        continue;
                    }

                    filename = indexFile;
                    break;
                }
            }

            filename = Path.Combine(_rootDirectory, filename);

            if (File.Exists(filename))
            {
                try
                {
                    var input = new FileStream(filename, FileMode.Open);

                    context.Response.ContentType = _mimeTypeMappings.TryGetValue(Path.GetExtension(filename), out var mime) ? mime : "application/octet-stream";
                    context.Response.ContentLength64 = input.Length;
                    context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                    context.Response.AddHeader("Last-Modified", System.IO.File.GetLastWriteTime(filename).ToString("r"));

                    var buffer = new byte[1024 * 16];
                    int nbytes;

                    while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        context.Response.OutputStream.Write(buffer, 0, nbytes);
                    }

                    input.Close();

                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Flush();
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            context.Response.OutputStream.Close();
        }

        public void Stop()
        {
            _serverThread.Abort();
            _listener.Stop();
        }
    }
}
