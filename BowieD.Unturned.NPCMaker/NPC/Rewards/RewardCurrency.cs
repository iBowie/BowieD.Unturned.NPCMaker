using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardCurrency : Reward
    {
        public override RewardType Type => RewardType.Currency;
        public override string UIText
        {
            get
            {
                return $"{LocalizationManager.Current.Reward["Type_Currency"]} x{Value}";
            }
        }
        public string GUID { get; set; }
        public UInt32 Value { get; set; }
    }
}
