using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardFlagBool : Reward
    {
        public override RewardType Type => RewardType.Flag_Bool;
        public override string UIText
        {
            get
            {
                return $"{LocalizationManager.Current.Reward["Type_Flag_Bool"]} [{ID}] -> {Value}";
            }
        }
        public UInt16 ID { get; set; }
        public bool Value { get; set; }
    }
}
