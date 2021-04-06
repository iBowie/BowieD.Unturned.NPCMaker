using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCMessage : IAXData
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

        public void Load(XmlNode node, int version)
        {
            prev = node["prev"].ToUInt16();
            pages = node["pages"].ParseStringCollection().ToList();
            rewards = node["rewards"].ParseAXDataCollection<Reward>(version).ToList();
            conditions = node["conditions"].ParseAXDataCollection<Condition>(version).ToList();
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("prev", node).WriteUInt16(prev);
            document.CreateNodeC("pages", node).WriteStringCollection(document, pages);
            document.CreateNodeC("rewards", node).WriteAXDataCollection(document, "Reward", rewards);
            document.CreateNodeC("conditions", node).WriteAXDataCollection(document, "Condition", conditions);
        }
    }
}
