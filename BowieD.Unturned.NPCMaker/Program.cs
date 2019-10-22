using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace BowieD.Unturned.NPCMaker
{
    public class Program
    {
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        static extern IntPtr RemoveMenu(IntPtr hMenu, uint nPosition, uint wFlags);
        internal const uint SC_CLOSE = 0xF060;

        internal const uint MF_GRAYED = 0x00000001;

        internal const uint MF_BYCOMMAND = 0x00000000;
        [STAThread]
        static void Main()
        {
            IntPtr hMenu = Process.GetCurrentProcess().MainWindowHandle;

            IntPtr hSystemMenu = GetSystemMenu(hMenu, false);
            EnableMenuItem(hSystemMenu, SC_CLOSE, MF_GRAYED);

            RemoveMenu(hSystemMenu, SC_CLOSE, MF_BYCOMMAND);
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
