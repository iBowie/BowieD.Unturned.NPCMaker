using BowieD.Unturned.NPCMaker.Localization;

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
    }
}
