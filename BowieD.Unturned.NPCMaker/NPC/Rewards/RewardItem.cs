using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml;

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
        [Priority(2)]
        public GUIDIDBridge ID { get; set; }
        [Range(byte.MinValue, byte.MaxValue)]
        [Priority(1)]
        public byte Amount { get; set; } = 1;
        [Optional(null)]
        [AssetPicker(typeof(GameItemSightAsset), "Control_SelectAsset_Sight", MahApps.Metro.IconPacks.PackIconMaterialKind.Crosshairs)]
        public ushort? Sight { get; set; }
        [Optional(null)]
        [AssetPicker(typeof(GameItemTacticalAsset), "Control_SelectAsset_Tactical", MahApps.Metro.IconPacks.PackIconMaterialKind.KnifeMilitary)]
        public ushort? Tactical { get; set; }
        [Optional(null)]
        [AssetPicker(typeof(GameItemGripAsset), "Control_SelectAsset_Grip", MahApps.Metro.IconPacks.PackIconMaterialKind.HandBackLeft)]
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
            var idControl = editor.GetAssociatedControl<GUIDIDControl>("ID");
            var sightControl = editor.GetAssociatedControl<Controls.OptionalUInt16ValueControl>("Sight");
            var barrelControl = editor.GetAssociatedControl<Controls.OptionalUInt16ValueControl>("Barrel");
            var magazineControl = editor.GetAssociatedControl<Controls.OptionalUInt16ValueControl>("Magazine");
            var gripControl = editor.GetAssociatedControl<Controls.OptionalUInt16ValueControl>("Grip");
            var tacticalControl = editor.GetAssociatedControl<Controls.OptionalUInt16ValueControl>("Tactical");
            var ammoControl = editor.GetAssociatedControl<Controls.OptionalByteValueControl>("Ammo");

            void check(GUIDIDBridge bridge)
            {
                if (!bridge.IsEmpty)
                {
                    if (GameAssetManager.TryGetAsset<GameItemGunAsset>(bridge, out var gunAsset))
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
                    else if (GameAssetManager.TryGetAsset<GameItemAsset>(bridge, out var itemAsset))
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
                check(e);
            };

            editor.Loaded += (sender, e) =>
            {
                check(idControl.Value);
            };
        }

        public override void Give(Simulation simulation)
        {
            simulation.Items.Add(new Simulation.Item()
            {
                Amount = (byte)(Ammo == 0 ? 1 : Ammo),
                ID = ID.ResolveLegacyID<GameItemAsset>(),
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
                itemName = ID.ExportValue;
            }

            return string.Format(text, Amount, itemName);
        }

        public bool UpdateIcon(out BitmapImage image)
        {
            if (!ID.IsEmpty && GameAssetManager.TryGetAsset<GameItemAsset>(ID, out var asset))
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

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            if (version >= 11)
            {
                ID = node["ID"].ToGuidIDBridge();
            }
            else
            {
                ID = (GUIDIDBridge)node["ID"].ToUInt16();
            }
            Amount = node["Amount"].ToByte();
            Sight = node["Sight"].ToNullableUInt16();
            Tactical = node["Tactical"].ToNullableUInt16();
            Grip = node["Grip"].ToNullableUInt16();
            Barrel = node["Barrel"].ToNullableUInt16();
            Magazine = node["Magazine"].ToNullableUInt16();
            Ammo = node["Ammo"].ToNullableByte();
            Auto_Equip = node["Auto_Equip"].ToBoolean();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("ID", node).WriteGuidIDBridge(ID);
            document.CreateNodeC("Amount", node).WriteByte(Amount);
            document.CreateNodeC("Sight", node).WriteNullableUInt16(Sight);
            document.CreateNodeC("Tactical", node).WriteNullableUInt16(Tactical);
            document.CreateNodeC("Grip", node).WriteNullableUInt16(Grip);
            document.CreateNodeC("Barrel", node).WriteNullableUInt16(Barrel);
            document.CreateNodeC("Magazine", node).WriteNullableUInt16(Magazine);
            document.CreateNodeC("Ammo", node).WriteNullableByte(Ammo);
            document.CreateNodeC("Auto_Equip", node).WriteBoolean(Auto_Equip);
        }
    }
}
