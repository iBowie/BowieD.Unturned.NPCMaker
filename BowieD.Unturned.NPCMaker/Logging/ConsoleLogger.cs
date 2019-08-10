using BowieD.Unturned.NPCMaker.Commands;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public sealed class ConsoleLogger : ILogger
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        public static void ShowConsoleWindow()
        {
            if (IsOpened)
                return;
            IsOpened = true;
            ShowWindow(GetConsoleWindow(), SW_SHOW);
        }
        public static void HideConsoleWindow()
        {
            if (!IsOpened)
                return;
            IsOpened = false;
            ShowWindow(GetConsoleWindow(), SW_HIDE);
        }
        public static bool IsOpened { get; private set; } = true;
        public void Close() { }
        public void LogDebug(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] - [DEBUG] - {message}");
        }
        public void LogException(string message, Exception ex)
        {
            var oldClr = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now}] - [ERROR] - {message}");
            Console.WriteLine($"[{DateTime.Now}] - [ERROR] - {ex.Message}");
            Console.WriteLine($"[{DateTime.Now}] - [ERROR] - {ex.StackTrace}");
            Console.ForegroundColor = oldClr;
        }
        public void LogInfo(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] - [INFO] - {message}");
        }
        public void LogWarning(string message)
        {
            var oldClr = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now}] - [WARN] - {message}");
            Console.ForegroundColor = oldClr;
        }
        public static void StartWaitForInput()
        {
            Thread th = new Thread(() => WaitForInput());
            th.Start();
        }
        public void Open() {  }
        public static void WaitForInput()
        {
            string input = Console.ReadLine();
            string[] command = input.Split(' ');
            var executionCommand = Command.Commands.SingleOrDefault(d => d.Name.ToLower() == command[0].ToLower());
            if (executionCommand == null)
            {
                App.Logger.LogInfo($"Command {command[0]} not found");
            }
            else
            {
                var matches = Regex.Matches(string.Join(" ", command.Skip(1)), "[\\\"](.+?)[\\\"]|([^ ]+)", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
                var filtered = (from Match d in matches select d.Value.Trim('"')).ToArray();
                executionCommand.Execute(filtered);
                App.Logger.LogInfo($"User executed a command: {executionCommand.Name}");
            }
            WaitForInput();
        }
    }
}
