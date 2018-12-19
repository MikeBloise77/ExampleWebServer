using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebServer.Services
{
    public interface IContentService
    {
        string GetContent(string resourceName);
    }
}
