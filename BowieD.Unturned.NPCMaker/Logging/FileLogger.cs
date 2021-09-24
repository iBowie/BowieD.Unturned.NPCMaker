using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.IO;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public sealed class FileLogger : ILogger, IDisposable
    {
        public static readonly string Dir = AppConfig.ExeDirectory;
        private StreamWriter stream;
        public void Close()
        {
            stream.Flush();
            stream.Close();
        }
        public void Open()
        {
            string oldLogPath = Path.Combine(Dir, "npcmaker.old.log");
            string logPath = Path.Combine(Dir, "npcmaker.log");

            if (File.Exists(oldLogPath))
            {
                File.Delete(oldLogPath);
            }

            if (File.Exists(logPath))
            {
                File.Move(logPath, oldLogPath);
            }

            var fs = new FileStream(logPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            stream = new StreamWriter(fs)
            {
                AutoFlush = true
            };
        }

        public void Log(string message, ELogLevel level)
        {
            try
            {
                stream.WriteLine(message);
            }
            catch (Exception ex)
            {
                ConsoleColor old = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Can't write in log file directly. Doubling the message in Console with error provided");
                Console.WriteLine(message);
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                Console.ForegroundColor = old;
            }
        }

        public void Dispose()
        {
            ((IDisposable)stream).Dispose();
        }
    }
}
