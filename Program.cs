using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebServer
{
    class Program
    {
        public const String WEB_DIR = "/root/web/";
        public const String VERSION = "HTTP/1.1";
        public const String SERVERNAME = "Example";

        public static string connetionString = "Data Source=54.213.195.209;Initial Catalog=Example;User ID=example;Password=example";

        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                SqlConnection cnn = new SqlConnection(connetionString);
                try
                {
                    cnn.Open();
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can not open data connection");
                }

                Int32 port = 8081;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                server.Start();

                Byte[] bytes = new Byte[1024];
                String data = null;

                while (true)
                {
                    Console.Write("Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected");

                    data = "";
                    NetworkStream stream = client.GetStream();

                    int i;
                    do
                    {
                        i = stream.Read(bytes, 0, bytes.Length);
                        data += System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    } while (stream.CanRead && stream.DataAvailable);

                    if (data == "") continue;

                    Console.Write("Recieved:\n" + data);

                    Request request = Request.GetRequest(data);
                    Response response = Response.Create(request);
                    if (response != null) response.Post(stream);

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

    }
}
