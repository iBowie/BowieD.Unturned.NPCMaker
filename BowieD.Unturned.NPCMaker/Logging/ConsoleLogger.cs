using BowieD.Unturned.NPCMaker.Commands;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Logging
{
    public sealed class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {
            IntPtr hMenu = Process.GetCurrentProcess().MainWindowHandle;
            IntPtr hSystemMenu = GetSystemMenu(hMenu, false);
            EnableMenuItem(hSystemMenu, SC_CLOSE, MF_GRAYED);
            RemoveMenu(hSystemMenu, SC_CLOSE, MF_BYCOMMAND);
        }
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern IntPtr RemoveMenu(IntPtr hMenu, uint nPosition, uint wFlags);

        private const uint
            SC_CLOSE = 0xF060,
            MF_GRAYED = 0x00000001,
            MF_BYCOMMAND = 0x00000000;
        private const int
            SW_HIDE = 0,
            SW_SHOW = 5;
        public static void ShowConsoleWindow()
        {
            if (IsOpened)
            {
                return;
            }

            IsOpened = true;
            ShowWindow(GetConsoleWindow(), SW_SHOW);
        }
        public static void HideConsoleWindow()
        {
            if (!IsOpened)
            {
                return;
            }

            IsOpened = false;
            ShowWindow(GetConsoleWindow(), SW_HIDE);
        }
        public static bool IsOpened { get; private set; } = true;
        public void Close() { }
        public Task Log(string message, ELogLevel level = ELogLevel.INFO)
        {
            ConsoleColor color;
            switch (level)
            {
                case ELogLevel.POSITIVE:
                    color = ConsoleColor.Green;
                    break;
                default:
                case ELogLevel.INFO:
                    color = ConsoleColor.White;
                    break;
                case ELogLevel.ERROR:
                case ELogLevel.CRITICAL:
                    color = ConsoleColor.Red;
                    break;
                case ELogLevel.DEBUG:
                case ELogLevel.TRACE:
                    color = ConsoleColor.Gray;
                    break;
                case ELogLevel.WARNING:
                    color = ConsoleColor.Yellow;
                    break;
            }
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
            return Task.Delay(0);
        }
        public static void StartWaitForInput()
        {
            Thread th = new Thread(() => WaitForInput());
            th.Start();
        }
        public void Open()
        {
            Console.Title = "Unturned 3.0 NPC Maker by BowieD";
        }
        public static void WaitForInput()
        {
            string input = Console.ReadLine();
            Console.WriteLine(Command.Execute(input));
            WaitForInput();
        }
    }
}
