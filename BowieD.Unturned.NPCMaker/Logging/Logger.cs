using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public static class Logger
    {
        public static void Log(string message, Log_Level logLevel = Log_Level.Normal)
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "npcmaker.log"))
                File.Create(AppDomain.CurrentDomain.BaseDirectory + "npcmaker.log").Close();
            using (StreamWriter sw = new StreamWriter(path: AppDomain.CurrentDomain.BaseDirectory + "npcmaker.log", encoding: Encoding.UTF8, append: true))
            {
                sw.WriteLine(GetFormatted(logLevel, message));
            }
            lines.Add(GetFormatted(logLevel, message));
            try
            {
                MainWindow.LogWindow.logBox.Text += $"{GetFormatted(logLevel, message)}{Environment.NewLine}";
            }
            catch { }
        }
        public static void Log(StringBuilder stringBuilder, Log_Level logLevel = Log_Level.Normal)
        {
            foreach (var line in stringBuilder.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                Log(line, logLevel);
            }
        }
        public static void Log(Exception ex, Log_Level logLevel = Log_Level.Critical)
        {
            Log("Exception: " + ex.Message, logLevel);
        }
        public static List<string> lines = new List<string>();
        public static void Clear()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "npcmaker.log"))
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "npcmaker.log");
        }

        public static string Format => "[%dateTime%] - [%level%]: %message%";
        public static string GetFormatted(Log_Level level, string message) => Format.Replace("%dateTime%", DateTime.Now.ToString()).Replace("%level%", level.ToString()).Replace("%message%", message);
    }

    public enum Log_Level
    {
        None,
        Normal,
        Warnings,
        Critical,
        Error,
        Debug
    }
}
