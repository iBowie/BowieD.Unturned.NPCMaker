using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System.Collections.Generic;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCMessage
    {
        public NPCMessage()
        {
            pages = new List<string>();
            conditions = new Condition[0];
            rewards = new Reward[0];
            prev = 0;
        }

        public ushort prev;

        public List<string> pages;

        public Reward[] rewards;

        public Condition[] conditions;
    }
}
