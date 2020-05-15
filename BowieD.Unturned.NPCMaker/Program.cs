using BowieD.Unturned.NPCMaker.Common.Utility;
using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker
{
    public sealed class Program
    {
        [STAThread]
        private static void Main()
        {
            try
            {
                SetupExceptionHandling();
                App app = new App();
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

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            DisplayException(e.Exception);
            SaveToCrashException(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            DisplayException((Exception)e.ExceptionObject);
            SaveToCrashException((Exception)e.ExceptionObject);
        }

        private static void DisplayException(Exception e)
        {
            const string caption = "NPC Maker Crashed";

            try
            {
                switch (e)
                {
                    case SecurityException _:
                    case UnauthorizedAccessException _:
                        MessageBox.Show($"Security exception.\nTry running the app with admin privileges.\n{e}", caption);
                        break;
                    default:
                        MessageBox.Show(e.ToString(), caption);
                        break;
                }
            }
            catch { }
        }

        private static void SaveToCrashException(Exception e)
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
