using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardItemRandom : Reward
    {
        public override RewardType Type => RewardType.Item_Random;
        public override string UIText
        {
            get
            {
                return $"{LocalizationManager.Current.Reward["Type_Item_Random"]} [{ID}] x{Amount}";
            }
        }
        public UInt16 ID { get; set; }
        public byte Amount { get; set; }
    }
}
