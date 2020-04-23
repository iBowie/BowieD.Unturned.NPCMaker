using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardFlagBool : Reward
    {
        public override RewardType Type => RewardType.Flag_Bool;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Flag_Bool"]} [{ID}] -> {Value}";
        public ushort ID { get; set; }
        public bool Value { get; set; }
    }
}
