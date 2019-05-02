using System.Linq;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCResponse
    {
        public NPCResponse()
        {
            mainText = "";
            conditions = new NPC.Conditions.Condition[0];
            rewards = new Reward[0];
            visibleIn = new int[0];
        }

        public string mainText;
        public ushort openDialogueId;
        public ushort openVendorId;
        public ushort openQuestId;
        public NPC.Conditions.Condition[] conditions;
        public NPC.Reward[] rewards;
        public int[] visibleIn;
        [XmlIgnore]
        public bool VisibleInAll => visibleIn == null || visibleIn.All(d => d == 1);
    }
}
