using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardItem : Reward
    {
        public override RewardType Type => RewardType.Item;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(LocalizationManager.Current.Reward["Type_Item"]);

                if (GameAssetManager.TryGetAsset<GameItemAsset>(ID, out var asset))
                {
                    sb.Append($" {asset.name} x{Amount}");
                }
                else
                {
                    sb.Append($" {ID} x{Amount}");
                }

                return sb.ToString();
            }
        }

        public ushort ID { get; set; }
        public byte Amount { get; set; }
        [RewardOptional(null)]
        public ushort? Sight { get; set; }
        [RewardOptional(null)]
        public ushort? Tactical { get; set; }
        [RewardOptional(null)]
        public ushort? Grip { get; set; }
        [RewardOptional(null)]
        public ushort? Barrel { get; set; }
        [RewardOptional(null)]
        public ushort? Magazine { get; set; }
        [RewardOptional(null)]
        public byte? Ammo { get; set; }
        public bool Auto_Equip { get; set; }

        public override void Give(Simulation simulation)
        {
            simulation.Items.Add(new Simulation.Item()
            {
                Amount = (byte)(Ammo == 0 ? 1 : Ammo),
                ID = ID,
                Quality = 100
            });
        }
        public override string FormatReward(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Reward_Item");
            }

            string itemName;

            if (GameAssetManager.TryGetAsset<GameItemAsset>(ID, out var asset))
            {
                itemName = asset.name;
            }
            else
            {
                itemName = ID.ToString();
            }

            return string.Format(text, Amount, itemName);
        }
    }
}
