using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Configuration
{
    public class AppConfig : IConfig
    {
        public static AppConfig Instance { get; private set; } = new AppConfig();

        public bool experimentalFeatures;
        public double scale;
        public string locale;
        public bool enableDiscord;
        public string currentTheme;
        public bool generateGuids;
        public byte autosaveOption;
        public bool animateControls;
        public bool autoUpdate;

        public void Save()
        {
            App.Logger.LogInfo($"[CFG] - Saving configuration to {path}");
            string content = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, content);
            App.Logger.LogInfo($"[CFG] - Saving complete!");
        }
        public void Load()
        {
            App.Logger.LogInfo($"[CFG] - Loading configuration from {path}");
            if (!File.Exists(path))
            {
                App.Logger.LogInfo($"[CFG] - File not found. Creating one...");
                LoadDefaults();
                Save();
            }
            else
            {
                try
                {
                    App.Logger.LogInfo($"[CFG] - File found. Loading configuration...");
                    string content = File.ReadAllText(path);
                    JsonConvert.PopulateObject(content, this);
                    App.Logger.LogInfo($"[CFG] - Configuration loaded from {path}");
                }
                catch
                {
                    App.Logger.LogWarning($"[CFG] - Could not load configuration from file. Reverting to default...");
                    LoadDefaults();
                    Save();
                }
            }
        }
        public void LoadDefaults()
        {
            App.Logger.LogInfo($"[CFG] - Loading default configuration...");
            scale = 1;
            enableDiscord = true;
            currentTheme = "Metro/LightGreen";
            generateGuids = true;
            autosaveOption = 1;
            experimentalFeatures = false;
            animateControls = true;
            autoUpdate = true;
            locale = LocUtil.SupportedCultures().Contains(CultureInfo.InstalledUICulture) ? CultureInfo.InstalledUICulture.Name : "en-US";
            App.Logger.LogInfo($"[CFG] - Default configuration loaded!");
        }
        public static string Directory
        {
            get
            {
                string res = $@"C{Path.VolumeSeparatorChar}{Path.DirectorySeparatorChar}Users{Path.DirectorySeparatorChar}{Environment.UserName}{Path.DirectorySeparatorChar}AppData{Path.DirectorySeparatorChar}Local{Path.DirectorySeparatorChar}BowieD{Path.DirectorySeparatorChar}NPCMaker{Path.DirectorySeparatorChar}";
                if (!System.IO.Directory.Exists(res))
                    System.IO.Directory.CreateDirectory(res);
                return res;
            }
        }
        private static string path => Directory + "config.json";
    }
}
