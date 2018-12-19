using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Drawing;

namespace ExampleWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                string connetionString = "Data Source=54.213.195.209;Initial Catalog=Example;User ID=example;Password=example";
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

                while (true)
                {
                    Console.WriteLine("Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    Thread t = new Thread(() => processClient(client));
                    t.Start();
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

        static void processClient(TcpClient tcpClient)
        {
            Console.WriteLine("Connected");
            Byte[] bytes = new Byte[256];
            String data = null;
            NetworkStream stream = tcpClient.GetStream();
            
            int i;
            while (tcpClient.Connected & (i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("{0}", data);

                if (i < 256)
                {
                    String assetLink = @"..\..\assets\index.htm";
                    //String assetLink = @"..\..\assets\earth2.jpg";
                    String assetType = contentType(assetLink);
                    byte[] body = File.ReadAllBytes(assetLink);
                    String bodyStr = System.Text.Encoding.ASCII.GetString(body, 0, body.Length);
                    String response =
                    @"HTTP/1.1 200 OK
                        Server: Example
                        Accept-Ranges: bytes
                        Content-Length: " + body.Length.ToString() + @"
                        Content-Type: " + assetType + @"

                    " + (contentType(assetLink) == "text/html" ? bodyStr : "");
                    
                    FileStream f = new FileStream(assetLink, FileMode.Open);
                    StreamReader s = new StreamReader(f);
                    String buff = s.ReadToEnd();
                    s.Close();

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                    byte[] b = System.Text.Encoding.ASCII.GetBytes(buff);
                    stream.Write(msg, 0, msg.Length);

                    if (assetType != "text/html")
                    {
                        stream.Write(b, 0, b.Length);
                    }
                    Console.WriteLine("Sent: {0}", response);
                    tcpClient.Close();
                    f.Close();
                    break;
                }
            }

            tcpClient.Close();
        }

        static String contentType(String type)
        {
            if (type.EndsWith("htm"))
            {
                return "text/html";
            }
            else if (type.EndsWith("pdf"))
            {
                return "application/pdf";
            }
            else if (type.EndsWith("jpg")) {
                return "image/jpeg";
            }

            return "";
        }
    }
}
