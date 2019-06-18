using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Managers;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public App()
        {
            InitializeComponent();
            #if !DEBUG
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            #endif
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Logger.Clear();
            Logger.Log("App started! Pre-launch stage.");
            Logger.Log("Loading configuration...");
            Config.Configuration.Load();
            Logger.Log("Configuration loaded!");
            if (Config.Configuration.Properties.firstLaunch || Config.Configuration.Properties.language == null)
            {
                Logger.Log("First launch! Detecting language...");
                if (LocUtil.SupportedCultures().Contains(CultureInfo.InstalledUICulture))
                {
                    Config.Configuration.Properties.language = CultureInfo.InstalledUICulture;
                }
                else
                {
                    Config.Configuration.Properties.language = new CultureInfo("en-US");
                }
            }
            #region SCALE
            Resources["Scale"] = Config.Configuration.Properties.scale;
            Logger.Log($"Scale set to {Config.Configuration.Properties.scale}");
            #endregion
            Config.Configuration.Save();

            Util.UpdateManager = new GitHubUpdateManager();
            Util.UpdateManager.CheckForUpdates().GetAwaiter().GetResult();
            if (Util.UpdateManager.UpdateAvailability == UpdateAvailability.AVAILABLE)
            {
                Util.UpdateManager.StartUpdate();
                return;
            }
            #region COPY LIBS
            CopyResource(NPCMaker.Properties.Resources.DiscordRPC, Config.Configuration.ConfigDirectory + "DiscordRPC.dll");
            CopyResource(NPCMaker.Properties.Resources.Newtonsoft_Json, Config.Configuration.ConfigDirectory + "Newtonsoft.Json.dll");
            CopyResource(NPCMaker.Properties.Resources.ControlzEx, Config.Configuration.ConfigDirectory + "ControlzEx.dll");
            CopyResource(NPCMaker.Properties.Resources.MahApps_Metro, Config.Configuration.ConfigDirectory + "MahApps.Metro.dll");
            CopyResource(NPCMaker.Properties.Resources.Microsoft_Xaml_Behaviors, Config.Configuration.ConfigDirectory + "Microsoft.Xaml.Behaviors.dll");
            CopyResource(NPCMaker.Properties.Resources.MahApps_Metro_IconPacks_Core, Config.Configuration.ConfigDirectory + "MahApps.Metro.IconPacks.Core.dll");
            CopyResource(NPCMaker.Properties.Resources.MahApps_Metro_IconPacks_Material, Config.Configuration.ConfigDirectory + "MahApps.Metro.IconPacks.Material.dll");
            #endregion
            LocUtil.LoadLanguage(Config.Configuration.Properties.Language);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains(".resources"))
                return null;
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(d => d.FullName == args.Name);
            if (assembly != null)
                return assembly;
            string fileName = args.Name.Split(',')[0] + ".dll";
            string asmFile = Path.Combine(Config.Configuration.ConfigDirectory, fileName);
            try
            {
                return Assembly.LoadFrom(asmFile);
            }
            catch { return null; }
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            AppCrashReport acr = new AppCrashReport(e.Exception);
            acr.ShowDialog();
            e.Handled = acr.Handle;
            if (acr.Handle)
                Logger.Log($"Ignoring exception {e.Exception.Message}.");
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
    }
}
