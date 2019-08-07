namespace BowieD.Unturned.NPCMaker.Configuration
{
    public interface IConfig
    {
        void Save();
        void Load();
        void LoadDefaults();
    }
    //public static class Configuration
    //{
    //    public class CFG
    //    {
    //        public bool experimentalFeatures;
    //        public string[] userColors;
    //        public string[] recent;
    //        public double scale;
    //        public bool firstLaunch;
    //        public string Language;
    //        public bool enableDiscord;
    //        public Theme currentTheme;
    //        public bool generateGuids;
    //        public byte autosaveOption;
    //        public bool animateControls;
    //        public bool autoUpdate;

    //        [XmlIgnore]
    //        public CultureInfo language
    //        {
    //            get
    //            {
    //                return new CultureInfo(Language ?? "en-US") ?? new CultureInfo("en-US");
    //            }
    //            set
    //            {
    //                Language = value.Name;
    //            }
    //        }

    //        public CFG()
    //        {
    //            firstLaunch = true;
    //            scale = 1;
    //            userColors = new string[0];
    //            recent = new string[0];
    //            enableDiscord = true;
    //            currentTheme = DefaultTheme;
    //            generateGuids = true;
    //            autosaveOption = 1;
    //            experimentalFeatures = false;
    //            animateControls = true;
    //            autoUpdate = true;
    //        }
    //    }

    //    public static CFG Properties { get; private set; }
    //    public static Theme DefaultTheme => new MetroTheme() { DictionaryName = "Light.Green", Name = "LightGreen", R = 84, G = 142, B = 25 };

    //    private static string Path => $@"C:{System.IO.Path.DirectorySeparatorChar}Users{System.IO.Path.DirectorySeparatorChar}{Environment.UserName}{System.IO.Path.DirectorySeparatorChar}AppData{System.IO.Path.DirectorySeparatorChar}Local{System.IO.Path.DirectorySeparatorChar}BowieD{System.IO.Path.DirectorySeparatorChar}UnturnedNPCMakerConfig.xml";
    //    public static string ConfigDirectory
    //    {
    //        get
    //        {
    //            var res = System.IO.Path.GetDirectoryName(Path);
    //            if (!res.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
    //                res += System.IO.Path.DirectorySeparatorChar;
    //            return res;
    //        }
    //    }

    //    public static bool ConfigExist => File.Exists(Path);

    //    public static void Load()
    //    {
    //        try
    //        {
    //            using (FileStream fs = new FileStream(Path, FileMode.Open))
    //            using (XmlReader reader = XmlReader.Create(fs))
    //            {
    //                XmlSerializer ser = new XmlSerializer(typeof(CFG));
    //                var content = ser.Deserialize(reader) as CFG;
    //                Properties = content;
    //                Properties.currentTheme.Apply();
    //            }
    //        }
    //        catch { Logger.Log("Can't load configuration. Loading defaults...", Log_Level.Error); LoadDefaults(); }
    //    }
    //    public static void Save()
    //    {
    //        Logger.Log("Saving configuration...");
    //        var dir = ConfigDirectory;
    //        if (!Directory.Exists(dir))
    //            Directory.CreateDirectory(dir);
    //        using (FileStream fs = new FileStream(Path, FileMode.Create))
    //        using (XmlWriter writer = XmlWriter.Create(fs, new XmlWriterSettings() { Indent = true, IndentChars = "\t" }))
    //        {
    //            XmlSerializer ser = new XmlSerializer(typeof(CFG));
    //            ser.Serialize(writer, Properties);
    //        }
    //        Logger.Log("Configuration saved!");
    //    }
    //    public static void LoadDefaults()
    //    {
    //        Properties = GetDefaults();
    //    }
    //    public static CFG GetDefaults() => new CFG();
    //    public static void Force(CFG newConfig)
    //    {
    //        Properties = newConfig;
    //    }
    //}
}
