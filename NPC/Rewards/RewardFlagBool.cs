using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardFlagBool : Reward
    {
        public override RewardType Type => RewardType.Flag_Bool;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardFlagBool")} [{ID}] -> {Value}";
            }
        }
        public UInt16 ID;
        public bool Value;
    }
}
