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
        public override string FormatReward(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Reward_Vehicle");
            }
            return string.Format(text, ID);
        }
    }
}
