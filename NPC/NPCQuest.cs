using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCQuest
    {
        public NPCQuest()
        {
            conditions = new List<Condition>();
            rewards = new List<Reward>();
            title = "";
            description = "";
            guid = Guid.NewGuid().ToString("N");
            comment = "";
        }
        
        [XmlAttribute]
        public string guid;
        [XmlAttribute]
        public string comment;

        public ushort id;
        public List<NPC.Condition> conditions;
        public List<NPC.Reward> rewards;
        public string title;
        public string description;

        public override string ToString()
        {
            return $"[{id}] {title}";
        }
    }
}