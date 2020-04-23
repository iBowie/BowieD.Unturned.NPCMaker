using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardItem : Reward
    {
        public override RewardType Type => RewardType.Item;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Item"]} {ID} x{Amount}";
        public ushort ID { get; set; }
        public byte Amount { get; set; }
        [RewardOptional(0, 0)]
        public ushort Sight { get; set; }
        [RewardOptional(0, 0)]
        public ushort Tactical { get; set; }
        [RewardOptional(0, 0)]
        public ushort Grip { get; set; }
        [RewardOptional(0, 0)]
        public ushort Barrel { get; set; }
        [RewardOptional(0, 0)]
        public ushort Magazine { get; set; }
        [RewardOptional(0, 0)]
        public byte Ammo { get; set; }
        public bool Auto_Equip { get; set; }
    }
}
