using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System.Xml;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCMessage : IAXData, IHasUIText
    {
        public NPCMessage()
        {
            pages = new LimitedList<string>(byte.MaxValue);
            conditions = new LimitedList<Condition>(byte.MaxValue);
            rewards = new LimitedList<Reward>(byte.MaxValue);
            prev = 0;
        }

        public ushort prev;

        public LimitedList<string> pages;

        public LimitedList<Reward> rewards;

        public LimitedList<Condition> conditions;

        public string UIText
        {
            get
            {
                var pagesContent = string.Join("|", pages);

                return TextUtil.Shortify(pagesContent);
            }
        }

        public void Load(XmlNode node, int version)
        {
            prev = node["prev"].ToUInt16();
            pages = new LimitedList<string>(node["pages"].ParseStringCollection(), byte.MaxValue);
            rewards = new LimitedList<Reward>(node["rewards"].ParseAXDataCollection<Reward>(version), byte.MaxValue);
            conditions = new LimitedList<Condition>(node["conditions"].ParseAXDataCollection<Condition>(version), byte.MaxValue);
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
