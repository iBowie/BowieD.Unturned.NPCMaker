using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCQuest : IHasUIText
    {
        public NPCQuest()
        {
            conditions = new List<Condition>();
            rewards = new List<Reward>();
            title = "";
            description = "";
            guid = Guid.NewGuid().ToString("N");
            Comment = "";
        }

        [XmlAttribute]
        public string guid;
        [XmlAttribute("comment")]
        public string Comment { get; set; }

        public ushort id;
        public List<Condition> conditions;
        public List<Reward> rewards;
        public string title;
        public string description;

        public string UIText => $"[{id}] {title}";
    }
}