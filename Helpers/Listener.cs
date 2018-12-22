using System;
using System.Net;
using System.Net.Sockets;

class Listener: IListener
{
    public void Listen()
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
                while (stream.DataAvailable && (i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.Write("{0}", data);
                }

                String body = @"<html><body>Hello world</body></html>";
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