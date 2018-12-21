using System;
using System.Data.SqlClient;

class DataService : IDataService
{
    public void Connect()
    {
        string connectionString = "Data Source=54.213.195.209;Initial Catalog=Example;User ID=example;Password=example";
        SqlConnection cnn = new SqlConnection(connectionString);
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
}