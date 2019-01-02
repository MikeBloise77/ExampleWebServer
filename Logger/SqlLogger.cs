using System;
using System.Data;
using System.Data.SqlClient;


namespace ExampleWebServer.Logger
{
    internal class SqlLogger : ILogger
    {
        public void Info(string msg)
        {
            WriteToDb($"[{DateTime.Now.TimeOfDay}][Info][{msg}]");
        }

        public void Error(string msg)
        {
            WriteToDb($"[{DateTime.Now.TimeOfDay}][Error][{msg}]");
        }

        private void WriteToDb(string msg)
        {
            const string connectionString =
                "Data Source=54.213.195.209;Initial Catalog=Example;User ID=example;Password=example"; //todo: move to config

            const string query = "insert into dbo.RequestLog values({0}, getdate()))";

            using (var cnn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(string.Format(query, msg), cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
