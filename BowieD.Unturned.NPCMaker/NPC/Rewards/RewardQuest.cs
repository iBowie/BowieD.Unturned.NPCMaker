using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardQuest : Reward
    {
        public override RewardType Type => RewardType.Quest;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Quest"]} [{ID}]";
        public ushort ID { get; set; }

        public override void Give(Simulation simulation)
        {
            simulation.Quests.Add(ID);
        }
    }
}
