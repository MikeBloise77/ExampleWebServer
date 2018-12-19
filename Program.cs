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
        static void Main(string[] args)
        {
            TcpListener server = null;
            string connetionString = "Data Source=54.213.195.209;Initial Catalog=Example;User ID=example;Password=example";
            SqlConnection cnn = new SqlConnection(connetionString);

            try
            {
                //string connetionString = "Data Source=54.213.195.209;Initial Catalog=Example;User ID=example;Password=example";
                //SqlConnection cnn = new SqlConnection(connetionString);
                try
                {
                    cnn.Open();                    
                    //cnn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can not open data connection");
                }

                //127.0.0.1:8081
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
                    try {
                        
                        while (stream.DataAvailable)
                        {
                            i = stream.Read(bytes, 0, bytes.Length);
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            Console.Write("{0}", data);
                            
                        }
                    }catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    stream.Close();

                    String body = @"<html><body>Hello world</body></html>";
                    String response =
                        @"HTTP/1.1 200 OK
                        Server: Example
                        Accept-Ranges: bytes
                        Content-Length: " + body.Length.ToString() + @"
                        Content-Type: text/html

                        " + body;

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                    //stream.Write(msg, 0, msg.Length);
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
                cnn.Close();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
