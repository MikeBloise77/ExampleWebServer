using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ExampleWebServer
{
    class Response
    {
        Byte[] data = null;
        String status;
        String mime;
        private Response(String status, String mime, Byte[] data)
        {
            this.data = data;
            this.status = status;
            this.mime = mime;
        }

        public static Response Create(Request request)
        {

            if (request == null) return null;

            switch (request.Type)
            {
                case "GET":
                    //index
                    if (request.Url == @"/") return new Response("200 OK", "text/html", Encoding.ASCII.GetBytes(@"<html><body>Hello world</body></html>"));

                    //respond from files
                    String file = Environment.CurrentDirectory + Program.WEB_DIR + request.Url;
                    FileInfo fi = new FileInfo(file);

                    if (fi.Exists && fi.Extension.Contains("."))
                        return RespondFromFile(fi.Name, "200 OK");
                    else //respond from sql
                        return RespondFromSql(request.Url.Substring(1, request.Url.Length - 1), "200 OK");
                default:
                    return RespondFromFile("400.htm", "400 Bad Request");
            }
        }

        private static Response RespondFromFile(String fileName, String status)
        {
            String file = Environment.CurrentDirectory + Program.WEB_DIR + fileName;
            return new Response(status, "text/html", ReadFileToArray(file));
        }

        private static Response RespondFromSql(String type, String status)
        {
            SqlConnection cnn = new SqlConnection(Program.connetionString);

            try
            {
                cnn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open data connection");
            }

            SqlCommand command = new SqlCommand("Select Content from Contents where ResourceName=@name", cnn);
            command.Parameters.AddWithValue("@name", type);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                    return new Response(status, "text/html", Encoding.ASCII.GetBytes(reader["Content"].ToString()));
            }
            try
            {
                cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not close data connection");
            }

            return RespondFromFile("400.htm", "400 Bad Request");
        }

        private static byte[] ReadFileToArray(String file)
        {
            FileInfo fi = new FileInfo(file);
            try
            {
                FileStream fs = fi.OpenRead();
                BinaryReader reader = new BinaryReader(fs);
                Byte[] b = new Byte[fs.Length];
                reader.Read(b, 0, b.Length);
                return b;
            }
            catch
            {
                Console.WriteLine("File exception: " + file);
                return null;
            }
        }

        public void Post(NetworkStream stream)
        {
            if (data == null) return;

            String response = String.Format("{0} {1}\r\nServer: {2}\r\nAccept-Ranges: bytes\r\nContent-Length: {3}\r\nContent-Type: text/html\r\n\r\n",
              Program.VERSION, this.status, Program.SERVERNAME, data.Length);

            byte[] resp = System.Text.Encoding.ASCII.GetBytes(response);
            byte[] msg = resp.Concat(data).ToArray();

            stream.Write(msg, 0, msg.Length);
            Console.WriteLine("Sent: {0}", response);
        }
    }
}
