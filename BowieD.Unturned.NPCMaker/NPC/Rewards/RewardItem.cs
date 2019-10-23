using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardItem : Reward
    {
        public override RewardType Type => RewardType.Item;
        public override string DisplayName
        {
            get
            {
                return $"{LocalizationManager.Current.Reward["Type_Item"]} {ID} x{Amount}";
            }
        }
        public UInt16 ID { get; set; }
        public byte Amount { get; set; }
        [RewardOptional(0, 0)]
        public UInt16 Sight { get; set; }
        [RewardOptional(0, 0)]
        public UInt16 Tactical { get; set; }
        [RewardOptional(0, 0)]
        public UInt16 Grip { get; set; }
        [RewardOptional(0, 0)]
        public UInt16 Barrel { get; set; }
        [RewardOptional(0, 0)]
        public UInt16 Magazine { get; set; }
        [RewardOptional(0, 0)]
        public byte Ammo { get; set; }
        public bool Auto_Equip { get; set; }
    }
}
