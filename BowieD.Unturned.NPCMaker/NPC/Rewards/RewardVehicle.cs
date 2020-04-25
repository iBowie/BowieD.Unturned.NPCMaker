using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardVehicle : Reward
    {
        public override RewardType Type => RewardType.Vehicle;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Vehicle"]} [{ID}] -> [{Spawnpoint}]";
        public ushort ID { get; set; }
        public string Spawnpoint { get; set; }

        public override void Give(Simulation simulation) { }
    }
}
