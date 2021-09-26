using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
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
        public ELanguage language;
        public EExportSchema exportSchema;
        public bool enableDiscord;
        public string currentTheme;
        public bool generateGuids;
        public byte autosaveOption;
        public bool animateControls;
        public bool autoUpdate;
        public bool downloadPrerelease;
        public bool alternateLogicTranslation;
        public bool replaceMissingKeysWithEnglish;
        public bool useCommentsInsteadOfData;
        public string unturnedDir;
        public bool importVanilla = true;
        public bool importWorkshop = true;
        public bool importHooked = true;
        public bool generateThumbnailsBeforehand = true;
        public bool highlightSearch;
        public bool useOldStyleMoveUpDown;

        public void Save()
        {
            App.Logger.Log($"[CFG] - Saving configuration to {path}");
            string content = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, content);
            App.Logger.Log($"[CFG] - Saving complete!");
        }
        public void Load()
        {
            App.Logger.Log($"[CFG] - Loading configuration from {path}");
            if (!File.Exists(path))
            {
                App.Logger.Log($"[CFG] - File not found. Creating one...");
                LoadDefaults();
                Save();
            }
            else
            {
                try
                {
                    App.Logger.Log($"[CFG] - File found. Loading configuration...");
                    string content = File.ReadAllText(path);
                    JsonConvert.PopulateObject(content, this);
                    App.Logger.Log($"[CFG] - Configuration loaded from {path}");
                }
                catch
                {
                    App.Logger.Log($"[CFG] - Could not load configuration from file. Reverting to default...", ELogLevel.WARNING);
                    LoadDefaults();
                    Save();
                }
            }
        }
        public void LoadDefaults()
        {
            App.Logger.Log($"[CFG] - Loading default configuration...");
            scale = 1;
            enableDiscord = true;
            currentTheme = "Metro/LightGreen";
            generateGuids = true;
            autosaveOption = 1;
            experimentalFeatures = false;
            animateControls = true;
            autoUpdate = true;
            downloadPrerelease = false;
            alternateLogicTranslation = false;
            replaceMissingKeysWithEnglish = true;
            useCommentsInsteadOfData = false;
            unturnedDir = null;
            importVanilla = true;
            importWorkshop = true;
            importHooked = true;
            generateThumbnailsBeforehand = true;
            highlightSearch = false;
            useOldStyleMoveUpDown = false;
            ELanguage c = LocalizationManager.GetLanguageFromCultureInfo(CultureInfo.InstalledUICulture);
            if (LocalizationManager.SupportedLanguages().Contains(c))
            {
                language = c;
            }
            else
            {
                language = ELanguage.English;
            }
            exportSchema = EExportSchema.Default;

            App.Logger.Log($"[CFG] - Default configuration loaded!");
        }
        private static readonly string defaultDir = Path.Combine($@"{Environment.SystemDirectory[0]}{Path.VolumeSeparatorChar}{Path.DirectorySeparatorChar}", "Users", Environment.UserName, "AppData", "Local", "BowieD", "NPCMaker");
        public static string Directory
        {
            get
            {
                string res = defaultDir;

                if (!System.IO.Directory.Exists(res))
                {
                    System.IO.Directory.CreateDirectory(res);
                }

                return res;
            }
        }
        public static string ExeDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(exeDir))
                {
                    exeDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                }

                return exeDir;
            }
        }
        private static string exeDir;
        private static string path => Path.Combine(Directory, "config.json");
    }
}
