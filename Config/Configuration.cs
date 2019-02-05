using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.Config
{
    public static class Configuration
    {
        public class CFG
        {
            public bool experimentalFeatures;
            public bool[] autosaveParams;
            public string[] userColors;
            public string[] recent;
            public double scale;
            public bool firstLaunch;
            public string Language;

            [XmlIgnore]
            public CultureInfo language => new CultureInfo(Language);

            public CFG()
            {
                firstLaunch = true;
                scale = 1;
                autosaveParams = new bool[4];
                userColors = new string[0];
                recent = new string[0];
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
            catch { LoadDefaults(); }
        }
        public static void Save()
        {
            using (FileStream fs = new FileStream(Path, FileMode.Create))
            using (XmlWriter writer = XmlWriter.Create(fs, new XmlWriterSettings() { Indent = true, IndentChars = "\t" }))
            {
                XmlSerializer ser = new XmlSerializer(typeof(CFG));
                ser.Serialize(writer, Properties);
            }
        }
        public static void LoadDefaults()
        {
            Properties = new CFG();
        }
        public static void Force(CFG newConfig)
        {
            Properties = newConfig;
        }

        public static CFG ConvertFromOldToNew => new CFG()
        {
            autosaveParams = NPCMaker.Properties.Settings.Default.autosaveParams,
            experimentalFeatures = NPCMaker.Properties.Settings.Default.experimentalFeatures,
            firstLaunch = NPCMaker.Properties.Settings.Default.firstLaunch,
            Language = NPCMaker.Properties.Settings.Default.language.Name,
            recent = NPCMaker.Properties.Settings.Default.recent,
            scale = NPCMaker.Properties.Settings.Default.scale,
            userColors = NPCMaker.Properties.Settings.Default.userColors
        };
    }
}
