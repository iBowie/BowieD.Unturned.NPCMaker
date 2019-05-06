using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCQuest : IHasDisplayName, IHasComment
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
        [XmlAttribute]
        [XmlElement("comment")]
        public string Comment { get; set; }

        public ushort id;
        public List<NPC.Condition> conditions;
        public List<NPC.Reward> rewards;
        public string title;
        public string description;

        public string DisplayName => $"[{id}] {title}";
    }
}