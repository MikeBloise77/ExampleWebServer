using ExampleWebServer.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
                    
                    data = GetRequest(stream);
                    Console.WriteLine("{0}", data);
                    RequestLoggerService.Log(data);
                    var serviceResponse = new ServiceResponse(GetResourceName(data));
                   

                    
                    stream.Write(serviceResponse.ResponseMessage, 0, serviceResponse.ResponseMessage.Length);
                    Console.WriteLine("Sent: {0}", serviceResponse.ResponseString);
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {                 
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        private static string GetRequest(NetworkStream stream)
        {
            byte[] data = new byte[1024];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                do
                {
                   stream.Read(data, 0, data.Length);
                   memoryStream.Write(data, 0, data.Length);
                } while (stream.DataAvailable);

                return Encoding.ASCII.GetString(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            }
        }

        private static string GetResourceName(string data)
        {
            var requestData = data.Split('\r');
            if(requestData.Length<2)
            {
                return "/";
            }
            var resourceName = requestData[0].Split(' ')[1];
            return resourceName;
        }
    }
}
