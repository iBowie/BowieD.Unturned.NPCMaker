using BowieD.Unturned.NPCMaker.Common.Utility;
using System;
using System.IO;
using System.Threading.Tasks;
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
                SetupExceptionHandling();
                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception e)
            {
                DisplayException(e);
                SaveToCrashException(e);
            }
        }

        internal static void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            DisplayException(e.Exception);
            SaveToCrashException(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            DisplayException((Exception)e.ExceptionObject);
            SaveToCrashException((Exception)e.ExceptionObject);
        }

        static void DisplayException(Exception e)
        {
            try
            {
                MessageBox.Show(e.ToString(), "NPC Maker Crashed");
            }
            catch { }
        }
        static void SaveToCrashException(Exception e)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("crash.txt", true))
                {
                    writer.WriteLine(DebugUtility.GetDebugInformation());
                    writer.WriteLine();
                    writer.WriteLine(e);
                }
            }
            catch { }
        }
    }
}
