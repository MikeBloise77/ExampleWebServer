using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebServer.DataAccess
{
    public interface IContentDAL
    {
        string GetContent(string resourceName);
    }
}
