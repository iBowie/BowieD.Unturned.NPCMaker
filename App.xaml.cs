using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace BowieD.Unturned.NPCMaker
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static List<CultureInfo> languages = new List<CultureInfo>();

        public static IList<CultureInfo> Languages
        {
            get
            {
                return languages;
            }
        }
        
        public App()
        {
            InitializeComponent();
            #if !DEBUG
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            #endif
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Logger.Clear();
            Logger.Log("App started! Pre-launch stage.");
            App.LanguageChanged += App_LanguageChanged;
            languages.Clear();
            languages.Add(new CultureInfo("en-US"));
            languages.Add(new CultureInfo("ru-RU"));
            //languages.Add(new CultureInfo("de-DE")); // German
            //languages.Add(new CultureInfo("es-ES")); // Spanish
            Logger.Log("Loading configuration...");
            Config.Configuration.Load();
            Logger.Log("Configuration loaded!");
            if (Config.Configuration.Properties.firstLaunch)
            {
                Logger.Log("First launch! Detecting language...");
                if (languages.Contains(CultureInfo.InstalledUICulture))
                {
                    Language = CultureInfo.InstalledUICulture;
                }
                else
                {
                    Language = new CultureInfo("en-US");
                }
                Config.Configuration.Properties.language = Language;
            }
            #region SCALE
            Resources["Scale"] = Config.Configuration.Properties.scale;
            Logger.Log($"Scale set to {Config.Configuration.Properties.scale}");
            #endregion
            Config.Configuration.Save();
            CopyResource(NPCMaker.Properties.Resources.DiscordRPC, Config.Configuration.ConfigDirectory + "DiscordRPC.dll");
            CopyResource(NPCMaker.Properties.Resources.Newtonsoft_Json, Config.Configuration.ConfigDirectory + "Newtonsoft.Json.dll");
            CopyResource(NPCMaker.Properties.Resources.ControlzEx, Config.Configuration.ConfigDirectory + "ControlzEx.dll");
            CopyResource(NPCMaker.Properties.Resources.MahApps_Metro, Config.Configuration.ConfigDirectory + "MahApps.Metro.dll");
            CopyResource(NPCMaker.Properties.Resources.Microsoft_Xaml_Behaviors, Config.Configuration.ConfigDirectory + "Microsoft.Xaml.Behaviors.dll");
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

        private void App_LanguageChanged(object sender, EventArgs e)
        {
            Config.Configuration.Properties.language = Language;
        }

        public static event EventHandler LanguageChanged;

        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;
                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "ru-RU":
                    case "es-ES":
                        dict.Source = new Uri(String.Format("Localization/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Localization/lang.xaml", UriKind.Relative);
                        break;
                }
                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Localization/lang.")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }
                LanguageChanged(Application.Current, new EventArgs());
            }
        }
    }
}
