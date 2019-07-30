using System;

namespace BowieD.NPCMaker.Logging
{
    public sealed class ConsoleLogger : ILogger
    {
        public void LogDebug(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] [DEBUG] - {message}");
        }

        public void LogException(string message, Exception exception)
        {
            var oldclr = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now}] [ERROR] - {message}");
            Console.ForegroundColor = oldclr;
        }

        public void LogInfo(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] [INFO] - {message}");
        }

        public void LogWarning(string message)
        {
            var oldclr = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now}] [WARN] - {message}");
            Console.ForegroundColor = oldclr;
        }
    }
}
