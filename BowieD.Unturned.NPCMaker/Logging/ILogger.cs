using System;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public interface ILogger
    {
        void Open();
        void Close();
        void LogInfo(string message);
        void LogDebug(string message);
        void LogException(string message, Exception ex);
        void LogWarning(string message);
    }
}
