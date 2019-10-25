using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardExperience : Reward
    {
        public override RewardType Type => RewardType.Experience;
        public override string UIText
        {
            get
            {
                return $"{LocalizationManager.Current.Reward["Type_Experience"]} x{Value}";
            }
        }

        public UInt32 Value { get; set; }
    }
}
