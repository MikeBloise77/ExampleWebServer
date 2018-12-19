using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebServer.DataAccess
{
    public class ContentDAL:IContentDAL
    {
        private string _connectionString;

        public ContentDAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
        }

        public string GetContent(string resourceName)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sql = @"SELECT * FROM dbo.Contents WHERE ResourceName=@ResourceName";
                    var content = string.Empty;
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@ResourceName", resourceName.Replace("/", ""));
                        connection.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            content = reader["Content"].ToString();
                        }
                        connection.Close();
                    }
                    return content;
                }
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}
