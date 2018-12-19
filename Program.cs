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

                Byte[] bytes = new Byte[1024];
                String data = null;

                while (true)
                {
                    Console.Write("Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected");

                    data = null;
                    NetworkStream stream = client.GetStream();

                    int i = stream.Read(bytes, 0, bytes.Length);
                    if(i  != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.Write("{0}", data);
                    }
                    //Parse the url from the resquest(I.e. the data variable)
                    //Then, check to see what needs to be sent:
                    //  If its an HTML file, load the html file from the html folder in memory and then send it.
                    //  If its not an html file, see if its a resource from the database(I.e. /testcases)
                    //      If it is, query the data and send the data as json(I.e. set the content type to json)
                    String body = @"<!DOCTYPE html><html><body>Hello world</body></html>";
                    String response =
@"HTTP/1.1 200 OK
Server: Example
Accept-Ranges: bytes
Content-Length: " + body.Length.ToString() + @"
Content-Type: text/html

" + body;

                    // Examples for CanWrite, and CanWrite  

                    // Check to see if this NetworkStream is writable.
                    if (stream.CanWrite)
                    {

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                        stream.Write(msg, 0, msg.Length);
                        stream.Flush();
                        Console.WriteLine("Sent: {0}", response);
                    }
                    
                    else
                    {
                        Console.WriteLine("Sorry.  You cannot write to this NetworkStream.");
                    }

                    stream.Close();
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
