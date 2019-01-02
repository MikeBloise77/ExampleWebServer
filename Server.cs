using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ExampleWebServer
{
    internal sealed class Server : IDisposable
    {
        private readonly TcpListener _server;
        private bool _started = false;

        public Server(string url, int port)
        {
            _server = new TcpListener(IPAddress.Parse(url), port);
        }

        public void Start()
        {
            _server.Start();
            _started = true;

            while (_started)
            {
                using (var client = _server.AcceptTcpClient())
                {
                    Console.WriteLine("Connected");

                   // using (
                    var sr = new StreamReader(client.GetStream());
                    {
                        var request = sr.ReadLine();

                        Console.WriteLine(request);

                        var tokens = request.Split(' ');
                        var page = tokens[1];
                        Console.WriteLine(page);
                        //if (page == "/")
                        {
                            var body = @"<html><body>Hello world</body></html>";
                            var response = @"HTTP/1.1 200 OK\n";
                            //Server: Example
                            //Accept-Ranges: bytes
                            //Content-Length: " + body.Length + @"
                            //Content-Type: text/html
                            //" + body;

                        //using (
                            var sw = new StreamWriter(client.GetStream());
                        {
                            sw.WriteLine(response);
                            sw.WriteLine(body);
                            sw.Flush();
                        }
                        }
                    }

                    client.Close();
                    //var bytes = new Byte[256];
                        ////while (stream.Read(bytes, 0, bytes.Length) > 0)
                        //{
                        //    stream.Read(bytes, 0, bytes.Length);
                        //    var data = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                        //    Console.Write("{0}", data);
                        //}

                        //Console.WriteLine("Sending response... ");

                        //var msg = System.Text.Encoding.ASCII.GetBytes("response");
                        //stream.Write(msg, 0, msg.Length);
                        //Console.Write("Sent: {0} bytes", msg.Length);
                }
            }
        }

        public void Stop()
        {
            _started = false;
        }

//        try

//        {
//                //string connetionString = "Data Source=54.213.195.209;Initial Catalog=Example;User ID=example;Password=example";
//                //SqlConnection cnn = new SqlConnection(connetionString);
//                //try
//                //{
//                //    cnn.Open();                    
//                //    cnn.Close();
//                //}
//                //catch (Exception ex)
//                //{
//                //    Console.WriteLine("Can not open data connection");
//                //}

//                Int32 port = 8083;
//                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
//                server = 
               

//                Byte[] bytes = new Byte[256];
//                String data = null;

//                while (true)
//                {
//                    Console.Write("Waiting for a connection... ");
//                    TcpClient client = server.AcceptTcpClient();
//                    Console.WriteLine("Connected");

//                    data = null;
//                    NetworkStream stream = client.GetStream();

//                    int i;
//                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
//                    {
//                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
//                        Console.Write("{0}", data);
//                    }

//                    String body = @"<html><body>Hello world</body></html>";
//                    String response =
//@"HTTP/1.1 200 OK
//Server: Example
//Accept-Ranges: bytes
//Content-Length: " + body.Length.ToString() + @"
//Content-Type: text/html

//" + body;

//                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
//                    stream.Write(msg, 0, msg.Length);
//                    Console.WriteLine("Sent: {0}", response);
//                    client.Close();
//                }
//            }
//            catch (SocketException e)
//            {
//                Console.WriteLine("SocketException: {0}", e);
//            }
//            finally
//            {
//                server.Stop();
//            }

//            Console.WriteLine("\nHit enter to continue...");
//            Console.Read();
//        }
        public void Dispose()
        {
            _server?.Stop();
        }
    }
}
