using BowieD.Unturned.NPCMaker.Common.Utility;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Data;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Notification;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Updating;
using BowieD.Unturned.NPCMaker.Workshop;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using static BowieD.Unturned.NPCMaker.Data.AppPackage;

namespace BowieD.Unturned.NPCMaker
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal const string CRASH_SAVE_FILENAME = "crashSave.npcproj";
#if DEBUG
        internal static ELogLevel LogLevel => ELogLevel.TRACE;
#else
        internal static ELogLevel LogLevel => ELogLevel.CRITICAL;
#endif
        internal static bool IsPreviewVersion
        {
            get
            {
#if PREVIEW
                return true;
#else
                return false;
#endif
            }
        }
        public static ISteamManager SteamManager { get; private set; }
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
            SetupExceptionHandling();
            Directory.SetCurrentDirectory(AppConfig.ExeDirectory);

            InitLoggers();
            Logger.Log($"Detected .NET {NETHelper.GetVersionString()}");
            NETVersion netVersion = NETHelper.GetVersion();
            NETVersion checkVersion = NETVersion.v4_7_2;

            if (checkVersion > netVersion)
                Logger.Log("You have to install .NET Framework 4.7.2 to run this app properly.", ELogLevel.CRITICAL);

            Logger.Log($"BowieD.Unturned.NPCMaker {Version}. Copyright (C) 2018 - 2022 Anton 'BowieD' Galakhov");
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
            CopyResource(NPCMaker.Properties.Resources.Steamworks_NET, Path.Combine(AppConfig.Directory, "Steamworks.NET.dll"));
            CopyResource(NPCMaker.Properties.Resources.steam_api, Path.Combine(AppConfig.Directory, "steam_api.dll"));
            CopyResource(NPCMaker.Properties.Resources.UnturnedWorkshopCLI, Path.Combine(AppConfig.Directory, "UnturnedWorkshopCLI.exe"));
            File.WriteAllText(Path.Combine(AppConfig.Directory, "steam_appid.txt"), NPCMaker.Properties.Resources.steam_appid);
            CopyResource(NPCMaker.Properties.Resources.Microsoft_WindowsAPICodePack, Path.Combine(AppConfig.Directory, "Microsoft.WindowsAPICodePack.dll"));
            CopyResource(NPCMaker.Properties.Resources.Microsoft_WindowsAPICodePack_Shell, Path.Combine(AppConfig.Directory, "Microsoft.WindowsAPICodePack.Shell.dll"));
            #endregion
            Logger.Log("[EXTRCT] - Extraction complete!", ELogLevel.DEBUG);
            AppConfig.Instance.Load();
            #region SCALE
            Resources["Scale"] = AppConfig.Instance.scale;
            #endregion
#if !FAST
            App.UpdateManager = new GitHubUpdateManager();
            var result = App.UpdateManager.CheckForUpdates(IsPreviewVersion || AppConfig.Instance.downloadPrerelease).GetAwaiter().GetResult();
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
                    var dlg = System.Windows.MessageBox.Show(LocalizationManager.Current.Interface["Update_Available_Body"], LocalizationManager.Current.Interface["Update_Available_Title"], System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
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

            try
            {
                PostRun();
                base.Run();
            }
            catch (Exception e)
            {
                TryToSaveProject();
                DisplayException(e);
                SaveToCrashException(e);
                ForceExit();
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

                    if (Package is null)
                        throw new NullReferenceException("Could not properly download package file. Falling back to the local cache.");

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

                    if (Package is null)
                    {
                        Logger.Log("Could not properly load locally cached package file. Falling back to the runtime-created one.", ELogLevel.WARNING);

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
                    else
                    {
                        File.WriteAllText(packagePath, data);
                    }
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

            if (AppConfig.Instance.forceSoftwareRendering)
            {
                Logger.Log("[APP] - Software rendering only mode is enabled!");
                System.Windows.Media.RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;
            }

            MainWindow mw = new MainWindow();
            InitManagers();
            Application.Current.MainWindow = mw;
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            mw.Show();
        }
        public static void InitLoggers()
        {
            Logger = new LoggingManager();
            Logger.ConnectLogger(new FileLogger());
            Logger.ConnectLogger(new DelegateLogger());
        }
        public static void InitManagers()
        {
            NotificationManager = new NotificationManager();

            SteamManager = new SteamManager();
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

        #region Exception Handling
        private static void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            TryToSaveProject();
            DisplayException(e.Exception);
            SaveToCrashException(e.Exception);
            ForceExit();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            TryToSaveProject();
            DisplayException((Exception)e.ExceptionObject);
            SaveToCrashException((Exception)e.ExceptionObject);
            ForceExit();
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
                using (StreamWriter writer = new StreamWriter(Path.Combine(AppConfig.ExeDirectory, "crash.txt"), true))
                {
                    writer.WriteLine(DebugUtility.GetDebugInformation());
                    writer.WriteLine();
                    writer.WriteLine(e);
                }
            }
            catch { }
        }

        private static void TryToSaveProject()
        {
            try
            {
                ProjectData.CurrentProject.file = Path.Combine(AppConfig.ExeDirectory, CRASH_SAVE_FILENAME);

                ProjectData.CurrentProject.DoSave();
            }
            catch { }
        }

        private static void ForceExit()
        {
            Environment.Exit(1);
        }
        #endregion
    }
}
