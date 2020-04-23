using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardItemRandom : Reward
    {
        public override RewardType Type => RewardType.Item_Random;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Item_Random"]} [{ID}] x{Amount}";
        public ushort ID { get; set; }
        public byte Amount { get; set; }
    }
}
