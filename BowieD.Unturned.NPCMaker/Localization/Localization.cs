namespace BowieD.Unturned.NPCMaker.Localization
{
    public sealed class Localization
    {
        public Localization()
        {
            Name = "Unknown";
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
        public string Name;
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
        public TranslationDictionary GetDictionary(string name)
        {
            switch (name.ToLower())
            {
                case "character":
                    return Character;
                default:
                    return null;
            }
        }
        public override string ToString()
        {
            return $"Localization {Name}. Author(s): {Author}";
        }
    }
}
