using BowieD.Unturned.NPCMaker.Common;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCResponse : IAXData
    {
        public NPCResponse()
        {
            mainText = "";
            conditions = new List<Condition>();
            rewards = new List<Reward>();
            visibleIn = new int[0];
        }

        public string mainText;
        public ushort openDialogueId;
        public ushort openVendorId;
        public ushort openQuestId;
        public List<Condition> conditions;
        public List<Reward> rewards;
        public int[] visibleIn;
        [XmlIgnore]
        public bool VisibleInAll => visibleIn == null || visibleIn.All(d => d == 1) || visibleIn.All(d => d == 0); // last condition may cause invalid logic, but it works for now

        public void Load(XmlNode node, int version)
        {
            mainText = node["mainText"].InnerText;
            openDialogueId = node["openDialogueId"].ToUInt16();
            openVendorId = node["openVendorId"].ToUInt16();
            openQuestId = node["openQuestId"].ToUInt16();

            conditions = node["conditions"].ParseAXDataCollection<Condition>(version).ToList();
            rewards = node["rewards"].ParseAXDataCollection<Reward>(version).ToList();

            visibleIn = node["visibleIn"].ParseInt32Collection().ToArray();
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("mainText", node).WriteString(mainText);
            document.CreateNodeC("openDialogueId", node).WriteUInt16(openDialogueId);
            document.CreateNodeC("openVendorId", node).WriteUInt16(openVendorId);
            document.CreateNodeC("openQuestId", node).WriteUInt16(openQuestId);

            document.CreateNodeC("conditions", node).WriteAXDataCollection(document, "Condition", conditions);
            document.CreateNodeC("rewards", node).WriteAXDataCollection(document, "Reward", rewards);

            document.CreateNodeC("visibleIn", node).WriteInt32Collection(document, visibleIn);
        }
    }
}
