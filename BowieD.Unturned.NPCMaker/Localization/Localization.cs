﻿namespace BowieD.Unturned.NPCMaker.Localization
{
    public sealed class Localization
    {
        public Localization()
        {
            Author = "Unknown";
            LastUpdate = "Never";
            General = new TranslationDictionary();
            Options = new TranslationDictionary();
            Character = new TranslationDictionary();
            Dialogue = new TranslationDictionary();
            Vendor = new TranslationDictionary();
            Quest = new TranslationDictionary();
            Condition = new TranslationDictionary();
            Reward = new TranslationDictionary();
            VendorItem = new TranslationDictionary();
            Mistakes = new TranslationDictionary();
            Notification = new TranslationDictionary();
            Interface = new TranslationDictionary();
        }
        public string Author;
        public string LastUpdate;
        public TranslationDictionary General;
        public TranslationDictionary Options;
        public TranslationDictionary Character;
        public TranslationDictionary Dialogue;
        public TranslationDictionary Vendor;
        public TranslationDictionary Quest;
        public TranslationDictionary Condition;
        public TranslationDictionary Reward;
        public TranslationDictionary VendorItem;
        public TranslationDictionary Mistakes;
        public TranslationDictionary Notification;
        public TranslationDictionary Interface;
        public override string ToString()
        {
            return $"Localization. Author: {Author}";
        }
    }
}