using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardTeleport : Reward
    {
        public override RewardType Type => RewardType.Teleport;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Teleport"]} [{Spawnpoint}]";

        public string Spawnpoint { get; set; }

        public override void Give(Simulation simulation) { }
    }
}
