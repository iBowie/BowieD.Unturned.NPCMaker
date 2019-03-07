using System;
using System.IO;
using System.Text;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public static class Logger
    {
        public static void Log(string message, Log_Level logLevel = Log_Level.Normal)
        {
            if (logLevel > LoggingLevel)
                return;
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "npcmaker.log"))
                File.Create(AppDomain.CurrentDomain.BaseDirectory + "npcmaker.log").Close();
            using (StreamWriter sw = new StreamWriter(path: AppDomain.CurrentDomain.BaseDirectory + "npcmaker.log", encoding: Encoding.UTF8, append: true))
            {
                sw.WriteLine(GetFormatted(logLevel, message));
            }
        }
        public static void Clear()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "npcmaker.log"))
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "npcmaker.log");
        }

        public static string Format => "[%dateTime%] - [%level%]: %message%";
        public static string GetFormatted(Log_Level level, string message) => Format.Replace("%dateTime%", DateTime.Now.ToString()).Replace("%level%", level.ToString()).Replace("%message%", message);

        public static Log_Level LoggingLevel => Config.Configuration.Properties == null ? Log_Level.Normal : Config.Configuration.Properties.LogLevel;
    }

    public enum Log_Level
    {
        None,
        Normal,
        Warnings,
        Critical,
        Errors
    }
}
