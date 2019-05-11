using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardReputation : Reward
    {
        public override RewardType Type => RewardType.Reputation;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardReputation")} x{Value}";
            }
        }
        public Int32 Value;
    }
}
