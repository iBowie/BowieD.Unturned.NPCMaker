using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardTeleport : Reward
    {
        public override RewardType Type => RewardType.Teleport;
        public override string DisplayName
        {
            get
            {
                return $"{LocalizationManager.Current.Reward["Type_Teleport"]} [{Spawnpoint}]";
            }
        }

        public string Spawnpoint { get; set; }
    }
}
