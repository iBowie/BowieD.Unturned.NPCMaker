using BowieD.NPCMaker.Configuration.Attributes;
using BowieD.NPCMaker.Data;

namespace BowieD.NPCMaker.Configuration
{
    public sealed class AppConfig : JsonConfig
    {
        public static AppConfig Instance;

        [ConfigurationField("general")]
        public string locale;
        [ConfigurationField("export")]
        public bool exportGuid;
        [ConfigurationField("graphics")]
        public bool animateControls;

        public override void Load(string filePath)
        {
            base.Load(filePath);
            Instance = this;
        }
        public override void LoadDefaults()
        {
            locale = "en_US";
            exportGuid = true;
            animateControls = true;
        }
    }
}
