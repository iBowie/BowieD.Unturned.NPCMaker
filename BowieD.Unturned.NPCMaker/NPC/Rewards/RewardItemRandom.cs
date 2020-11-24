using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardItemRandom : Reward
    {
        public override RewardType Type => RewardType.Item_Random;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Item_Random"]} [{ID}] x{Amount}";
        [RewardAssetPicker(typeof(GameSpawnAsset), "Control_SelectAsset_Spawn", MahApps.Metro.IconPacks.PackIconMaterialKind.Dice6)]
        public ushort ID { get; set; }
        public byte Amount { get; set; }

        public override void Give(Simulation simulation)
        {
            ushort item = resolve(ID);
            if (item > 0)
            {
                simulation.Items.Add(new Simulation.Item()
                {
                    ID = item,
                    Amount = 1,
                    Quality = 100
                });
            }
            // MessageBox.Show("This action requires app to load all in-game assets, which i don't want to.");
        }
        ushort resolve(ushort id)
        {
            if (GameAssetManager.TryGetAsset<GameSpawnAsset>(ID, out var asset))
            {
                asset.resolve(out id, out bool isSpawn);
                if (isSpawn)
                    id = resolve(id);
                return id;
            }
            else
            {
                return 0;
            }
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
