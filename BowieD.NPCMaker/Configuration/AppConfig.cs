﻿using BowieD.NPCMaker.Data;

namespace BowieD.NPCMaker.Configuration
{
    public sealed class AppConfig : JsonConfig
    {
        public static AppConfig Instance;

        public string locale;
        public bool exportGuid;

        public override void Load(string filePath)
        {
            base.Load(filePath);
            Instance = this;
        }
        public override void LoadDefaults()
        {
            locale = "en_US";
            exportGuid = true;
        }
    }
}
