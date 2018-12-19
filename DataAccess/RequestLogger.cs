using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace ExampleWebServer.DataAccess
{
    public class RequestLogger : IRequestLogger
    {
        private string _connectionString;

        public RequestLogger()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
        }

        public void Log(string message)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sql = @"INSERT INTO dbo.RequestLog (Request) VALUES (@Message)";

                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@Message", message);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
