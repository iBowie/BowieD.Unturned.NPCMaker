using System;

namespace BowieD.NPCMaker.Logging
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogException(string message, Exception exception);
        void LogDebug(string message);
        void Start();
        void Stop();
    }
}
