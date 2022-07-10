using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public class LoggingManager : ILoggingManager
    {
        public string LOG_FORMAT => "[%dt%] - [%lv%] - %msg%";
        public LoggingManager()
        {
            loggers = new List<ILogger>();
            asyncLoggers = new List<IAsyncLogger>();
        }
        private readonly List<ILogger> loggers;
        private readonly List<IAsyncLogger> asyncLoggers;
        public void ConnectLogger(ILogger logger)
        {
            if (logger is IAsyncLogger al)
            {
                asyncLoggers.Add(al);
            }
            else
            {
                loggers.Add(logger);
            }

            logger.Open();
            Log($"Connected new Logger of type {logger.GetType().FullName}", ELogLevel.DEBUG).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public void CloseLogger(ILogger logger)
        {
            logger.Close();

            if (logger is IAsyncLogger al)
            {
                asyncLoggers.Remove(al);
            }
            else
            {
                loggers.Remove(logger);
            }
        }
        public async Task Log(string message, ELogLevel level = ELogLevel.INFO)
        {
            if (level <= App.LogLevel)
            {
                string logMessage = LOG_FORMAT.Replace("%dt%", DateTime.Now.ToString()).Replace("%lv%", level.ToString()).Replace("%msg%", message);
                foreach (ILogger l in loggers)
                {
                    try
                    {
                        l.Log(logMessage, level);
                        if (l is IAsyncLogger al)
                        {
                            await al.LogAsync(logMessage, level);
                        }
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

                foreach (IAsyncLogger al in asyncLoggers)
                {
                    try
                    {
                        await al.LogAsync(logMessage, level);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            Debug.WriteLine("Can't log in {0}. Exception: \n{1}\n{2}", al.GetType().FullName, ex.Message, ex.StackTrace);
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
                {
                    await LogException(message, level, ex.InnerException);
                }

                await Log(ex.Message, level);
                await Log(ex.StackTrace, level);
            }
        }

        public T GetLogger<T>() where T : ILogger
        {
            return (T)loggers.FirstOrDefault(d => d is T);
        }
    }
}
