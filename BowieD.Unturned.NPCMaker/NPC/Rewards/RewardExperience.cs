using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardExperience : Reward
    {
        public override RewardType Type => RewardType.Experience;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Experience"]} x{Value}";

        public uint Value { get; set; }
    }
}
