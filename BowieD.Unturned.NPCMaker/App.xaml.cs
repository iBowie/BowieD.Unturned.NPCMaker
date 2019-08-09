using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Updating;
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
            Logger.Log("Loading configuration...");
            AppConfig.Instance.Load();
            #region SCALE
            Resources["Scale"] = AppConfig.Instance.scale;
            Logger.Log($"Scale set to {AppConfig.Instance.scale}");
            #endregion
            Util.UpdateManager = new GitHubUpdateManager();
            Util.UpdateManager.CheckForUpdates().GetAwaiter().GetResult();
            if (Util.UpdateManager.UpdateAvailability == UpdateAvailability.AVAILABLE)
            {
                if (AppConfig.Instance.autoUpdate)
                {
                    Util.UpdateManager.StartUpdate();
                    return;
                }
                else
                {
                    LocUtil.LoadLanguage(AppConfig.Instance.locale);
                    var dlg = MessageBox.Show(LocUtil.LocalizeInterface("update_available_body"), LocUtil.LocalizeInterface("update_available_title"), MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (dlg == MessageBoxResult.Yes)
                    {
                        Util.UpdateManager.StartUpdate();
                        return;
                    }
                }
            }
            if (!LocUtil.IsLoaded)
                LocUtil.LoadLanguage(AppConfig.Instance.locale);
            MainWindow mw = new MainWindow();
            mw.Show();
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
