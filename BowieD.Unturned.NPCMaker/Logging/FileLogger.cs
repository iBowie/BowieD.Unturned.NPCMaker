using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public sealed class FileLogger : ILogger
    {
        public static readonly string Dir = AppConfig.ExeDirectory;
        public static string GetContents()
        {
            return File.ReadAllText(Path.Combine(Dir, "npcmaker.log"));
        }
        private StreamWriter stream;
        public void Close()
        {
            stream.Flush();
            stream.Close();
        }
        public void Open()
        {
            if (File.Exists(Path.Combine(Dir, "npcmaker.old.log")))
            {
                File.Delete(Path.Combine(Dir, "npcmaker.old.log"));
            }

            if (File.Exists(Path.Combine(Dir, "npcmaker.log")))
            {
                File.Move(Path.Combine(Dir, "npcmaker.log"), Path.Combine(Dir, "npcmaker.old.log"));
            }

            stream = new StreamWriter(Path.Combine(Dir, "npcmaker.log"), false, Encoding.UTF8);
        }

        public async Task Log(string message, ELogLevel level)
        {
            try
            {
                await stream.WriteLineAsync(message);
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
    }
}
