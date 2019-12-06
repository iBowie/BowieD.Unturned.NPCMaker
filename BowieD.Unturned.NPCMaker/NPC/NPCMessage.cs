using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System.Collections.Generic;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCMessage
    {
        public NPCMessage()
        {
            pages = new List<string>();
            conditions = new Condition[0];
            rewards = new Reward[0];
        }

        public List<string> pages;
        [XmlIgnore]
        public int PagesAmount => pages == null ? 0 : pages.Count;

        public Reward[] rewards;

        public Condition[] conditions;
    }
}
