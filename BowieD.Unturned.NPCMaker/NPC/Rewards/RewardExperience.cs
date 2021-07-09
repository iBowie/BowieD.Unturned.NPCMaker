using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardExperience : Reward
    {
        public override RewardType Type => RewardType.Experience;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Experience"]} x{Value}";

        public uint Value { get; set; }

        public override void Give(Simulation simulation)
        {
            simulation.Experience += Value;
        }
        public override string FormatReward(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Reward_Experience");
            }
            return string.Format(text, Value);
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            Value = node["Value"].ToUInt32();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("Value", node).WriteUInt32(Value);
        }
    }
}
