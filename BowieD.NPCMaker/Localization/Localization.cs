using System.Collections.Generic;

namespace BowieD.NPCMaker.Localization
{
    public sealed class Localization
    {
        public Localization()
        {
            Author = "Unknown";
            LastUpdate = "Never";
            General = new Dictionary<string, string>();
            Options = new Dictionary<string, string>();
            Character = new Dictionary<string, string>();
            Dialogue = new Dictionary<string, string>();
            Vendor = new Dictionary<string, string>();
            Quest = new Dictionary<string, string>();
            Condition = new Dictionary<string, string>();
            Reward = new Dictionary<string, string>();
        }
        public string Author;
        public string LastUpdate;
        public Dictionary<string, string> General;
        public Dictionary<string, string> Options;
        public Dictionary<string, string> Character;
        public Dictionary<string, string> Dialogue;
        public Dictionary<string, string> Vendor;
        public Dictionary<string, string> Quest;
        public Dictionary<string, string> Condition;
        public Dictionary<string, string> Reward;
    }
}
