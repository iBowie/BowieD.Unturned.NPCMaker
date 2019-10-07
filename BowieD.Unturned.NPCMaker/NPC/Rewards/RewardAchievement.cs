using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardAchievement : Reward
    {
        public override RewardType Type => RewardType.Achievement;
        public override string GameName
        {
            get
            {
                return $"{LocalizationManager.Current.Reward["Type_Achievement"]} [{ID}]";
            }
        }
        public string ID;
    }
}
