using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using ExampleWebServer.Handlers;

namespace ExampleWebServer
{
    internal class WebServer : IDisposable
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly IResponseHandler _responseHandler;

        public WebServer(IResponseHandler responseHandler, string[] prefixes) 
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");

            _responseHandler = responseHandler ?? throw new ArgumentException("responseHandler");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            foreach (var s in prefixes)
                _listener.Prefixes.Add(s);

            _listener.Start();
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Listening..."); //todo: extract to ILogger
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                var responseString = _responseHandler.Handle(ctx?.Request);
                                byte[] buf = Encoding.UTF8.GetBytes(responseString);

                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            } 
                            finally
                            {
                                ctx?.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                } 
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        public void Dispose()
        {
            ((IDisposable) _listener)?.Dispose();
        }
    }
}
