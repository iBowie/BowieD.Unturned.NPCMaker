using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public sealed class FileLogger : ILogger
    {
        public static readonly string Dir = AppDomain.CurrentDomain.BaseDirectory;
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
        public void Open()
        {
            if (File.Exists(Dir + "npcmaker.old.log"))
                File.Delete(Dir + "npcmaker.old.log");
            if (File.Exists(Dir + "npcmaker.log"))
                File.Move(Dir + "npcmaker.log", Dir + "npcmaker.old.log");
            stream = new StreamWriter(Dir + "npcmaker.log", false, Encoding.UTF8);
        }

        public async Task Log(string message, ELogLevel level)
        {
            try
            {
                await stream.WriteLineAsync(message);
            }
            catch (Exception ex)
            {
                var old = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Can't write in log file directly. Doubling the message in Console with error provided");
                Console.WriteLine(message);
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                Console.ForegroundColor = old;
            }
        }
    }
}
