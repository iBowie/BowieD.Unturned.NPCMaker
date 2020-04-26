using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
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
                text = "{0} Reputation";
            }
            return string.Format(text, Value > 0 ? $"+{Value}" : $"{Value}");
        }
    }
}
