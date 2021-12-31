using BowieD.Unturned.NPCMaker;
using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.FindReplace;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Filtering;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Managers;
using BowieD.Unturned.NPCMaker.NPC;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class CharacterTabViewModel : BaseViewModel, ITabEditor, INPCTab
    {
        private NPCCharacter _character;
        public CharacterTabViewModel()
        {
            MainWindow.Instance.characterTabSelect.SelectionChanged += CharacterTabSelect_SelectionChanged;
            MainWindow.Instance.characterTabButtonAdd.Click += CharacterTabButtonAdd_Click;
            NPCCharacter empty = new NPCCharacter();
            Character = empty;
            UpdateColorPicker();
            UpdateTabs();

            ContextMenu cmenu3 = new ContextMenu();

            cmenu3.Items.Add(ContextHelper.CreateAddFromTemplateButton(typeof(NPCCharacter), (result) =>
            {
                if (result is NPCCharacter npcc)
                {
                    MainWindow.CurrentProject.data.characters.Add(npcc);
                    MetroTabItem tabItem = CreateTab(npcc);
                    MainWindow.Instance.characterTabSelect.Items.Add(tabItem);
                    MainWindow.Instance.characterTabSelect.SelectedIndex = MainWindow.Instance.characterTabSelect.Items.Count - 1;
                }
            }));

            MainWindow.Instance.characterTabButtonAdd.ContextMenu = cmenu3;

            MainWindow.Instance.txtDisplayName.ContextMenu = ContextHelper.CreateContextMenu(ContextHelper.EContextOption.Group_Rich | ContextHelper.EContextOption.Group_TextEdit);

            #region Clothing Init
            #region Default
            ContextMenu cidDefault_hat = new ContextMenu();
            cidDefault_hat.Items.Add(ContextHelper.CreateSelectHatButton((asset) =>
            {
                this.DefaultClothing.Hat = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);

                MainWindow.Instance.controlClothingDefaultHat.Value = this.DefaultClothing.Hat;
            }));
            MainWindow.Instance.controlClothingDefaultHat.ContextMenu = cidDefault_hat;
            MainWindow.Instance.controlClothingDefaultHat.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidDefault_glasses = new ContextMenu();
            cidDefault_glasses.Items.Add(ContextHelper.CreateSelectGlassesButton((asset) =>
            {
                this.DefaultClothing.Glasses = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingDefaultGlasses.Value = this.DefaultClothing.Glasses;
            }));
            MainWindow.Instance.controlClothingDefaultGlasses.ContextMenu = cidDefault_glasses;
            MainWindow.Instance.controlClothingDefaultGlasses.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidDefault_backpack = new ContextMenu();
            cidDefault_backpack.Items.Add(ContextHelper.CreateSelectBackpackButton((asset) =>
            {
                this.DefaultClothing.Backpack = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingDefaultBackpack.Value = this.DefaultClothing.Backpack;
            }));
            MainWindow.Instance.controlClothingDefaultBackpack.ContextMenu = cidDefault_backpack;
            MainWindow.Instance.controlClothingDefaultBackpack.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidDefault_shirt = new ContextMenu();
            cidDefault_shirt.Items.Add(ContextHelper.CreateSelectShirtButton((asset) =>
            {
                this.DefaultClothing.Shirt = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingDefaultShirt.Value = this.DefaultClothing.Shirt;
            }));
            MainWindow.Instance.controlClothingDefaultShirt.ContextMenu = cidDefault_shirt;
            MainWindow.Instance.controlClothingDefaultShirt.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidDefault_pants = new ContextMenu();
            cidDefault_pants.Items.Add(ContextHelper.CreateSelectPantsButton((asset) =>
            {
                this.DefaultClothing.Pants = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingDefaultPants.Value = this.DefaultClothing.Pants;
            }));
            MainWindow.Instance.controlClothingDefaultPants.ContextMenu = cidDefault_pants;
            MainWindow.Instance.controlClothingDefaultPants.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidDefault_mask = new ContextMenu();
            cidDefault_mask.Items.Add(ContextHelper.CreateSelectMaskButton((asset) =>
            {
                this.DefaultClothing.Mask = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingDefaultMask.Value = this.DefaultClothing.Mask;
            }));
            MainWindow.Instance.controlClothingDefaultMask.ContextMenu = cidDefault_mask;
            MainWindow.Instance.controlClothingDefaultMask.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidDefault_vest = new ContextMenu();
            cidDefault_vest.Items.Add(ContextHelper.CreateSelectVestButton((asset) =>
            {
                this.DefaultClothing.Vest = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingDefaultVest.Value = this.DefaultClothing.Vest;
            }));
            MainWindow.Instance.controlClothingDefaultVest.ContextMenu = cidDefault_vest;
            MainWindow.Instance.controlClothingDefaultVest.ValueChanged += (sender, e) => UpdateClothing();
            #endregion
            #region Halloween
            ContextMenu cidHalloween_hat = new ContextMenu();
            cidHalloween_hat.Items.Add(ContextHelper.CreateSelectHatButton((asset) =>
            {
                this.HalloweenClothing.Hat = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingHalloweenHat.Value = this.HalloweenClothing.Hat;
            }));
            MainWindow.Instance.controlClothingHalloweenHat.ContextMenu = cidHalloween_hat;
            MainWindow.Instance.controlClothingHalloweenHat.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidHalloween_glasses = new ContextMenu();
            cidHalloween_glasses.Items.Add(ContextHelper.CreateSelectGlassesButton((asset) =>
            {
                this.HalloweenClothing.Glasses = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingHalloweenGlasses.Value = this.HalloweenClothing.Glasses;
            }));
            MainWindow.Instance.controlClothingHalloweenGlasses.ContextMenu = cidHalloween_glasses;
            MainWindow.Instance.controlClothingHalloweenGlasses.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidHalloween_backpack = new ContextMenu();
            cidHalloween_backpack.Items.Add(ContextHelper.CreateSelectBackpackButton((asset) =>
            {
                this.HalloweenClothing.Backpack = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingHalloweenBackpack.Value = this.HalloweenClothing.Backpack;
            }));
            MainWindow.Instance.controlClothingHalloweenBackpack.ContextMenu = cidHalloween_backpack;
            MainWindow.Instance.controlClothingHalloweenBackpack.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidHalloween_shirt = new ContextMenu();
            cidHalloween_shirt.Items.Add(ContextHelper.CreateSelectShirtButton((asset) =>
            {
                this.HalloweenClothing.Shirt = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingHalloweenShirt.Value = this.HalloweenClothing.Shirt;
            }));
            MainWindow.Instance.controlClothingHalloweenShirt.ContextMenu = cidHalloween_shirt;
            MainWindow.Instance.controlClothingHalloweenShirt.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidHalloween_pants = new ContextMenu();
            cidHalloween_pants.Items.Add(ContextHelper.CreateSelectPantsButton((asset) =>
            {
                this.HalloweenClothing.Pants = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingHalloweenPants.Value = this.HalloweenClothing.Pants;
            }));
            MainWindow.Instance.controlClothingHalloweenPants.ContextMenu = cidHalloween_pants;
            MainWindow.Instance.controlClothingHalloweenPants.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidHalloween_mask = new ContextMenu();
            cidHalloween_mask.Items.Add(ContextHelper.CreateSelectMaskButton((asset) =>
            {
                this.HalloweenClothing.Mask = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingHalloweenMask.Value = this.HalloweenClothing.Mask;
            }));
            MainWindow.Instance.controlClothingHalloweenMask.ContextMenu = cidHalloween_mask;
            MainWindow.Instance.controlClothingHalloweenMask.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidHalloween_vest = new ContextMenu();
            cidHalloween_vest.Items.Add(ContextHelper.CreateSelectVestButton((asset) =>
            {
                this.HalloweenClothing.Vest = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingHalloweenVest.Value = this.HalloweenClothing.Vest;
            }));
            MainWindow.Instance.controlClothingHalloweenVest.ContextMenu = cidHalloween_vest;
            MainWindow.Instance.controlClothingHalloweenVest.ValueChanged += (sender, e) => UpdateClothing();
            #endregion
            #region Christmas
            ContextMenu cidChristmas_hat = new ContextMenu();
            cidChristmas_hat.Items.Add(ContextHelper.CreateSelectHatButton((asset) =>
            {
                this.ChristmasClothing.Hat = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingChristmasHat.Value = this.ChristmasClothing.Hat;
            }));
            MainWindow.Instance.controlClothingChristmasHat.ContextMenu = cidChristmas_hat;
            MainWindow.Instance.controlClothingChristmasHat.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidChristmas_glasses = new ContextMenu();
            cidChristmas_glasses.Items.Add(ContextHelper.CreateSelectGlassesButton((asset) =>
            {
                this.ChristmasClothing.Glasses = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingChristmasGlasses.Value = this.ChristmasClothing.Glasses;
            }));
            MainWindow.Instance.controlClothingChristmasGlasses.ContextMenu = cidChristmas_glasses;
            MainWindow.Instance.controlClothingChristmasGlasses.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidChristmas_backpack = new ContextMenu();
            cidChristmas_backpack.Items.Add(ContextHelper.CreateSelectBackpackButton((asset) =>
            {
                this.ChristmasClothing.Backpack = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingChristmasBackpack.Value = this.ChristmasClothing.Backpack;
            }));
            MainWindow.Instance.controlClothingChristmasBackpack.ContextMenu = cidChristmas_backpack;
            MainWindow.Instance.controlClothingChristmasBackpack.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidChristmas_shirt = new ContextMenu();
            cidChristmas_shirt.Items.Add(ContextHelper.CreateSelectShirtButton((asset) =>
            {
                this.ChristmasClothing.Shirt = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingChristmasShirt.Value = this.ChristmasClothing.Shirt;
            }));
            MainWindow.Instance.controlClothingChristmasShirt.ContextMenu = cidChristmas_shirt;
            MainWindow.Instance.controlClothingChristmasShirt.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidChristmas_pants = new ContextMenu();
            cidChristmas_pants.Items.Add(ContextHelper.CreateSelectPantsButton((asset) =>
            {
                this.ChristmasClothing.Pants = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingChristmasPants.Value = this.ChristmasClothing.Pants;
            }));
            MainWindow.Instance.controlClothingChristmasPants.ContextMenu = cidChristmas_pants;
            MainWindow.Instance.controlClothingChristmasPants.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidChristmas_mask = new ContextMenu();
            cidChristmas_mask.Items.Add(ContextHelper.CreateSelectMaskButton((asset) =>
            {
                this.ChristmasClothing.Mask = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingChristmasMask.Value = this.ChristmasClothing.Mask;
            }));
            MainWindow.Instance.controlClothingChristmasMask.ContextMenu = cidChristmas_mask;
            MainWindow.Instance.controlClothingChristmasMask.ValueChanged += (sender, e) => UpdateClothing();

            ContextMenu cidChristmas_vest = new ContextMenu();
            cidChristmas_vest.Items.Add(ContextHelper.CreateSelectVestButton((asset) =>
            {
                this.ChristmasClothing.Vest = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.controlClothingChristmasVest.Value = this.ChristmasClothing.Vest;
            }));
            MainWindow.Instance.controlClothingChristmasVest.ContextMenu = cidChristmas_vest;
            MainWindow.Instance.controlClothingChristmasVest.ValueChanged += (sender, e) => UpdateClothing();
            #endregion
            #endregion
            MainWindow.Instance.clothingTabControl.SelectionChanged += (sender, e) => UpdateClothing();
            #region Equipment Init
            ContextMenu cid_primary = new ContextMenu();
            cid_primary.Items.Add(ContextHelper.CreateSelectItemButton((asset) =>
            {
                EquipmentPrimary = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.primaryIdBox.Value = EquipmentPrimary;
            }, new AssetFilter_Equippable("Interface", "AssetPicker_Filter_Equippable", Equip_Type.Primary)));
            MainWindow.Instance.primaryIdBox.ContextMenu = cid_primary;
            MainWindow.Instance.primaryIdBox.ValueChanged += (sender, e) => UpdateEquipment();

            ContextMenu cid_secondary = new ContextMenu();
            cid_secondary.Items.Add(ContextHelper.CreateSelectItemButton((asset) =>
            {
                EquipmentSecondary = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.secondaryIdBox.Value = EquipmentSecondary;
            }, new AssetFilter_Equippable("Interface", "AssetPicker_Filter_Equippable", Equip_Type.Secondary)));
            MainWindow.Instance.secondaryIdBox.ContextMenu = cid_secondary;
            MainWindow.Instance.secondaryIdBox.ValueChanged += (sender, e) => UpdateEquipment();

            ContextMenu cid_tertiary = new ContextMenu();
            cid_tertiary.Items.Add(ContextHelper.CreateSelectItemButton((asset) =>
            {
                EquipmentTertiary = AppConfig.Instance.preferLegacyIDsOverGUIDs ? new GUIDIDBridge(asset.id) : new GUIDIDBridge(asset.guid);
                MainWindow.Instance.tertiaryIdBox.Value = EquipmentTertiary;
            }, new AssetFilter_Equippable("Interface", "AssetPicker_Filter_Equippable", Equip_Type.Tertiary)));
            MainWindow.Instance.tertiaryIdBox.ContextMenu = cid_tertiary;
            MainWindow.Instance.tertiaryIdBox.ValueChanged += (sender, e) => UpdateEquipment();
            #endregion

            var txtIDContext = new ContextMenu();

            txtIDContext.Items.Add(ContextHelper.CreateFindUnusedIDButton((id) =>
            {
                this.ID = id;
                MainWindow.Instance.txtID.Value = id;
            }, GameIntegration.EGameAssetCategory.OBJECT));
            txtIDContext.Items.Add(ContextHelper.CreateFindReplaceButton(FindReplaceFormats.CHARACTER_ID));

            MainWindow.Instance.txtID.ContextMenu = txtIDContext;

            var dialogueIDContext = new ContextMenu();

            dialogueIDContext.Items.Add(ContextHelper.CreateSelectAssetButton(typeof(GameDialogueAsset), (asset) =>
            {
                this.DialogueID = asset.ID;
                MainWindow.Instance.txtStartDialogueID.Value = asset.ID;
            }, "Control_SelectAsset_Dialogue", MahApps.Metro.IconPacks.PackIconMaterialKind.Chat));
            dialogueIDContext.Items.Add(ContextHelper.CreateFindReplaceButton(FindReplaceFormats.DIALOGUE_ID));

            MainWindow.Instance.txtStartDialogueID.ContextMenu = dialogueIDContext;
        }

        private void CharacterTabButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NPCCharacter item = new NPCCharacter();
            MainWindow.CurrentProject.data.characters.Add(item);
            MetroTabItem tabItem = CreateTab(item);
            MainWindow.Instance.characterTabSelect.Items.Add(tabItem);
            MainWindow.Instance.characterTabSelect.SelectedIndex = MainWindow.Instance.characterTabSelect.Items.Count - 1;
        }

        private void CharacterTabSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = MainWindow.Instance.characterTabSelect;
            if (tab.SelectedItem != null && tab.SelectedItem is TabItem tabItem && tabItem.DataContext != null)
            {
                if (tabItem.DataContext is NPCCharacter selectedTabChar)
                    Character = selectedTabChar;
            }

            if (tab.SelectedItem is null)
            {
                MainWindow.Instance.characterTabGrid.IsEnabled = false;
                MainWindow.Instance.characterTabGrid.Visibility = Visibility.Collapsed;
                MainWindow.Instance.characterTabGridNoSelection.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.characterTabGrid.IsEnabled = true;
                MainWindow.Instance.characterTabGrid.Visibility = Visibility.Visible;
                MainWindow.Instance.characterTabGridNoSelection.Visibility = Visibility.Collapsed;
            }
        }

        public void Save() { }
        public void Reset() { }

        public void UpdateTabs()
        {
            var tab = MainWindow.Instance.characterTabSelect;
            tab.Items.Clear();
            int selected = -1;
            for (int i = 0; i < MainWindow.CurrentProject.data.characters.Count; i++)
            {
                var character = MainWindow.CurrentProject.data.characters[i];
                if (character == _character)
                    selected = i;
                MetroTabItem tabItem = CreateTab(character);
                tab.Items.Add(tabItem);
            }
            if (selected != -1)
                tab.SelectedIndex = selected;

            if (tab.SelectedItem is null)
            {
                MainWindow.Instance.characterTabGrid.IsEnabled = false;
                MainWindow.Instance.characterTabGrid.Visibility = Visibility.Collapsed;
                MainWindow.Instance.characterTabGridNoSelection.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.characterTabGrid.IsEnabled = true;
                MainWindow.Instance.characterTabGrid.Visibility = Visibility.Visible;
                MainWindow.Instance.characterTabGridNoSelection.Visibility = Visibility.Collapsed;
            }
        }

        private MetroTabItem CreateTab(NPCCharacter character)
        {
            var binding = new Binding()
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("UIText")
            };
            Label l = new Label();
            l.SetBinding(Label.ContentProperty, binding);

            MetroTabItem tabItem = new MetroTabItem
            {
                CloseButtonEnabled = true,
                CloseTabCommand = CloseTabCommand,
                Header = l,
                DataContext = character
            };
            tabItem.CloseTabCommandParameter = tabItem;

            var cmenu = new ContextMenu();
            List<MenuItem> cmenuItems = new List<MenuItem>()
            {
                ContextHelper.CreateCopyButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    MetroTabItem target = context.PlacementTarget as MetroTabItem;
                    ClipboardManager.SetObject(Universal_ItemList.ReturnType.Character, target.DataContext);
                }),
                ContextHelper.CreateDuplicateButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    MetroTabItem target = context.PlacementTarget as MetroTabItem;
                    var cloned = (target.DataContext as NPCCharacter).Clone();

                    MainWindow.CurrentProject.data.characters.Add(cloned);
                    MetroTabItem ti = CreateTab(cloned);
                    MainWindow.Instance.characterTabSelect.Items.Add(ti);
                }),
                ContextHelper.CreatePasteButton((object sender, RoutedEventArgs e) =>
                {
                    if (ClipboardManager.TryGetObject(ClipboardManager.CharacterFormat, out var obj) && !(obj is null) && obj is NPCCharacter cloned)
                    {
                        MainWindow.CurrentProject.data.characters.Add(cloned);
                        MetroTabItem ti = CreateTab(cloned);
                        MainWindow.Instance.characterTabSelect.Items.Add(ti);
                    }
                })
            };

            foreach (var cmenuItem in cmenuItems)
                cmenu.Items.Add(cmenuItem);

            tabItem.ContextMenu = cmenu;

            return tabItem;
        }

        public NPCCharacter Character
        {
            get
            {
                Save();

                return _character;
            }

            set
            {
                Save();

                _character = value;

                HairColor = Character.hairColor;
                SkinColor = Character.skinColor;
                FaceID = Character.face;
                HairID = Character.haircut;
                BeardID = Character.beard;
                OnPropertyChange("");
            }
        }
        public string DisplayName { get => Character.DisplayName; set => Character.DisplayName = value; }
        public string EditorName { get => Character.EditorName; set => Character.EditorName = value; }
        public ushort ID { get => Character.ID; set => Character.ID = value; }
        public string GUID
        {
            get => Character.GUID;
            set
            {
                Character.GUID = value;
                OnPropertyChange("GUID");
            }
        }
        public string Comment
        {
            get => Character.Comment;
            set
            {
                Character.Comment = value;
                OnPropertyChange("Comment");
            }
        }
        public ushort DialogueID
        {
            get => Character.startDialogueId;
            set
            {
                Character.startDialogueId = value;
                OnPropertyChange("DialogueID");
            }
        }
        public byte FaceID
        {
            get => Character.face;

            set
            {
                Character.face = value;
                MainWindow.Instance.faceImageControl.Source = ("Resources/Unturned/Faces/" + value + ".png").GetImageSource();
                OnPropertyChange("FaceID");
            }
        }
        public byte HairID
        {
            get => Character.haircut;
            set
            {
                Character.haircut = value;
                foreach (UIElement ui in MainWindow.Instance.hairRenderGrid.Children)
                {
                    if (ui is Canvas c)
                    {
                        c.Visibility = Visibility.Collapsed;
                    }
                }
                MainWindow.Instance.hairRenderGrid.Children[value].Visibility = Visibility.Visible;
                OnPropertyChange("HairID");
            }
        }
        public byte BeardID
        {
            get => Character.beard;
            set
            {
                Character.beard = value;
                foreach (UIElement ui in MainWindow.Instance.beardRenderGrid.Children)
                {
                    if (ui is Canvas c)
                    {
                        c.Visibility = Visibility.Collapsed;
                    }
                }
                MainWindow.Instance.beardRenderGrid.Children[value].Visibility = Visibility.Visible;
                OnPropertyChange("BeardID");
            }
        }
        public Color? SkinColor
        {
            get => Character.skinColor;
            set
            {
                Character.skinColor = value ?? new Coloring.Color(0, 0, 0);
                MainWindow.Instance.faceImageBorder.Background = new SolidColorBrush(Character.skinColor);
                if (Coloring.ColorConverter.ColorToHSV(Character.skinColor).V <= 0.1d)
                {
                    DropShadowEffect effect = new DropShadowEffect
                    {
                        BlurRadius = 2,
                        Direction = 0,
                        Color = Brushes.White.Color,
                        ShadowDepth = 0
                    };
                    MainWindow.Instance.faceImageControl.Effect = effect;
                }
                else
                {
                    MainWindow.Instance.faceImageControl.Effect = null;
                }
                OnPropertyChange("SkinColor");
            }
        }
        public Color? HairColor
        {
            get => Character.hairColor;
            set
            {
                Character.hairColor = value ?? new Coloring.Color(0, 0, 0);
                SolidColorBrush color = new SolidColorBrush(Character.hairColor);
                MainWindow.Instance.beardRenderGrid.DataContext = color;
                MainWindow.Instance.hairRenderGrid.DataContext = color;
                OnPropertyChange("HairColor");
            }
        }
        public bool IsLeftHanded { get => Character.leftHanded; set => Character.leftHanded = value; }
        public NPCClothing DefaultClothing { get => Character.clothing; set => Character.clothing = value; }
        public NPCClothing ChristmasClothing { get => Character.christmasClothing; set => Character.christmasClothing = value; }
        public NPCClothing HalloweenClothing { get => Character.halloweenClothing; set => Character.halloweenClothing = value; }
        public GUIDIDBridge EquipmentPrimary { get => Character.equipPrimary; set => Character.equipPrimary = value; }
        public GUIDIDBridge EquipmentSecondary { get => Character.equipSecondary; set => Character.equipSecondary = value; }
        public GUIDIDBridge EquipmentTertiary { get => Character.equipTertiary; set => Character.equipTertiary = value; }
        public Equip_Type Equipped { get => Character.equipped; set => Character.equipped = value; }
        public ENPCHoliday HolidayRestriction { get => Character.holidayRestriction; set => Character.holidayRestriction = value; }

        private ICommand editVisibilityConditionsCommand;
        private ICommand randomFaceCommand;
        private ICommand randomHairCommand;
        private ICommand randomBeardCommand;
        private ICommand saveColorSkin;
        private ICommand saveColorHair;
        private ICommand regenerateGUIDsCommand;
        private ICommand poseEditorCommand;
        private ICommand closeTabCommand;
        private ICommand sortEditorNameA, sortEditorNameD, sortDisplayNameA, sortDisplayNameD, sortIDA, sortIDD;
        private ICommand randomGuidCommand;
        private ICommand setGuidCommand;

        public ICommand SortEditorNameAscending
        {
            get
            {
                if (sortEditorNameA == null)
                {
                    sortEditorNameA = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.characters = MainWindow.CurrentProject.data.characters.OrderBy(d => d.EditorName).ToList();
                        UpdateTabs();
                    });
                }
                return sortEditorNameA;
            }
        }
        public ICommand SortEditorNameDescending
        {
            get
            {
                if (sortEditorNameD == null)
                {
                    sortEditorNameD = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.characters = MainWindow.CurrentProject.data.characters.OrderByDescending(d => d.EditorName).ToList();
                        UpdateTabs();
                    });
                }
                return sortEditorNameD;
            }
        }
        public ICommand SortDisplayNameAscending
        {
            get
            {
                if (sortDisplayNameA == null)
                {
                    sortDisplayNameA = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.characters = MainWindow.CurrentProject.data.characters.OrderBy(d => d.DisplayName).ToList();
                        UpdateTabs();
                    });
                }
                return sortDisplayNameA;
            }
        }
        public ICommand SortDisplayNameDescending
        {
            get
            {
                if (sortDisplayNameD == null)
                {
                    sortDisplayNameD = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.characters = MainWindow.CurrentProject.data.characters.OrderByDescending(d => d.DisplayName).ToList();
                        UpdateTabs();
                    });
                }
                return sortDisplayNameD;
            }
        }
        public ICommand SortIDAscending
        {
            get
            {
                if (sortIDA == null)
                {
                    sortIDA = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.characters = MainWindow.CurrentProject.data.characters.OrderBy(d => d.ID).ToList();
                        UpdateTabs();
                    });
                }
                return sortIDA;
            }
        }
        public ICommand SortIDDescending
        {
            get
            {
                if (sortIDD == null)
                {
                    sortIDD = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.characters = MainWindow.CurrentProject.data.characters.OrderByDescending(d => d.ID).ToList();
                        UpdateTabs();
                    });
                }
                return sortIDD;
            }
        }
        public ICommand CloseTabCommand
        {
            get
            {
                if (closeTabCommand == null)
                {
                    closeTabCommand = new BaseCommand((sender) =>
                    {
                        var tab = (sender as MetroTabItem);
                        MainWindow.CurrentProject.data.characters.Remove(tab.DataContext as NPCCharacter);
                        MainWindow.Instance.characterTabSelect.Items.Remove(sender);
                    });
                }
                return closeTabCommand;
            }
        }
        public ICommand EditVisibilityConditionsCommand
        {
            get
            {
                if (editVisibilityConditionsCommand == null)
                {
                    editVisibilityConditionsCommand = new BaseCommand(() =>
                    {
                        Universal_ListView ulv = new Universal_ListView(Character.visibilityConditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, true)).ToLimitedList(byte.MaxValue), Universal_ItemList.ReturnType.Condition)
                        {
                            Owner = MainWindow.Instance
                        };
                        ulv.ShowDialog();
                        Character.visibilityConditions = new LimitedList<Condition>(ulv.Values.Cast<Condition>(), byte.MaxValue);
                        MainWindow.CurrentProject.isSaved = false;
                    });
                }
                return editVisibilityConditionsCommand;
            }
        }
        public ICommand RandomFaceCommand
        {
            get
            {
                if (randomFaceCommand == null)
                {
                    randomFaceCommand = new BaseCommand(() =>
                    {
                        FaceID = Random.NextByte(0, MainWindow.faceAmount);
                        OnPropertyChange("FaceID");
                    });
                }
                return randomFaceCommand;
            }
        }
        public ICommand RandomHairCommand
        {
            get
            {
                if (randomHairCommand == null)
                {
                    randomHairCommand = new BaseCommand(() =>
                    {
                        HairID = Random.NextByte(0, MainWindow.haircutAmount);
                        OnPropertyChange("HairID");
                    });
                }
                return randomHairCommand;
            }
        }
        public ICommand RandomBeardCommand
        {
            get
            {
                if (randomBeardCommand == null)
                {
                    randomBeardCommand = new BaseCommand(() =>
                    {
                        BeardID = Random.NextByte(0, MainWindow.beardAmount);
                        OnPropertyChange("BeardID");
                    });
                }
                return randomBeardCommand;
            }
        }
        public ICommand RandomGuidCommand
        {
            get
            {
                if (randomGuidCommand == null)
                {
                    randomGuidCommand = new BaseCommand(() =>
                    {
                        GUID = Guid.NewGuid().ToString("N");
                    });
                }
                return randomGuidCommand;
            }
        }
        public ICommand SetGuidCommand
        {
            get
            {
                if (setGuidCommand == null)
                {
                    setGuidCommand = new BaseCommand(() =>
                    {
                        MultiFieldInputView_Dialog mfiv = new MultiFieldInputView_Dialog(new string[1] { GUID });
                        if (mfiv.ShowDialog(new string[1] { LocalizationManager.Current.Character["Guid"] }, "") == true)
                        {
                            string res = mfiv.Values[0];
                            if (Guid.TryParse(res, out var newGuid))
                            {
                                GUID = newGuid.ToString("N");
                            }
                            else
                            {
                                MessageBox.Show(LocalizationManager.Current.Character["Guid_Invalid"]);
                            }
                        }
                    });
                }
                return setGuidCommand;
            }
        }
        private ICommand copyGuidCommand;
        public ICommand CopyGuidCommand
        {
            get
            {
                if (copyGuidCommand == null)
                {
                    copyGuidCommand = new BaseCommand(() =>
                    {
                        ClipboardManager.SetObject(Universal_ItemList.ReturnType.GenericString, GUID);
                    });
                }

                return copyGuidCommand;
            }
        }
        public ICommand SaveColorSkin
        {
            get
            {
                if (saveColorSkin == null)
                {
                    saveColorSkin = new BaseCommand(() =>
                    {
                        if (MainWindow.Instance.skinColorPicker.SelectedColor != null)
                        {
                            SaveColor(((Coloring.Color)MainWindow.Instance.skinColorPicker.SelectedColor.Value).ToHEX());
                        }
                    });
                }
                return saveColorSkin;
            }
        }
        public ICommand SaveColorHair
        {
            get
            {
                if (saveColorHair == null)
                {
                    saveColorHair = new BaseCommand(() =>
                    {
                        if (MainWindow.Instance.hairColorPicker.SelectedColor != null)
                        {
                            SaveColor(((Coloring.Color)MainWindow.Instance.hairColorPicker.SelectedColor.Value).ToHEX());
                        }
                    });
                }
                return saveColorHair;
            }
        }
        public ICommand RegenerateGUIDsCommand
        {
            get
            {
                if (regenerateGUIDsCommand == null)
                {
                    regenerateGUIDsCommand = new BaseCommand(() =>
                    {
                        if (MainWindow.CurrentProject.data.characters != null)
                        {
                            foreach (NPCCharacter c in MainWindow.CurrentProject.data.characters)
                            {
                                if (c != null)
                                {
                                    c.GUID = Guid.NewGuid().ToString("N");
                                }
                            }
                            MainWindow.Instance.MainWindowViewModel.CharacterTabViewModel.OnPropertyChange(nameof(CharacterTabViewModel.GUID));
                        }
                        if (MainWindow.CurrentProject.data.dialogues != null)
                        {
                            foreach (NPCDialogue d in MainWindow.CurrentProject.data.dialogues)
                            {
                                if (d != null)
                                {
                                    d.GUID = Guid.NewGuid().ToString("N");
                                }
                            }
                            MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.RelayChange(nameof(DialogueTabViewModel.GUID));
                        }
                        if (MainWindow.CurrentProject.data.vendors != null)
                        {
                            foreach (NPCVendor v in MainWindow.CurrentProject.data.vendors)
                            {
                                if (v != null)
                                {
                                    v.GUID = Guid.NewGuid().ToString("N");
                                }
                            }
                            MainWindow.Instance.MainWindowViewModel.VendorTabViewModel.RelayChange(nameof(VendorTabViewModel.GUID));
                        }
                        if (MainWindow.CurrentProject.data.dialogueVendors != null)
                        {
                            foreach (VirtualDialogueVendor v in MainWindow.CurrentProject.data.dialogueVendors)
                            {
                                if (v != null)
                                {
                                    v.GUID = Guid.NewGuid().ToString("N");
                                }
                            }
                            MainWindow.Instance.MainWindowViewModel.DialogueVendorTabViewModel.RelayChange(nameof(VirtualDialogueVendorTabViewModel.GUID));
                        }
                        if (MainWindow.CurrentProject.data.quests != null)
                        {
                            foreach (NPCQuest q in MainWindow.CurrentProject.data.quests)
                            {
                                if (q != null)
                                {
                                    q.GUID = Guid.NewGuid().ToString("N");
                                }
                            }
                            MainWindow.Instance.MainWindowViewModel.QuestTabViewModel.RelayChange(nameof(QuestTabViewModel.GUID));
                        }
                        MainWindow.CurrentProject.data.guid = Guid.NewGuid().ToString("N");
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["App_GUID_Regenerated"]);
                    });
                }
                return regenerateGUIDsCommand;
            }
        }
        public ICommand PoseEditorCommand
        {
            get
            {
                if (poseEditorCommand == null)
                {
                    poseEditorCommand = new BaseCommand(() =>
                    {
                        Character_PoseEditor cpe = new Character_PoseEditor(Character)
                        {
                            Owner = MainWindow.Instance
                        };
                        cpe.ShowDialog();
                    });
                }
                return poseEditorCommand;
            }
        }
        internal void SaveColor(string hex)
        {
            if (DataManager.UserColorsData.data.Contains(hex))
            {
                return;
            }

            DataManager.UserColorsData.data = DataManager.UserColorsData.data.Prepend(hex).ToArray();
            DataManager.UserColorsData.Save();
            Coloring.Color color = new Coloring.Color(hex);
            Xceed.Wpf.Toolkit.ColorItem colorItem = new Xceed.Wpf.Toolkit.ColorItem(color, hex);
            MainWindow.Instance.skinColorPicker.AvailableColors.Insert(0, colorItem);
        }
        internal void UpdateColorPicker()
        {
            MainWindow.Instance.skinColorPicker.TabBackground = Brushes.Transparent;
            MainWindow.Instance.hairColorPicker.TabBackground = Brushes.Transparent;
            MainWindow.Instance.skinColorPicker.AvailableColors.Clear();
            MainWindow.Instance.hairColorPicker.AvailableColors.Clear();
            MainWindow.Instance.skinColorPicker.StandardColors.Clear();
            MainWindow.Instance.hairColorPicker.StandardColors.Clear();
            foreach (string k in DataManager.UserColorsData.data)
            {
                MainWindow.Instance.skinColorPicker.AvailableColors.Add(new Xceed.Wpf.Toolkit.ColorItem(new Coloring.Color(k), k));
            }
            foreach (Coloring.Color k in GetUnturnedColors())
            {
                MainWindow.Instance.skinColorPicker.StandardColors.Add(new Xceed.Wpf.Toolkit.ColorItem(k, k.ToHEX()));
            }
        }
        private IEnumerable<Coloring.Color> GetUnturnedColors()
        {
            yield return new Coloring.Color("#F4E6D2");
            yield return new Coloring.Color("#D9CAB4");
            yield return new Coloring.Color("#BEA582");
            yield return new Coloring.Color("#9D886B");
            yield return new Coloring.Color("#94764B");
            yield return new Coloring.Color("#706049");
            yield return new Coloring.Color("#534736");
            yield return new Coloring.Color("#4B3D31");
            yield return new Coloring.Color("#332C25");
            yield return new Coloring.Color("#231F1C");
            yield return new Coloring.Color("#D7D7D7");
            yield return new Coloring.Color("#C1C1C1");
            yield return new Coloring.Color("#CDC08C");
            yield return new Coloring.Color("#AC6A39");
            yield return new Coloring.Color("#665037");
            yield return new Coloring.Color("#57452F");
            yield return new Coloring.Color("#352C22");
            yield return new Coloring.Color("#373737");
            yield return new Coloring.Color("#191919");
        }

        private void UpdateEquipment()
        {
            updateItemIcon<GameItemAsset>(EquipmentPrimary, MainWindow.Instance.controlEquipmentPrimaryIcon);
            updateItemIcon<GameItemAsset>(EquipmentSecondary, MainWindow.Instance.controlEquipmentSecondaryIcon);
            updateItemIcon<GameItemAsset>(EquipmentTertiary, MainWindow.Instance.controlEquipmentTertiaryIcon);
        }

        private void UpdateClothing()
        {
            NPCClothing clothing;

            switch (MainWindow.Instance.clothingTabControl.SelectedIndex)
            {
                case 0:
                    clothing = DefaultClothing;
                    {
                        updateItemIcon<GameItemHatAsset>(clothing.Hat, MainWindow.Instance.controlClothingDefaultHatIcon);
                        updateItemIcon<GameItemMaskAsset>(clothing.Mask, MainWindow.Instance.controlClothingDefaultMaskIcon);
                        updateItemIcon<GameItemShirtAsset>(clothing.Shirt, MainWindow.Instance.controlClothingDefaultShirtIcon);
                        updateItemIcon<GameItemGlassesAsset>(clothing.Glasses, MainWindow.Instance.controlClothingDefaultGlassesIcon);
                        updateItemIcon<GameItemVestAsset>(clothing.Vest, MainWindow.Instance.controlClothingDefaultVestIcon);
                        updateItemIcon<GameItemPantsAsset>(clothing.Pants, MainWindow.Instance.controlClothingDefaultPantsIcon);
                        updateItemIcon<GameItemBackpackAsset>(clothing.Backpack, MainWindow.Instance.controlClothingDefaultBackpackIcon);
                    }
                    break;
                case 1:
                    clothing = ChristmasClothing;
                    {
                        updateItemIcon<GameItemHatAsset>(clothing.Hat, MainWindow.Instance.controlClothingChristmasHatIcon);
                        updateItemIcon<GameItemMaskAsset>(clothing.Mask, MainWindow.Instance.controlClothingChristmasMaskIcon);
                        updateItemIcon<GameItemShirtAsset>(clothing.Shirt, MainWindow.Instance.controlClothingChristmasShirtIcon);
                        updateItemIcon<GameItemGlassesAsset>(clothing.Glasses, MainWindow.Instance.controlClothingChristmasGlassesIcon);
                        updateItemIcon<GameItemVestAsset>(clothing.Vest, MainWindow.Instance.controlClothingChristmasVestIcon);
                        updateItemIcon<GameItemPantsAsset>(clothing.Pants, MainWindow.Instance.controlClothingChristmasPantsIcon);
                        updateItemIcon<GameItemBackpackAsset>(clothing.Backpack, MainWindow.Instance.controlClothingChristmasBackpackIcon);
                    }
                    break;
                case 2:
                    clothing = HalloweenClothing;
                    {
                        updateItemIcon<GameItemHatAsset>(clothing.Hat, MainWindow.Instance.controlClothingHalloweenHatIcon);
                        updateItemIcon<GameItemMaskAsset>(clothing.Mask, MainWindow.Instance.controlClothingHalloweenMaskIcon);
                        updateItemIcon<GameItemShirtAsset>(clothing.Shirt, MainWindow.Instance.controlClothingHalloweenShirtIcon);
                        updateItemIcon<GameItemGlassesAsset>(clothing.Glasses, MainWindow.Instance.controlClothingHalloweenGlassesIcon);
                        updateItemIcon<GameItemVestAsset>(clothing.Vest, MainWindow.Instance.controlClothingHalloweenVestIcon);
                        updateItemIcon<GameItemPantsAsset>(clothing.Pants, MainWindow.Instance.controlClothingHalloweenPantsIcon);
                        updateItemIcon<GameItemBackpackAsset>(clothing.Backpack, MainWindow.Instance.controlClothingHalloweenBackpackIcon);
                    }
                    break;
                default:
                    return;
            }

            if (clothing.IsHairVisible)
            {
                MainWindow.Instance.hairInvisibleIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindow.Instance.hairInvisibleIcon.Visibility = Visibility.Visible;
            }

            if (clothing.IsBeardVisible)
            {
                MainWindow.Instance.beardInvisibleIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindow.Instance.beardInvisibleIcon.Visibility = Visibility.Visible;
            }
        }

        private static void updateItemIcon<T>(ushort id, Image control) where T : GameItemAsset
        {
            if (id > 0 && GameAssetManager.TryGetAsset<T>(id, out var asset))
            {
                control.Visibility = Visibility.Visible;
                control.Source = ThumbnailManager.CreateThumbnail(asset.ImagePath);
            }
            else
            {
                control.Visibility = Visibility.Collapsed;
            }
        }

        private static void updateItemIcon<T>(GUIDIDBridge bridge, Image control) where T : GameItemAsset
        {
            if (!bridge.IsEmpty && GameAssetManager.TryGetAsset<T>(bridge, out var asset))
            {
                control.Visibility = Visibility.Visible;
                control.Source = ThumbnailManager.CreateThumbnail(asset.ImagePath);
            }
            else
            {
                control.Visibility = Visibility.Collapsed;
            }
        }
    }
}
