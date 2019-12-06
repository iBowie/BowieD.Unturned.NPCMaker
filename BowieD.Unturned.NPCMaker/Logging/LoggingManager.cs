using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public class LoggingManager : ILoggingManager
    {
        public string LOG_FORMAT { get => "[%dt%] - [%lv%] - %msg%"; }
        public LoggingManager()
        {
            loggers = new List<ILogger>();
        }
        private readonly List<ILogger> loggers;
        public void ConnectLogger(ILogger logger)
        {
            loggers.Add(logger);
            logger.Open();
            Log($"Connected new Logger of type {logger.GetType().FullName}", ELogLevel.DEBUG);
        }
        public void CloseLogger(ILogger logger)
        {
            logger.Close();
            loggers.Remove(logger);
        }
        public async Task Log(string message, ELogLevel level = ELogLevel.INFO)
        {
            if (level <= App.LogLevel)
            {
                string logMessage = LOG_FORMAT.Replace("%dt%", DateTime.Now.ToString()).Replace("%lv%", level.ToString()).Replace("%msg%", message);
                foreach (var l in loggers)
                {
                    try
                    {
                        await l.Log(logMessage, level);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            Debug.WriteLine("Can't log in {0}. Exception: \n{1}\n{2}", l.GetType().FullName, ex.Message, ex.StackTrace);
                        }
                        finally { }
                    }
                }
            }
        }

        public async Task LogException(string message, ELogLevel level = ELogLevel.ERROR, Exception ex = null)
        {
            await Log(message, level);
            if (ex != null)
            {
                if (ex.InnerException != null)
                    await LogException(message, level, ex.InnerException);
                await Log(ex.Message, level);
                await Log(ex.StackTrace, level);
            }
        }
    }
}
