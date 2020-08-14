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
            conditions = new List<Condition>();
            rewards = new List<Reward>();
            prev = 0;
        }

        public ushort prev;

        public List<string> pages;

        public List<Reward> rewards;

        public List<Condition> conditions;
    }
}
