using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardCurrency : Reward
    {
        public override RewardType Type => RewardType.Currency;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Currency"]} x{Value}";
        public string GUID { get; set; }
        public uint Value { get; set; }

        public override void Give(Simulation simulation)
        {
            if (simulation.Currencies.ContainsKey(GUID))
            {
                simulation.Currencies[GUID] += Value;
            }
            else
            {
                simulation.Currencies.Add(GUID, Value);
            }
        }
        public override string FormatReward(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Reward_Currency");
            }
            return string.Format(text, Value);
        }
    }
}
