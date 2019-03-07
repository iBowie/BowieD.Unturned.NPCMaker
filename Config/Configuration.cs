using BowieD.Unturned.NPCMaker.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.Config
{
    public static class Configuration
    {
        public class CFG
        {
            public bool experimentalFeatures;
            public string[] userColors;
            public string[] recent;
            public double scale;
            public bool firstLaunch;
            public string Language;
            public bool enableDiscord;
            public string theme;
            public bool generateGuids;
            public byte autosaveOption;
            public Logging.Log_Level LogLevel;

            [XmlIgnore]
            public CultureInfo language => new CultureInfo(Language ?? "en-US");

            public CFG()
            {
                firstLaunch = true;
                scale = 1;
                userColors = new string[0];
                recent = new string[0];
                enableDiscord = true;
                theme = "DarkGreen";
                generateGuids = true;
                autosaveOption = 1;
                experimentalFeatures = false;
                LogLevel = Logging.Log_Level.Normal;
            }
        }

        public static CFG Properties { get; private set; }

        private static string Path => $@"C:\Users\{Environment.UserName}\AppData\Local\BowieD\UnturnedNPCMakerConfig.xml";
        public static bool ConfigExist => File.Exists(Path);

        public static void Load()
        {
            try
            {
                using (FileStream fs = new FileStream(Path, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(CFG));
                    var content = ser.Deserialize(reader) as CFG;
                    Properties = content;
                }
            }
            catch { Logger.Log("Can't load configuration. Loading defaults...", Log_Level.Errors); LoadDefaults(); }
        }
        public static void Save()
        {
            Logger.Log("Saving configuration...");
            var dir = System.IO.Path.GetDirectoryName(Path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            using (FileStream fs = new FileStream(Path, FileMode.Create))
            using (XmlWriter writer = XmlWriter.Create(fs, new XmlWriterSettings() { Indent = true, IndentChars = "\t" }))
            {
                XmlSerializer ser = new XmlSerializer(typeof(CFG));
                ser.Serialize(writer, Properties);
            }
            Logger.Log("Configuration saved!");
        }
        public static void LoadDefaults()
        {
            Properties = new CFG();
        }
        public static void Force(CFG newConfig)
        {
            Properties = newConfig;
        }
    }
}
