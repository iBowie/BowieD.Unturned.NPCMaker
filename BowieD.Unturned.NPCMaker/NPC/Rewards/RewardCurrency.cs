using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardCurrency : Reward
    {
        public override RewardType Type => RewardType.Currency;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Currency"]} x{Value}";
        public string GUID { get; set; }
        public uint Value { get; set; }
    }
}
