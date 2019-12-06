using System;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public interface ILoggingManager
    {
        string LOG_FORMAT { get; }

        void CloseLogger(ILogger logger);
        void ConnectLogger(ILogger logger);
        Task Log(string message, LogLevel level = LogLevel.INFO);
        Task LogException(string message, LogLevel level = LogLevel.ERROR, Exception ex = null);
    }
}