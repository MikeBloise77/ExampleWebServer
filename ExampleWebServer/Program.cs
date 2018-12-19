using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
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
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.Write("{0}", data);
                    
                    String htmlPath = @"C:\Users\cjflo\Source\Repos\ExampleWebServer\index.html";
                    String htmlHolder = System.IO.File.ReadAllText(htmlPath);
                    String body = htmlHolder;
                    String response =
@"HTTP/1.1 200 OK
Server: Example
Accept-Ranges: bytes
Content-Length: " + body.Length.ToString() + @"
Content-Type: text/html

" + body;

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", response);
                    String path = @"C:\Users\cjflo\Source\Repos\ExampleWebServer\log.txt";

                    if(!File.Exists(path)){
                            using(StreamWriter sw = File.CreateText(path)){
                                sw.WriteLine(msg);
                            }
                    }else{
                               File.AppendAllText("log.txt", msg.ToString() + Environment.NewLine);
                         }
                    }
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
