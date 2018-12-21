using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection cnn = new SqlConnection(connectionString);
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

                Byte[] bytes = new Byte[256];
                String data = null;

                while (true)
                {
                    Console.Write("Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected");

                    data = null;
                    NetworkStream stream = client.GetStream();

                    int i;
                    while (stream.DataAvailable)
                    {
                       i = stream.Read(bytes, 0, bytes.Length);
                       data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                       Console.Write("{0}", data);
                    }

                    String body = @"<html><body>Hello world</body></html>";
                    String response =@"HTTP/1.1 200 OK
                                       Server: Example
                                       Accept-Ranges: bytes
                                       Content-Length: " + body.Length.ToString() + @"
                                       Content-Type: text/html"
                                       + body;

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", response);
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
