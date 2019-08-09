using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public static class LoggerExtensions
    {
        public static void Open(this IEnumerable<ILogger> loggers)
        {
            foreach (var k in loggers)
                k.Open();
        }
        public static void Close(this IEnumerable<ILogger> loggers)
        {
            foreach (var k in loggers)
                k.Close();
        }
        public static void LogInfo(this IEnumerable<ILogger> loggers, string message)
        {
            foreach (var k in loggers)
                k.LogInfo(message);
        }
        public static void LogDebug(this IEnumerable<ILogger> loggers, string message)
        {
            foreach (var k in loggers)
                k.LogDebug(message);
        }
        public static void LogException(this IEnumerable<ILogger> loggers, string message, Exception ex)
        {
            foreach (var k in loggers)
                k.LogException(message, ex);
        }
        public static void LogWarning(this IEnumerable<ILogger> loggers, string message)
        {
            foreach (var k in loggers)
                k.LogWarning(message);
        }
    }
}
