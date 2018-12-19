using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace ExampleWebServer
{
    class Program
    {


        private static TcpListener server = null;
        private static string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;



        #region ServerConnections
        static void OpenServerConn()
        {
            Int32 port = 8081;
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
        }


        static void interfaceWithClient()
        {
            Byte[] bytes = new Byte[256];
            String data = null;
            const String body = @"<!DOCTYPE html><html><body>Hello world</body></html>";
            String response =
                                @"HTTP/1.1 200 OK
                                Server: Example
                                Accept-Ranges: bytes
                                Content-Length: " + body.Length.ToString() + @"
                                Content-Type: text/html

                                " + body;




            while (true)
            {
                Console.Write("Waiting for a connection... ");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected");

                data = null;
                NetworkStream stream = client.GetStream();




                byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("Sent: {0}", response);


                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.Write("{0}", data);
                }


                client.Close();
            }
        }
        #endregion


        static void Main(string[] args)
        {
            

            try
            {
                ConnectSQL();
                OpenServerConn();
                interfaceWithClient();

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


        #region Database and File Management
        static void ConnectSQL()
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
        }

        


        static void FindFile(string fileToUpload)
        {
            StreamReader reader = new StreamReader(rootDirectory + "/Example.html");
            string tmp = reader.ReadToEnd();

            Console.WriteLine(tmp);

            reader.Close();
        }
        #endregion


    }
}
