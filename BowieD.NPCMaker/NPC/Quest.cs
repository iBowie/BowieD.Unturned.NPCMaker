using System;
using System.Collections.Generic;

namespace BowieD.NPCMaker.NPC
{
    public sealed class Quest
    {
        public Quest()
        {
            guid = Guid.NewGuid().ToString("N");
            conditions = new List<Condition.Condition>();
            rewards = new List<Reward.Reward>();
            title = new Dictionary<ELanguage, string>();
            description = new Dictionary<ELanguage, string>();
        }
        public string guid;
        public ushort id;
        public List<Condition.Condition> conditions;
        public List<Reward.Reward> rewards;
        public Dictionary<ELanguage, string> title;
        public Dictionary<ELanguage, string> description;
    }
}
