using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardQuest : Reward
    {
        public override RewardType Type => RewardType.Quest;
        public override string GameName
        {
            get
            {
                return $"{LocalizationManager.Current.Reward["Type_Quest"]} [{ID}]";
            }
        }
        public UInt16 ID;
    }
}
