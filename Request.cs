using System;
using System.Data.SqlClient;

namespace ExampleWebServer
{
    class Request
    {
        public String Type { get; set; }
        public String Url { get; set; }
        private String Host { get; set; }

        private Request(String type, String url, String host)
        {
            Type = type;
            Url = url;
            Host = host;
        }

        public static Request GetRequest(String req)
        {
            if (String.IsNullOrEmpty(req))
                return null;

            //simple parser
            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = req.Split(stringSeparators, StringSplitOptions.None);
            string[] tokens = lines[0].Split(' '); //type, url, version

            string host = lines[1].Split(' ')[1]; //host  

            InsertIntolog(req);

            Console.WriteLine("Type is: {0}, url is: {1}, vesrion is: {2} host is: {3}", tokens[0], tokens[1], tokens[2], host);
            return new Request(tokens[0], tokens[1], host);

        }

        private static void InsertIntolog(string request)
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

            SqlCommand cmd = new SqlCommand("INSERT INTO RequestLog (Request) VALUES (@Request)", cnn);
            cmd.Parameters.AddWithValue("@Request", DateTime.Now.ToString() + " " + request);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to insert into log: " + request);
            }

            try
            {
                cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not close data connection");
            }
        }
    }
}
