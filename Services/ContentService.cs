using ExampleWebServer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebServer.Services
{
    public class ContentService:IContentService
    {
        IContentDAL _contentDAL;

        public ContentService()
        {
            _contentDAL = new ContentDAL();
        }

        public string GetContent(string resourceName)
        {
            return _contentDAL.GetContent(resourceName);
        }
    }
}
