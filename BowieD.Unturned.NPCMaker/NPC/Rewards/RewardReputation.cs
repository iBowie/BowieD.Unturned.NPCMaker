using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardReputation : Reward
    {
        public override RewardType Type => RewardType.Reputation;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Reputation"]} x{Value}";
        public int Value { get; set; }

        public override void Give(Simulation simulation)
        {
            simulation.Reputation += Value;
        }
        public override string FormatReward(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Reward_Reputation");
            }
            return string.Format(text, Value > 0 ? $"+{Value}" : $"{Value}");
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            Value = node["Value"].ToInt32();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("Value", node).WriteInt32(Value);
        }
    }
}
