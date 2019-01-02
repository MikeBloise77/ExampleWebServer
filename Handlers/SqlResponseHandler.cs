using System.Data;
using System.Data.SqlClient;
using System.Net;
using ExampleWebServer.Logger;

namespace ExampleWebServer.Handlers
{
    internal class SqlResponseHandler : BaseResponseHandler
    {
        public SqlResponseHandler(ILogger logger) : base(logger)
        {
        }

        public override string Handle(HttpListenerRequest request)
        {
            LogRequest(request);
            return GetContentFromDb(request.RawUrl.TrimEnd('/'));
        }

        string GetContentFromDb(string uri)
        {
            const string connectionString =
                "Data Source=54.213.195.209;Initial Catalog=Example;User ID=example;Password=example"; //todo: move to config

            const string query = "select Content from dbo.HtmlContents where url = {0}";

            using (var cnn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(string.Format(query, uri), cnn))
                {
                    cmd.CommandType = CommandType.Text;
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }
    }
}
