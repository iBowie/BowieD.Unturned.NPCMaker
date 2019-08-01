using System;
using System.IO;
using System.Text;

namespace BowieD.NPCMaker.Logging
{
    public sealed class FileLogger : ILogger
    {
        private StreamWriter stream;
        public void Start()
        {
            if (File.Exists(PathUtil.GetWorkDir() + "npcmaker.log.old"))
                File.Delete(PathUtil.GetWorkDir() + "npcmaker.log.old");
            if (File.Exists(PathUtil.GetWorkDir() + "npcmaker.log"))
                File.Move(PathUtil.GetWorkDir() + "npcmaker.log", PathUtil.GetWorkDir() + "npcmaker.log.old");
            stream = new StreamWriter(PathUtil.GetWorkDir() + "npcmaker.log", false, Encoding.UTF8);
        }
        public void Stop()
        {
            stream.Close();
            stream = null;
        }
        public void LogInfo(string message)
        {
            if (stream != null)
                stream.WriteLine($"[{DateTime.Now}] [INFO] - {message}");
        }
        public void LogWarning(string message)
        {
            if (stream != null)
                stream.WriteLine($"[{DateTime.Now}] [WARN] - {message}");
        }
        public void LogDebug(string message)
        {
            if (stream != null)
                stream.WriteLine($"[{DateTime.Now}] [DEBUG] - {message}");
        }
        public void LogException(string message, Exception exception)
        {
            if (stream != null)
            {
                stream.WriteLine($"[{DateTime.Now}] [ERROR] - {message}");
                stream.WriteLine($"[{DateTime.Now}] [ERROR] - {exception.StackTrace}");
            }
        }
    }
}
