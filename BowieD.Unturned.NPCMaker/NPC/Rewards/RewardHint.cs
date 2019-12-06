using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardHint : Reward
    {
        public override RewardType Type => RewardType.Hint;
        public string Text { get; set; }
        public float Duration { get; set; }
        public override string UIText
        {
            get
            {
                return $"{LocalizationManager.Current.Reward["Type_Hint"]}: {Text} ({Duration} s.)";
            }
        }
    }
}
