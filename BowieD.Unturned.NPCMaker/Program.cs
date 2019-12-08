using BowieD.Unturned.NPCMaker.Common.Utility;
using System;
using System.IO;
using System.Windows;

namespace BowieD.Unturned.NPCMaker
{
    public sealed class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception e)
            {
                DisplayException(e);
            }
        }

        internal static void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            DisplayException(e.Exception);
        }

        static void DisplayException(Exception e)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("crash.txt", true))
                {
                    writer.WriteLine(DebugUtility.GetDebugInformation());
                    writer.WriteLine();
                    writer.WriteLine(e);
                }
                MessageBox.Show(e.ToString(), "NPC Maker Crashed");
            }
            catch { }
        }
    }
}
