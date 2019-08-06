using Newtonsoft.Json;
using System;
using System.IO;

namespace BowieD.Unturned.NPCMaker.Config
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
            string content = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, content);
        }
        public void Load()
        {
            if (!File.Exists(path))
            {
                LoadDefaults();
                Save();
            }
            else
            {
                string content = File.ReadAllText(path);
                JsonConvert.PopulateObject(content, this);
            }
        }
        public void LoadDefaults()
        {
            scale = 1;
            enableDiscord = true;
            currentTheme = "Metro/LightGreen";
            generateGuids = true;
            autosaveOption = 1;
            experimentalFeatures = false;
            animateControls = true;
            autoUpdate = true;
            locale = "en-US";
        }
        public static string Directory
        {
            get
            {
                string res = $@"C{Path.VolumeSeparatorChar}Users{Path.DirectorySeparatorChar}{Environment.UserName}{Path.DirectorySeparatorChar}AppData{Path.DirectorySeparatorChar}Local{Path.DirectorySeparatorChar}BowieD{Path.DirectorySeparatorChar}NPCMaker{Path.DirectorySeparatorChar}";
                if (!System.IO.Directory.Exists(res))
                    System.IO.Directory.CreateDirectory(res);
                return res;
            }
        }
        private static string path => Directory + "config.json";
    }
}
