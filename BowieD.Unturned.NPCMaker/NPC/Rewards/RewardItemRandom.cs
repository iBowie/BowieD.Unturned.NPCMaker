using BowieD.Unturned.NPCMaker.Localization;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardItemRandom : Reward
    {
        public override RewardType Type => RewardType.Item_Random;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Item_Random"]} [{ID}] x{Amount}";
        public ushort ID { get; set; }
        public byte Amount { get; set; }

        public override void Give(Simulation simulation)
        {
            MessageBox.Show("This action requires app to load all in-game assets, which i don't want to.");
        }
        public override string FormatReward(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Reward_Item_Random");
            }
            return string.Format(text, Amount);
        }
    }
}
