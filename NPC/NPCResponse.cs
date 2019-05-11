using System.Linq;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCResponse
    {
        public NPCResponse()
        {
            mainText = "";
            conditions = new Condition[0];
            rewards = new Reward[0];
            visibleIn = new int[0];
        }

        public string mainText;
        public ushort openDialogueId;
        public ushort openVendorId;
        public ushort openQuestId;
        public Condition[] conditions;
        public Reward[] rewards;
        public int[] visibleIn;
        [XmlIgnore]
        public bool VisibleInAll => visibleIn == null || visibleIn.All(d => d == 1);
    }
}
