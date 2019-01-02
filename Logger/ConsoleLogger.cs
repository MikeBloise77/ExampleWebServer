using System;

namespace ExampleWebServer.Logger
{
    internal class ConsoleLogger : ILogger
    {
        public void Info(string msg)
        {
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}][Info][{msg}]");
        }

        public void Error(string msg)
        {
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}][Error][{msg}]");
        }
    }
}
