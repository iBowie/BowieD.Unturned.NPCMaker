using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Notification;
using BowieD.Unturned.NPCMaker.Updating;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace BowieD.Unturned.NPCMaker
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IUpdateManager UpdateManager { get; private set; }
        public static INotificationManager NotificationManager { get; private set; }
        public static List<ILogger> Logger { get; private set; } = new List<ILogger>();
        public static Version Version
        {
            get
            {
                try
                {
                    if (_readVersion == null)
                        _readVersion = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
                    return _readVersion;
                }
                catch { return new Version("0.0.0.0"); }
            }
        }
        public App()
        {
            InitializeComponent();
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }
        public new void Run()
        {
            InitLoggers();
            Logger.LogInfo("Copying libraries...");
            #region COPY LIBS
            CopyResource(NPCMaker.Properties.Resources.DiscordRPC, AppConfig.Directory + "DiscordRPC.dll");
            CopyResource(NPCMaker.Properties.Resources.Newtonsoft_Json, AppConfig.Directory + "Newtonsoft.Json.dll");
            CopyResource(NPCMaker.Properties.Resources.ControlzEx, AppConfig.Directory + "ControlzEx.dll");
            CopyResource(NPCMaker.Properties.Resources.MahApps_Metro, AppConfig.Directory + "MahApps.Metro.dll");
            CopyResource(NPCMaker.Properties.Resources.Microsoft_Xaml_Behaviors, AppConfig.Directory + "Microsoft.Xaml.Behaviors.dll");
            CopyResource(NPCMaker.Properties.Resources.MahApps_Metro_IconPacks_Core, AppConfig.Directory + "MahApps.Metro.IconPacks.Core.dll");
            CopyResource(NPCMaker.Properties.Resources.MahApps_Metro_IconPacks_Material, AppConfig.Directory + "MahApps.Metro.IconPacks.Material.dll");
            CopyResource(NPCMaker.Properties.Resources.Xceed_Wpf_AvalonDock, AppConfig.Directory + "Xceed.Wpf.AvalonDock.dll");
            CopyResource(NPCMaker.Properties.Resources.Xceed_Wpf_AvalonDock_Themes_Aero, AppConfig.Directory + "Xceed.Wpf.AvalonDock.Themes.Aero.dll");
            CopyResource(NPCMaker.Properties.Resources.Xceed_Wpf_AvalonDock_Themes_Metro, AppConfig.Directory + "Xceed.Wpf.AvalonDock.Themes.Metro.dll");
            CopyResource(NPCMaker.Properties.Resources.Xceed_Wpf_AvalonDock_Themes_VS2010, AppConfig.Directory + "Xceed.Wpf.AvalonDock.Themes.VS2010.dll");
            CopyResource(NPCMaker.Properties.Resources.Xceed_Wpf_Toolkit, AppConfig.Directory + "Xceed.Wpf.Toolkit.dll");
            #endregion
            Logger.LogInfo("Copying complete!");
            AppConfig.Instance.Load();
            #region SCALE
            Resources["Scale"] = AppConfig.Instance.scale;
            #endregion
            App.UpdateManager = new GitHubUpdateManager();
            var result = App.UpdateManager.CheckForUpdates().GetAwaiter().GetResult();
            if (result == UpdateAvailability.AVAILABLE)
            {
                if (AppConfig.Instance.autoUpdate)
                {
                    App.UpdateManager.StartUpdate();
                    return;
                }
                else
                {
                    LocUtil.LoadLanguage(AppConfig.Instance.locale);
                    var dlg = MessageBox.Show(LocUtil.LocalizeInterface("update_available_body"), LocUtil.LocalizeInterface("update_available_title"), MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (dlg == MessageBoxResult.Yes)
                    {
                        App.UpdateManager.StartUpdate();
                        return;
                    }
                }
            }
            if (!LocUtil.IsLoaded)
                LocUtil.LoadLanguage(AppConfig.Instance.locale);
            Logger.LogInfo("Closing console and opening app...");
            MainWindow mw = new MainWindow();
            InitManagers();
            ConsoleLogger.HideConsoleWindow();
            mw.Show();
            base.Run();
        }
        public static void InitLoggers()
        {
            Logger.Add(new ConsoleLogger());
            Logger.Add(new FileLogger());
            Logger.Open();
        }
        public static void InitManagers()
        {
            UpdateManager = new GitHubUpdateManager();
            NotificationManager = new NotificationManager();
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            AppCrashReport acr = new AppCrashReport(e.Exception);
            acr.ShowDialog();
            e.Handled = acr.Handle;
            if (acr.Handle)
                App.Logger.LogWarning($"Ignoring exception {e.Exception.Message}.");
        }
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains(".resources"))
                return null;
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(d => d.FullName == args.Name);
            if (assembly != null)
                return assembly;
            string fileName = args.Name.Split(',')[0] + ".dll";
            string asmFile = Path.Combine(AppConfig.Directory, fileName);
            try
            {
                return Assembly.LoadFrom(asmFile);
            }
            catch { return null; }
        }
        private void CopyResource(byte[] res, string file)
        {
            try
            {
                using (Stream output = File.OpenWrite(file))
                {
                    output.Write(res, 0, res.Length);
                }
            }
            catch { }
        }
        private static Version _readVersion = null;
    }
}
