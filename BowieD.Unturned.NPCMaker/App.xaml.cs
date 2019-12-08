using BowieD.Unturned.NPCMaker.Common.Utility;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Notification;
using BowieD.Unturned.NPCMaker.Updating;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace BowieD.Unturned.NPCMaker
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
#if DEBUG
        internal static ELogLevel LogLevel => ELogLevel.TRACE;
#else
        internal static ELogLevel LogLevel => ELogLevel.CRITICAL;
#endif
        public static IUpdateManager UpdateManager { get; private set; }
        public static INotificationManager NotificationManager { get; private set; }
        public static ILoggingManager Logger { get; private set; }
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
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }
        public new void Run()
        {
            InitLoggers();
            Logger.Log($"Detected .NET {NETHelper.GetVersionString()}");
            if (NETHelper.GetVersion() >= NETVersion.v4_7_2)
            {
                Logger.Log($"User has required .NET Framework version. Launching...", ELogLevel.POSITIVE);
                Logger.Log($"BowieD.Unturned.NPCMaker {Version}. Copyright (C) 2019 Anton 'BowieD' Galakhov");
                Logger.Log("This program comes with ABSOLUTELY NO WARRANTY; for details type `license w'.");
                Logger.Log("This is free software, and you are welcome to redistribute it");
                Logger.Log("under certain conditions; type `license c' for details.");
                Logger.Log("[EXTRCT] - Extracting libraries...", ELogLevel.DEBUG);
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
                Logger.Log("[EXTRCT] - Extraction complete!", ELogLevel.DEBUG);
                AppConfig.Instance.Load();
#region SCALE
                Resources["Scale"] = AppConfig.Instance.scale;
#endregion
#if !FAST
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
                        LocalizationManager.LoadLanguage(AppConfig.Instance.language);
                        var dlg = MessageBox.Show(LocalizationManager.Current.Interface["Update_Available_Body"], LocalizationManager.Current.Interface["Update_Available_Title"], MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (dlg == MessageBoxResult.Yes)
                        {
                            App.UpdateManager.StartUpdate();
                            return;
                        }
                    }
                }
#else
                Logger.Log("[APP] - DebugFast enabled! Skipping update check...", ELogLevel.DEBUG);
#endif
                if (!LocalizationManager.IsLoaded)
                    LocalizationManager.LoadLanguage(AppConfig.Instance.language);
#if DEBUG
                Logger.Log("[APP] - Opening MainWindow...");
#else
                Logger.Log("[APP] - Closing console and opening app...");
#endif
                MainWindow mw = new MainWindow();
                InitManagers();
#if DEBUG
#else
                ConsoleLogger.HideConsoleWindow();
#endif
                mw.Show();
                base.Run();
            }
            else
            {
                Logger.Log("You have to install .NET Framework 4.7.2 to run this app properly.", ELogLevel.CRITICAL);
                Console.ReadKey(true);
            }
        }
        public static void InitLoggers()
        {
            Logger = new LoggingManager();
            Logger.ConnectLogger(new ConsoleLogger());
            Logger.ConnectLogger(new FileLogger());
        }
        public static void InitManagers()
        {
            UpdateManager = new GitHubUpdateManager();
            NotificationManager = new NotificationManager();
        }
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains(".resources"))
                return null;
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(d => d.FullName == args.Name);
            if (assembly != null)
                return assembly;
            string fileName = args.Name.Split(',')[0] + ".dll";
            try
            {
                App.Logger.Log($"Resolving {fileName}", ELogLevel.TRACE);
            }
            catch { }
            string asmFile = Path.Combine(AppConfig.Directory, fileName);
            try
            {
                return Assembly.LoadFrom(asmFile);
            }
            catch { return null; }
        }
        private void CopyResource(byte[] res, string file)
        {
            Logger.Log($"[EXTRCT] - Extracting to {file}", ELogLevel.DEBUG);
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
