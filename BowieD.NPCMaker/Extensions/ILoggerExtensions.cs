using BowieD.NPCMaker.Logging;
using System;
using System.Collections.Generic;

namespace BowieD.NPCMaker.Extensions
{
    public static class ILoggerExtensions
    {
        public static void LogInfo(this IEnumerable<ILogger> loggers, string message)
        {
            foreach (var k in loggers)
            {
                k.LogInfo(message);
            }
        }
        public static void LogDebug(this IEnumerable<ILogger> loggers, string message)
        {
            foreach (var k in loggers)
            {
                k.LogDebug(message);
            }
        }
        public static void LogWarning(this IEnumerable<ILogger> loggers, string message)
        {
            foreach (var k in loggers)
            {
                k.LogWarning(message);
            }
        }
        public static void LogException(this IEnumerable<ILogger> loggers, string message, Exception exception)
        {
            foreach (var k in loggers)
            {
                k.LogException(message, exception);
            }
        }
        public static void Start(this IEnumerable<ILogger> loggers)
        {
            foreach (var k in loggers)
                k.Start();
        }
        public static void Stop(this IEnumerable<ILogger> loggers)
        {
            foreach (var k in loggers)
                k.Stop();
        }
    }
}
