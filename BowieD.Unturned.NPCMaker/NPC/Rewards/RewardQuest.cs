using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardQuest : Reward
    {
        public override RewardType Type => RewardType.Quest;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardQuest")} [{ID}]";
            }
        }
        public UInt16 ID;
    }
}
