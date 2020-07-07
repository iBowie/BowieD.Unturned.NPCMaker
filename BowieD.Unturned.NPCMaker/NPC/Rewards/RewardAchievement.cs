using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardAchievement : Reward
    {
        public override RewardType Type => RewardType.Achievement;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Achievement"]} [{ID}]";
        public string ID { get; set; }

        public override void Give(Simulation simulation) { }
    }
}
