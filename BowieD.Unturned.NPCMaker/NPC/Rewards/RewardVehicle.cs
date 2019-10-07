using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardVehicle : Reward
    {
        public override RewardType Type => RewardType.Vehicle;
        public override string GameName
        {
            get
            {
                return $"{LocalizationManager.Current.Reward["Type_Vehicle"]} [{ID}] -> [{Spawnpoint}]";
            }
        }
        public UInt16 ID;
        public string Spawnpoint;
    }
}
