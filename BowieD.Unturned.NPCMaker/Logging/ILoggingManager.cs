using System;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public interface ILoggingManager
    {
        string LOG_FORMAT { get; }

        T GetLogger<T>() where T : ILogger;
        void CloseLogger(ILogger logger);
        void ConnectLogger(ILogger logger);
        Task Log(string message, ELogLevel level = ELogLevel.INFO);
        Task LogException(string message, ELogLevel level = ELogLevel.ERROR, Exception ex = null);
    }
}