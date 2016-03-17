using System;

namespace UsageExample
{
    public class ØvelsesConsoleLogger : IØvelsesLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}