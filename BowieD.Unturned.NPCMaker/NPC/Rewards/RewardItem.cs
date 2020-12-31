using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Text;
using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardItem : Reward, IUIL_Icon
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

        [AssetPicker(typeof(GameItemAsset), "Control_SelectAsset_Item", MahApps.Metro.IconPacks.PackIconMaterialKind.Archive)]
        public ushort ID { get; set; }
        [Range((byte)1, byte.MaxValue)]
        public byte Amount { get; set; }
        [Optional(null)]
        [AssetPicker(typeof(GameItemSightAsset), "Control_SelectAsset_Sight", MahApps.Metro.IconPacks.PackIconMaterialKind.Crosshairs)]
        public ushort? Sight { get; set; }
        [Optional(null)]
        [AssetPicker(typeof(GameItemTacticalAsset), "Control_SelectAsset_Tactical", MahApps.Metro.IconPacks.PackIconMaterialKind.KnifeMilitary)]
        public ushort? Tactical { get; set; }
        [Optional(null)]
        [AssetPicker(typeof(GameItemGripAsset), "Control_SelectAsset_Grip", MahApps.Metro.IconPacks.PackIconMaterialKind.Hand)]
        public ushort? Grip { get; set; }
        [Optional(null)]
        [AssetPicker(typeof(GameItemBarrelAsset), "Control_SelectAsset_Barrel", MahApps.Metro.IconPacks.PackIconMaterialKind.Pistol)]
        public ushort? Barrel { get; set; }
        [Optional(null)]
        [AssetPicker(typeof(GameItemMagazineAsset), "Control_SelectAsset_Magazine", MahApps.Metro.IconPacks.PackIconMaterialKind.Ammunition)]
        public ushort? Magazine { get; set; }
        [Optional(null)]
        public byte? Ammo { get; set; }
        public bool Auto_Equip { get; set; }

        public override void PostLoad(Universal_RewardEditor editor)
        {
            var idControl = editor.GetAssociatedControl<MahApps.Metro.Controls.NumericUpDown>("ID");
            var sightControl = editor.GetAssociatedControl<Controls.OptionalUInt16ValueControl>("Sight");
            var barrelControl = editor.GetAssociatedControl<Controls.OptionalUInt16ValueControl>("Barrel");
            var magazineControl = editor.GetAssociatedControl<Controls.OptionalUInt16ValueControl>("Magazine");
            var gripControl = editor.GetAssociatedControl<Controls.OptionalUInt16ValueControl>("Grip");
            var tacticalControl = editor.GetAssociatedControl<Controls.OptionalUInt16ValueControl>("Tactical");
            var ammoControl = editor.GetAssociatedControl<Controls.OptionalByteValueControl>("Ammo");

            void check(ushort ID)
            {
                if (ID > 0)
                {
                    if (GameAssetManager.TryGetAsset<GameItemGunAsset>(ID, out var gunAsset))
                    {
                        if (gunAsset.hasSight)
                            sightControl.Visibility = System.Windows.Visibility.Visible;
                        else
                        {
                            sightControl.Visibility = System.Windows.Visibility.Collapsed;
                            sightControl.checkbox.IsChecked = false;
                        }

                        if (gunAsset.hasBarrel)
                            barrelControl.Visibility = System.Windows.Visibility.Visible;
                        else
                        {
                            barrelControl.Visibility = System.Windows.Visibility.Collapsed;
                            barrelControl.checkbox.IsChecked = false;
                        }

                        magazineControl.Visibility = System.Windows.Visibility.Visible;

                        if (gunAsset.hasGrip)
                            gripControl.Visibility = System.Windows.Visibility.Visible;
                        else
                        {
                            gripControl.Visibility = System.Windows.Visibility.Collapsed;
                            gripControl.checkbox.IsChecked = false;
                        }

                        if (gunAsset.hasTactical)
                            tacticalControl.Visibility = System.Windows.Visibility.Visible;
                        else
                        {
                            tacticalControl.Visibility = System.Windows.Visibility.Collapsed;
                            tacticalControl.checkbox.IsChecked = false;
                        }

                        ammoControl.Visibility = System.Windows.Visibility.Visible;
                    }
                    else if (GameAssetManager.TryGetAsset<GameItemAsset>(ID, out var itemAsset))
                    {
                        sightControl.Visibility = System.Windows.Visibility.Collapsed;
                        barrelControl.Visibility = System.Windows.Visibility.Collapsed;
                        magazineControl.Visibility = System.Windows.Visibility.Collapsed;
                        gripControl.Visibility = System.Windows.Visibility.Collapsed;
                        tacticalControl.Visibility = System.Windows.Visibility.Collapsed;
                        ammoControl.Visibility = System.Windows.Visibility.Collapsed;

                        sightControl.checkbox.IsChecked = false;
                        barrelControl.checkbox.IsChecked = false;
                        magazineControl.checkbox.IsChecked = false;
                        gripControl.checkbox.IsChecked = false;
                        tacticalControl.checkbox.IsChecked = false;
                        ammoControl.checkbox.IsChecked = false;
                    }
                    else
                    {
                        sightControl.Visibility = System.Windows.Visibility.Visible;
                        barrelControl.Visibility = System.Windows.Visibility.Visible;
                        magazineControl.Visibility = System.Windows.Visibility.Visible;
                        gripControl.Visibility = System.Windows.Visibility.Visible;
                        tacticalControl.Visibility = System.Windows.Visibility.Visible;
                        ammoControl.Visibility = System.Windows.Visibility.Visible;
                    }
                }
                else
                {
                    sightControl.Visibility = System.Windows.Visibility.Visible;
                    barrelControl.Visibility = System.Windows.Visibility.Visible;
                    magazineControl.Visibility = System.Windows.Visibility.Visible;
                    gripControl.Visibility = System.Windows.Visibility.Visible;
                    tacticalControl.Visibility = System.Windows.Visibility.Visible;
                    ammoControl.Visibility = System.Windows.Visibility.Visible;
                }
            }

            idControl.ValueChanged += (sender, e) =>
            {
                if (e.NewValue.HasValue)
                {
                    check((ushort)e.NewValue);
                }
                else
                {
                    check(0);
                }
            };

            editor.Loaded += (sender, e) =>
            {
                if (idControl.Value.HasValue)
                {
                    check((ushort)idControl.Value);
                }
                else
                {
                    check(0);
                }
            };
        }

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

        public bool UpdateIcon(out BitmapImage image)
        {
            if (ID > 0 && GameAssetManager.TryGetAsset<GameItemAsset>(ID, out var asset))
            {
                image = ThumbnailManager.CreateThumbnail(asset.ImagePath);
                return true;
            }
            else
            {
                image = default;
                return false;
            }
        }
    }
}
