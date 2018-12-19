using System;

public class DBClass
{
	public DBClass()
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
}
