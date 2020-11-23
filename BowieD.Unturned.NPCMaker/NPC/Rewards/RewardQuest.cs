using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardQuest : Reward
    {
        public override RewardType Type => RewardType.Quest;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Quest"]} [{ID}]";
        [RewardAssetPicker(typeof(GameQuestAsset))]
        public ushort ID { get; set; }

        public override void Give(Simulation simulation)
        {
            simulation.Quests.Add(ID);
        }
    }
}
