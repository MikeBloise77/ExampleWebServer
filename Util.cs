using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace ExampleWebServer
{
   public class Util
    {
        public static void Log(SqlConnection cnn,StringBuilder msg)
        {

            if (msg != null && msg.ToString().Length > 0)
            {
                try
                {
                    string sql = string.Format("INSERT INTO REQUESTLOG (REQUEST) VALUES ('{0}')", msg.ToString());
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cnn.Open();
                    cmd.ExecuteScalar();

                    cnn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can not open data connection");
                }
            }

        }
    }
}
