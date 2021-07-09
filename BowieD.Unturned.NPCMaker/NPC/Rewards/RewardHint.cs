using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardHint : Reward
    {
        public override RewardType Type => RewardType.Hint;
        public float Duration { get; set; }
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Hint"]}: {Localization} ({Duration} s.)";

        public override void Give(Simulation simulation)
        {
            MessageBox.Show(Localization, $"Displays for {Duration:0.##} seconds");
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            Duration = node["Duration"].ToSingle();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("Duration", node).WriteSingle(Duration);
        }
    }
}
