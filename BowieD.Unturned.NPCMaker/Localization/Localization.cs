using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Localization
{
    public sealed class Localization
    {
        public Localization()
        {
            Name = "Unknown";
            Author = "Unknown";
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
            Simulation = new Dictionary<string, TranslationDictionary>();
        }
        public string Name;
        public string Author;
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
        public Dictionary<string, TranslationDictionary> Simulation;
        public TranslationDictionary GetDictionary(string name)
        {
            switch (name.ToLower())
            {
                case "general":
                    return General;
                case "options":
                    return Options;
                case "character":
                    return Character;
                case "dialogue":
                    return Dialogue;
                case "vendor":
                    return Vendor;
                case "quest":
                    return Quest;
                case "condition":
                    return Condition;
                case "reward":
                    return Reward;
                case "vendoritem":
                    return VendorItem;
                case "mistakes":
                    return Mistakes;
                case "notification":
                    return Notification;
                case "interface":
                    return Interface;
                case "simulation.dialogue":
                    return Simulation["Dialogue"];
                case "simulation.vendor":
                    return Simulation["Vendor"];
                case "simulation.quest":
                    return Simulation["Quest"];
                case "simulation.simulation":
                    return Simulation["Simulation"];
                case "simulation.flags":
                    return Simulation["Flags"];
                case "simulation.quests":
                    return Simulation["Quests"];
                case "simulation.inventory":
                    return Simulation["Inventory"];
                default:
                    return null;
            }
        }
        public override string ToString()
        {
            return $"Localization {Name}. Author(s): {Author}";
        }

        public void AddMissingKeys(Localization from)
        {
            TranslationDictionary.AddMissingKeys(General, from.General);
            TranslationDictionary.AddMissingKeys(Options, from.Options);
            TranslationDictionary.AddMissingKeys(Character, from.Character);
            TranslationDictionary.AddMissingKeys(Dialogue, from.Dialogue);
            TranslationDictionary.AddMissingKeys(Vendor, from.Vendor);
            TranslationDictionary.AddMissingKeys(Quest, from.Quest);
            TranslationDictionary.AddMissingKeys(Condition, from.Condition);
            TranslationDictionary.AddMissingKeys(Reward, from.Reward);
            TranslationDictionary.AddMissingKeys(VendorItem, from.VendorItem);
            TranslationDictionary.AddMissingKeys(Mistakes, from.Mistakes);
            TranslationDictionary.AddMissingKeys(Notification, from.Notification);
            TranslationDictionary.AddMissingKeys(Interface, from.Interface);
            try
            {
                TranslationDictionary.AddMissingKeys(Simulation["Dialogue"], from.Simulation["Dialogue"]);
                TranslationDictionary.AddMissingKeys(Simulation["Vendor"], from.Simulation["Vendor"]);
                TranslationDictionary.AddMissingKeys(Simulation["Quest"], from.Simulation["Quest"]);
                TranslationDictionary.AddMissingKeys(Simulation["Simulation"], from.Simulation["Simulation"]);
                TranslationDictionary.AddMissingKeys(Simulation["Flags"], from.Simulation["Flags"]);
                TranslationDictionary.AddMissingKeys(Simulation["Quests"], from.Simulation["Quests"]);
                TranslationDictionary.AddMissingKeys(Simulation["Inventory"], from.Simulation["Inventory"]);
            }
            catch { }
        }
    }
}
