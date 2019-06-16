using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardExperience : Reward
    {
        public override RewardType Type => RewardType.Experience;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardExperience")} x{Value}";
            }
        }

        public UInt32 Value;
    }
}
