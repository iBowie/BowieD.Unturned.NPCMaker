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
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardTeleport")} [{Spawnpoint}]";
            }
        }

        public string Spawnpoint;
    }
}
