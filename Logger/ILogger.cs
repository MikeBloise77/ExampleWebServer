namespace ExampleWebServer.Logger
{
    internal interface ILogger
    {
        void Info(string msg);
        void Error(string msg);
    }
}
