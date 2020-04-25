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
    }
}
