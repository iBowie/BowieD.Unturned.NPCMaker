using BowieD.Unturned.NPCMaker.Common.Utility;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Data;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Notification;
using BowieD.Unturned.NPCMaker.Updating;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using static BowieD.Unturned.NPCMaker.Data.AppPackage;

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
        public static AppPackage Package { get; private set; }
        public static Version Version
        {
            get
            {
                try
                {
                    if (_readVersion == null)
                    {
                        _readVersion = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
                    }

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
                Logger.Log($"BowieD.Unturned.NPCMaker {Version}. Copyright (C) 2020 Anton 'BowieD' Galakhov");
                Logger.Log("This program comes with ABSOLUTELY NO WARRANTY; for details type `license w'.");
                Logger.Log("This is free software, and you are welcome to redistribute it");
                Logger.Log("under certain conditions; type `license c' for details.");
                Logger.Log("This programs uses 3rd party apps, type `license l' for details.");
                Logger.Log("[EXTRCT] - Extracting libraries...", ELogLevel.DEBUG);
                #region COPY LIBS
                CopyResource(NPCMaker.Properties.Resources.DiscordRPC, Path.Combine(AppConfig.Directory, "DiscordRPC.dll"));
                CopyResource(NPCMaker.Properties.Resources.Newtonsoft_Json, Path.Combine(AppConfig.Directory, "Newtonsoft.Json.dll"));
                CopyResource(NPCMaker.Properties.Resources.ControlzEx, Path.Combine(AppConfig.Directory, "ControlzEx.dll"));
                CopyResource(NPCMaker.Properties.Resources.MahApps_Metro, Path.Combine(AppConfig.Directory, "MahApps.Metro.dll"));
                CopyResource(NPCMaker.Properties.Resources.Microsoft_Xaml_Behaviors, Path.Combine(AppConfig.Directory, "Microsoft.Xaml.Behaviors.dll"));
                CopyResource(NPCMaker.Properties.Resources.MahApps_Metro_IconPacks_Core, Path.Combine(AppConfig.Directory, "MahApps.Metro.IconPacks.Core.dll"));
                CopyResource(NPCMaker.Properties.Resources.MahApps_Metro_IconPacks_Material, Path.Combine(AppConfig.Directory, "MahApps.Metro.IconPacks.Material.dll"));
                CopyResource(NPCMaker.Properties.Resources.Xceed_Wpf_AvalonDock, Path.Combine(AppConfig.Directory, "Xceed.Wpf.AvalonDock.dll"));
                CopyResource(NPCMaker.Properties.Resources.Xceed_Wpf_AvalonDock_Themes_Aero, Path.Combine(AppConfig.Directory, "Xceed.Wpf.AvalonDock.Themes.Aero.dll"));
                CopyResource(NPCMaker.Properties.Resources.Xceed_Wpf_AvalonDock_Themes_Metro, Path.Combine(AppConfig.Directory, "Xceed.Wpf.AvalonDock.Themes.Metro.dll"));
                CopyResource(NPCMaker.Properties.Resources.Xceed_Wpf_AvalonDock_Themes_VS2010, Path.Combine(AppConfig.Directory, "Xceed.Wpf.AvalonDock.Themes.VS2010.dll"));
                CopyResource(NPCMaker.Properties.Resources.Xceed_Wpf_Toolkit, Path.Combine(AppConfig.Directory, "Xceed.Wpf.Toolkit.dll"));
                #endregion
                Logger.Log("[EXTRCT] - Extraction complete!", ELogLevel.DEBUG);
                AppConfig.Instance.Load();
                #region SCALE
                Resources["Scale"] = AppConfig.Instance.scale;
                #endregion
#if !FAST
                App.UpdateManager = new GitHubUpdateManager();
                var result = App.UpdateManager.CheckForUpdates(AppConfig.Instance.downloadPrerelease).GetAwaiter().GetResult();
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
                {
                    LocalizationManager.LoadLanguage(AppConfig.Instance.language);
                }

                PostRun();
                base.Run();
            }
            else
            {
                Logger.Log("You have to install .NET Framework 4.7.2 to run this app properly.", ELogLevel.CRITICAL);
                Console.ReadKey(true);
            }
        }
        public static void PostRun()
        {
            string packagePath = Path.Combine(AppConfig.Directory, "package.json");

            try
            {
#if DEBUG
                if (Environment.GetCommandLineArgs().Contains("-offline-package"))
                {
                    throw new Exception("Skipping cache downloading");
                }
#endif
                App.Logger.Log("Updating package...");
                using (WebClient client = new WebClient())
                {
                    string data = client.DownloadString(AppPackage.url);

                    Package = JsonConvert.DeserializeObject<AppPackage>(data);

                    File.WriteAllText(packagePath, data);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("Could not load latest version of the package. Loading offline cache...", ex: ex);
                if (File.Exists(packagePath))
                {
                    string data = File.ReadAllText(packagePath);

                    Package = JsonConvert.DeserializeObject<AppPackage>(data);

                    File.WriteAllText(packagePath, data);
                }
                else
                {
                    Logger.Log("Could not find offline cache. Creating placeholder...", ELogLevel.WARNING);
                    Package = new AppPackage()
                    {
                        FeedbackLinks = new FeedbackLink[1]
                        {
                            new FeedbackLink()
                            {
                                Icon = "pack://application:,,,/Resources/Services/GitHub.png",
                                Text = "Main_Menu_Communication_GitHub",
                                Localize = true,
                                URL = "https://github.com/iBowie/BowieD.Unturned.NPCMaker"
                            }
                        }
                    };
                }
            }
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
            {
                return null;
            }

            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(d => d.FullName == args.Name);
            if (assembly != null)
            {
                return assembly;
            }

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
