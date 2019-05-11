using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardVehicle : Reward
    {
        public override RewardType Type => RewardType.Vehicle;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardVehicle")} [{ID}] -> [{Spawnpoint}]";
            }
        }
        public UInt16 ID;
        public string Spawnpoint;
    }
}
