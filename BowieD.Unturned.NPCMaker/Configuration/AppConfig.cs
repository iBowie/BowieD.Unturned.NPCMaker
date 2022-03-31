using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Themes;
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
        public bool automaticallyCheckForErrors = false;
        public string[] disabledErrors;
        public bool preferLegacyIDsOverGUIDs = false;
        public bool autoCloseOpenBoomerangs = true;
        public string mainWindowBackgroundImage;
        public double mainWindowBackgroundImageBlurRadius;
        public bool alternateBoolValue;
        public bool forceSoftwareRendering;

        public void Apply(AppConfig from, out bool hasToRestart)
        {
            hasToRestart = false;

            // these cannot be changed that easily
            hasToRestart |= (experimentalFeatures != from.experimentalFeatures);
            hasToRestart |= (scale != from.scale);
            hasToRestart |= (language != from.language);
            hasToRestart |= (autosaveOption != from.autosaveOption);
            hasToRestart |= (autoUpdate != from.autoUpdate);
            hasToRestart |= (downloadPrerelease != from.downloadPrerelease);
            hasToRestart |= (enableDiscord != from.enableDiscord);
            hasToRestart |= (unturnedDir != from.unturnedDir);
            hasToRestart |= (importVanilla != from.importVanilla);
            hasToRestart |= (importWorkshop != from.importWorkshop);
            hasToRestart |= (importHooked != from.importHooked);
            hasToRestart |= (generateThumbnailsBeforehand != from.generateThumbnailsBeforehand);
            hasToRestart |= (useOldStyleMoveUpDown != from.useOldStyleMoveUpDown);
            hasToRestart |= (replaceMissingKeysWithEnglish != from.replaceMissingKeysWithEnglish);
            hasToRestart |= (forceSoftwareRendering != from.forceSoftwareRendering);

            // it has to do some work before it can be applied
            if (currentTheme != from.currentTheme)
            {
                currentTheme = from.currentTheme;

                Theme theme = ThemeManager.Themes.ContainsKey(currentTheme ?? "") ? ThemeManager.Themes[currentTheme] : ThemeManager.Themes["Metro/LightGreen"];
                ThemeManager.Apply(theme);
            }

            if (automaticallyCheckForErrors != from.automaticallyCheckForErrors)
            {
                automaticallyCheckForErrors = from.automaticallyCheckForErrors;

                if (automaticallyCheckForErrors)
                {
                    MainWindow.Instance.statusNoErrorsItem.Visibility = System.Windows.Visibility.Visible;
                    MainWindow.ErrorCheckTimer.Start();
                }
                else
                {
                    MainWindow.Instance.statusNoErrorsItem.Visibility = System.Windows.Visibility.Collapsed;
                    MainWindow.ErrorCheckTimer.Stop();
                }
            }

            if (mainWindowBackgroundImage != from.mainWindowBackgroundImage || mainWindowBackgroundImageBlurRadius != from.mainWindowBackgroundImageBlurRadius)
            {
                mainWindowBackgroundImage = from.mainWindowBackgroundImage;
                mainWindowBackgroundImageBlurRadius = from.mainWindowBackgroundImageBlurRadius;

                MainWindow.Instance.SetBackground(mainWindowBackgroundImage, currentTheme.Substring("Metro/".Length).StartsWith("Dark"));
            }

            // it's enough to just change value to apply it
            useCommentsInsteadOfData = from.useCommentsInsteadOfData;
            exportSchema = from.exportSchema;
            generateGuids = from.generateGuids;
            animateControls = from.animateControls;
            highlightSearch = from.highlightSearch;
            disabledErrors = from.disabledErrors;
            preferLegacyIDsOverGUIDs = from.preferLegacyIDsOverGUIDs;
            autoCloseOpenBoomerangs = from.autoCloseOpenBoomerangs;
            alternateLogicTranslation = from.alternateLogicTranslation;
            alternateBoolValue = from.alternateBoolValue;
        }
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
                
                PostLoad();
                
                Save();
            }
            else
            {
                try
                {
                    App.Logger.Log($"[CFG] - File found. Loading configuration...");
                    string content = File.ReadAllText(path);
                    JsonConvert.PopulateObject(content, this);
                    
                    PostLoad();
                    
                    App.Logger.Log($"[CFG] - Configuration loaded from {path}");
                }
                catch
                {
                    App.Logger.Log($"[CFG] - Could not load configuration from file. Reverting to default...", ELogLevel.WARNING);
                    LoadDefaults();
                    
                    PostLoad();

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
            automaticallyCheckForErrors = false;
            disabledErrors = System.Array.Empty<string>();
            preferLegacyIDsOverGUIDs = false;
            autoCloseOpenBoomerangs = true;
            alternateBoolValue = true;
            forceSoftwareRendering = false;

            App.Logger.Log($"[CFG] - Default configuration loaded!");
        }
        public void PostLoad()
        {
            if (disabledErrors is null)
                disabledErrors = System.Array.Empty<string>();
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
