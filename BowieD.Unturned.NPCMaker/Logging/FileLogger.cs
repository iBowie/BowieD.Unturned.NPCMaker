using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.IO;
using System.Text;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public sealed class FileLogger : ILogger
    {
        public static readonly string Dir = AppConfig.Directory;
        public static string GetContents()
        {
            return File.ReadAllText(Dir + "npcmaker.log");
        }
        private StreamWriter stream;
        public void Close()
        {
            stream.Flush();
            stream.Close();
        }
        public void LogDebug(string message)
        {
            stream.WriteLine($"[{DateTime.Now}] - [DEBUG] - {message}");
        }
        public void LogException(string message, Exception ex)
        {
            if (ex.InnerException != null)
            {
                LogException(message, ex.InnerException);
            }
            stream.WriteLine($"[{DateTime.Now}] - [ERROR] - {message}");
            stream.WriteLine($"[{DateTime.Now}] - [ERROR] - {ex.Message}");
            stream.WriteLine($"[{DateTime.Now}] - [ERROR] - {ex.StackTrace}");
        }
        public void LogInfo(string message)
        {
            stream.WriteLine($"[{DateTime.Now}] - [INFO] - {message}");
        }
        public void LogWarning(string message)
        {
            stream.WriteLine($"[{DateTime.Now}] - [WARN] - {message}");
        }
        public void Open()
        {
            if (File.Exists(Dir + "npcmaker.old.log"))
                File.Delete(Dir + "npcmaker.old.log");
            if (File.Exists(Dir + "npcmaker.log"))
                File.Move(Dir + "npcmaker.log", Dir + "npcmaker.old.log");
            stream = new StreamWriter(Dir + "npcmaker.log", false, Encoding.UTF8)
            {
                AutoFlush = true
            };
        }
    }
}
