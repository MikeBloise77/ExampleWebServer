using ExampleWebServer.Services;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace ExampleWebServer
{
    public class ServiceResponse
    {
        private string _resourceName;
        public bool IsResourceExist = false;
        public string content = string.Empty;
        public string contentType = "text/html";
        public string ResponseString = string.Empty;
        public Byte[] ResponseMessage;

        IContentService _contentService;

        public ServiceResponse(string resouceName)
        {
            _contentService = new ContentService();

            _resourceName = resouceName;

            string path = GetRootPath()+ @"html\";

            if (string.IsNullOrEmpty(_resourceName) || _resourceName == "/")
            {
                _resourceName = "index.htm";
            }

            if (File.Exists(path + _resourceName))
            {
                content = System.IO.File.ReadAllText(path + _resourceName);
                IsResourceExist = true;
            }

            if(!isFileRequest())
            { 
                getContentFromDB();
            }

            getResponse();
        }

        private bool isFileRequest()
        {
            var fileParts = _resourceName.Split('.');
            if(fileParts.Length<=1)
            {
                return false;
            }

            contentType = getContentType();
            return true;
        }

        private string getContentType()
        {
            var fileParts = _resourceName.Split('.');
            var fileExtension = fileParts[1].ToLower();

            switch (fileExtension)
            {
                case "txt":
                case "htm":
                case "html":
                    return "text/html";
                case "jpeg":
                case "jpg":
                    return "image/jpeg";
                case "js":
                    return "application/javascript";
                case "json":
                    return "application/json";
                case "png":
                    return "image/png";
                case "pdf":
                    return "application/pdf";
                case "xml":
                    return "application/xml";
                default:
                    return "application/octet-stream";
            }
        }

        public static string GetRootPath()
        {
            var debugPath = string.Empty;
            #if (DEBUG)
            debugPath = "..\\..\\";
            #endif
            return Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath), debugPath);
        }

        private void getResponse()
        {
            StringBuilder sb = new StringBuilder();
            if (IsResourceExist)
            {
                sb.AppendLine("HTTP/1.1 200 OK");
            }
            else
            {
                sb.AppendLine("HTTP/1.1 404 Not found");
            }

            sb.AppendLine("Server: Example");
            sb.AppendLine("Accept-Ranges: bytes");
            sb.AppendLine("Content-Length: " + content.Length.ToString());
            sb.AppendLine("Content-Type: " + contentType);
            sb.AppendLine("");
            sb.AppendLine(content);

            ResponseString = sb.ToString();
            ResponseMessage = System.Text.Encoding.ASCII.GetBytes(sb.ToString());
        }

        private void getContentFromDB()
        {
            var data = _contentService.GetContent(_resourceName);
            if(string.IsNullOrEmpty(data))
            {
                IsResourceExist = false;
            }
            else
            {
                IsResourceExist = true;
                content = data;
            }
        }
    }
}
