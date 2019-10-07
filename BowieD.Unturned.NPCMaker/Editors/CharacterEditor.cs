using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Data;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.Editors
{
    public class CharacterEditor : IEditor<NPCCharacter>
    {
        private static bool isMenuInit = false;
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
        private readonly Func<Equip_Type> getEquipType = new Func<Equip_Type>(() =>
        {
            if (MainWindow.Instance.equipSlotBox.SelectedItem is ComboBoxItem item)
                return (Equip_Type)item.Tag;
            return Equip_Type.None;
        });
        private readonly Func<NPC_Pose> getPose = new Func<NPC_Pose>(() =>
        {
            if (MainWindow.Instance.apparelPoseBox.SelectedItem is ComboBoxItem item)
                return (NPC_Pose)item.Tag;
            return NPC_Pose.Stand;
        });
        public CharacterEditor()
        {
            MainWindow.Instance.characterResetButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Reset();
                SendPresence();
            });
            MainWindow.Instance.characterOpenButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Open();
                SendPresence();
            });
            MainWindow.Instance.characterSaveButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Save();
                SendPresence();
            });
            MainWindow.Instance.apparelFaceRandomize.Click += Apparel_Face_Random_Button_Click;
            MainWindow.Instance.faceImageIndex.ValueChanged += FaceImageIndex_Changed;
            MainWindow.Instance.beardImageIndex.ValueChanged += BeardImageIndex_Changed;
            MainWindow.Instance.hairImageIndex.ValueChanged += HairImageIndex_Changed;
            MainWindow.Instance.apparelHairRandomize.Click += Apparel_Hair_Random_Button_Click;
            MainWindow.Instance.apparelBeardRandomize.Click += Apparel_Beard_Random_Button_Click;
            MainWindow.Instance.apparelFaceRandomize.Click += Apparel_Face_Random_Button_Click;
            MainWindow.Instance.visibilityCondsButton.Click += Char_EditConditions_Button_Click;
            MainWindow.Instance.skinColorPicker.StandardColorsHeader = "Unturned";
            MainWindow.Instance.hairColorPicker.StandardColorsHeader = "Unturned";
            MainWindow.Instance.skinColorPicker.StandardColors = new System.Collections.ObjectModel.ObservableCollection<Xceed.Wpf.Toolkit.ColorItem>(GetUnturnedColors().Select(d => new Xceed.Wpf.Toolkit.ColorItem(d, d.ToHEX())));
            MainWindow.Instance.hairColorPicker.StandardColors = new System.Collections.ObjectModel.ObservableCollection<Xceed.Wpf.Toolkit.ColorItem>(GetUnturnedColors().Select(d => new Xceed.Wpf.Toolkit.ColorItem(d, d.ToHEX())));
            MainWindow.Instance.skinColorPicker.SelectedColorChanged += SkinColorPicker_SelectedColorChanged;
            MainWindow.Instance.hairColorPicker.SelectedColorChanged += HairColorPicker_SelectedColorChanged;
            MainWindow.Instance.skinColorPicker_SaveButton.Click += SkinColorPicker_SaveButton_Click;
            MainWindow.Instance.hairColorPicker_SaveButton.Click += HairColorPicker_SaveButton_Click;
            conditions = new List<Condition>();
            UserColors.Load(new string[0]);
            UpdateColorPickerFromBuffer();
            if (!isMenuInit)
            {
                MainWindow.Instance.txtDisplayName.ContextMenu = new ContextMenu();
                MainWindow.Instance.txtDisplayName.ContextMenu.Items.Add(ContextHelper.CreatePasteColorMenu());
                isMenuInit = true;
            }
        }
        public NPCCharacter Current
        {
            get
            {
                return new NPCCharacter()
                {
                    ID = (ushort)(MainWindow.Instance.txtID.Value ?? 0),
                    Beard = (byte)(MainWindow.Instance.beardImageIndex.Value ?? 0),
                    ChristmasClothing = new NPCClothing()
                    {
                        backpack = (ushort)(MainWindow.Instance.christmasbackpackIdBox.Value ?? 0),
                        bottom = (ushort)(MainWindow.Instance.christmasbottomIdBox.Value ?? 0),
                        glasses = (ushort)(MainWindow.Instance.christmasglassesIdBox.Value ?? 0),
                        hat = (ushort)(MainWindow.Instance.christmashatIdBox.Value ?? 0),
                        mask = (ushort)(MainWindow.Instance.christmasmaskIdBox.Value ?? 0),
                        top = (ushort)(MainWindow.Instance.christmastopIdBox.Value ?? 0),
                        vest = (ushort)(MainWindow.Instance.christmasvestIdBox.Value ?? 0)
                    },
                    HalloweenClothing = new NPCClothing()
                    {
                        backpack = (ushort)(MainWindow.Instance.halloweenbackpackIdBox.Value ?? 0),
                        bottom = (ushort)(MainWindow.Instance.halloweenbottomIdBox.Value ?? 0),
                        glasses = (ushort)(MainWindow.Instance.halloweenglassesIdBox.Value ?? 0),
                        hat = (ushort)(MainWindow.Instance.halloweenhatIdBox.Value ?? 0),
                        mask = (ushort)(MainWindow.Instance.halloweenmaskIdBox.Value ?? 0),
                        top = (ushort)(MainWindow.Instance.halloweentopIdBox.Value ?? 0),
                        vest = (ushort)(MainWindow.Instance.halloweenvestIdBox.Value ?? 0)
                    },
                    Clothing = new NPCClothing()
                    {
                        backpack = (ushort)(MainWindow.Instance.backpackIdBox.Value ?? 0),
                        bottom = (ushort)(MainWindow.Instance.bottomIdBox.Value ?? 0),
                        glasses = (ushort)(MainWindow.Instance.glassesIdBox.Value ?? 0),
                        hat = (ushort)(MainWindow.Instance.hatIdBox.Value ?? 0),
                        mask = (ushort)(MainWindow.Instance.maskIdBox.Value ?? 0),
                        top = (ushort)(MainWindow.Instance.topIdBox.Value ?? 0),
                        vest = (ushort)(MainWindow.Instance.vestIdBox.Value ?? 0)
                    },
                    DisplayName = MainWindow.Instance.txtDisplayName.Text ?? "",
                    EditorName = MainWindow.Instance.txtEditorName.Text ?? "",
                    Equipped = getEquipType.Invoke(),
                    Pose = getPose.Invoke(),
                    EquipPrimary = (ushort)(MainWindow.Instance.primaryIdBox.Value ?? 0),
                    EquipSecondary = (ushort)(MainWindow.Instance.secondaryIdBox.Value ?? 0),
                    EquipTertiary = (ushort)(MainWindow.Instance.tertiaryIdBox.Value ?? 0),
                    FaceID = (byte)(MainWindow.Instance.faceImageIndex.Value ?? 0),
                    Haircut = (byte)(MainWindow.Instance.hairImageIndex.Value ?? 0),
                    guid = loadedGUID,
                    LeftHanded = MainWindow.Instance.apparelLeftHandedCheckbox.IsChecked == true,
                    DialogueID = (ushort)(MainWindow.Instance.txtStartDialogueID.Value ?? 0),
                    visibilityConditions = conditions,
                    SkinColor = (MainWindow.Instance.skinColorPicker.SelectedColor ?? new Coloring.Color(0, 0, 0)),
                    HairColor = (MainWindow.Instance.hairColorPicker.SelectedColor ?? new Coloring.Color(0, 0, 0))
                };
            }
            set
            {
                Reset();
                MainWindow.Instance.txtID.Value = value.ID;
                MainWindow.Instance.txtStartDialogueID.Value = value.DialogueID;
                MainWindow.Instance.txtDisplayName.Text = value.DisplayName;
                MainWindow.Instance.txtEditorName.Text = value.EditorName;
                MainWindow.Instance.skinColorPicker.SelectedColor = value.SkinColor;
                MainWindow.Instance.hairColorPicker.SelectedColor = value.HairColor;
                MainWindow.Instance.backpackIdBox.Value = value.Clothing.backpack;
                MainWindow.Instance.maskIdBox.Value = value.Clothing.mask;
                MainWindow.Instance.vestIdBox.Value = value.Clothing.vest;
                MainWindow.Instance.topIdBox.Value = value.Clothing.top;
                MainWindow.Instance.bottomIdBox.Value = value.Clothing.bottom;
                MainWindow.Instance.glassesIdBox.Value = value.Clothing.glasses;
                MainWindow.Instance.hatIdBox.Value = value.Clothing.hat;
                MainWindow.Instance.halloweenbackpackIdBox.Value = value.HalloweenClothing.backpack;
                MainWindow.Instance.halloweenmaskIdBox.Value = value.HalloweenClothing.mask;
                MainWindow.Instance.halloweenvestIdBox.Value = value.HalloweenClothing.vest;
                MainWindow.Instance.halloweentopIdBox.Value = value.HalloweenClothing.top;
                MainWindow.Instance.halloweenbottomIdBox.Value = value.HalloweenClothing.bottom;
                MainWindow.Instance.halloweenglassesIdBox.Value = value.HalloweenClothing.glasses;
                MainWindow.Instance.halloweenhatIdBox.Value = value.HalloweenClothing.hat;
                MainWindow.Instance.christmasbackpackIdBox.Value = value.ChristmasClothing.backpack;
                MainWindow.Instance.christmasmaskIdBox.Value = value.ChristmasClothing.mask;
                MainWindow.Instance.christmasvestIdBox.Value = value.ChristmasClothing.vest;
                MainWindow.Instance.christmastopIdBox.Value = value.ChristmasClothing.top;
                MainWindow.Instance.christmasbottomIdBox.Value = value.ChristmasClothing.bottom;
                MainWindow.Instance.christmasglassesIdBox.Value = value.ChristmasClothing.glasses;
                MainWindow.Instance.christmashatIdBox.Value = value.ChristmasClothing.hat;
                MainWindow.Instance.primaryIdBox.Value = value.EquipPrimary;
                MainWindow.Instance.secondaryIdBox.Value = value.EquipSecondary;
                MainWindow.Instance.tertiaryIdBox.Value = value.EquipTertiary;
                MainWindow.Instance.faceImageIndex.Value = value.FaceID;
                MainWindow.Instance.beardImageIndex.Value = value.Beard;
                MainWindow.Instance.hairImageIndex.Value = value.Haircut;
                MainWindow.Instance.apparelLeftHandedCheckbox.IsChecked = value.LeftHanded;
                for (int k = 0; k < MainWindow.Instance.apparelPoseBox.Items.Count; k++)
                {
                    if (((NPC_Pose)(MainWindow.Instance.apparelPoseBox.Items[k] as ComboBoxItem).Tag) == value.Pose)
                    {
                        MainWindow.Instance.apparelPoseBox.SelectedIndex = k;
                        break;
                    }
                }
                for (int k = 0; k < MainWindow.Instance.equipSlotBox.Items.Count; k++)
                {
                    if (((Equip_Type)(MainWindow.Instance.equipSlotBox.Items[k] as ComboBoxItem).Tag) == value.Equipped)
                    {
                        MainWindow.Instance.equipSlotBox.SelectedIndex = k;
                        break;
                    }
                }
                conditions = value.visibilityConditions;
                loadedGUID = value.guid;
            }
        }
        public void Open()
        {
            var ulv = new Universal_ListView(MainWindow.CurrentProject.data.characters.OrderBy(d => d.ID).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Character, false)).ToList(), Universal_ItemList.ReturnType.Character);
            if (ulv.ShowDialog() == true)
            {
                Save();
                Current = ulv.SelectedValue as NPCCharacter;
                App.Logger.LogInfo($"Opened character {MainWindow.Instance.txtID.Value}");
            }
            MainWindow.CurrentProject.data.characters = ulv.Values.Cast<NPCCharacter>().ToList();
        }
        public void Reset()
        {
            NPCCharacter value = new NPCCharacter();
            MainWindow.Instance.txtID.Value = value.ID;
            MainWindow.Instance.txtStartDialogueID.Value = value.DialogueID;
            MainWindow.Instance.txtDisplayName.Text = value.DisplayName;
            MainWindow.Instance.txtEditorName.Text = value.EditorName;
            MainWindow.Instance.backpackIdBox.Value = value.Clothing.backpack;
            MainWindow.Instance.maskIdBox.Value = value.Clothing.mask;
            MainWindow.Instance.vestIdBox.Value = value.Clothing.vest;
            MainWindow.Instance.topIdBox.Value = value.Clothing.top;
            MainWindow.Instance.bottomIdBox.Value = value.Clothing.bottom;
            MainWindow.Instance.glassesIdBox.Value = value.Clothing.glasses;
            MainWindow.Instance.hatIdBox.Value = value.Clothing.hat;
            MainWindow.Instance.halloweenbackpackIdBox.Value = value.HalloweenClothing.backpack;
            MainWindow.Instance.halloweenmaskIdBox.Value = value.HalloweenClothing.mask;
            MainWindow.Instance.halloweenvestIdBox.Value = value.HalloweenClothing.vest;
            MainWindow.Instance.halloweentopIdBox.Value = value.HalloweenClothing.top;
            MainWindow.Instance.halloweenbottomIdBox.Value = value.HalloweenClothing.bottom;
            MainWindow.Instance.halloweenglassesIdBox.Value = value.HalloweenClothing.glasses;
            MainWindow.Instance.halloweenhatIdBox.Value = value.HalloweenClothing.hat;
            MainWindow.Instance.christmasbackpackIdBox.Value = value.ChristmasClothing.backpack;
            MainWindow.Instance.christmasmaskIdBox.Value = value.ChristmasClothing.mask;
            MainWindow.Instance.christmasvestIdBox.Value = value.ChristmasClothing.vest;
            MainWindow.Instance.christmastopIdBox.Value = value.ChristmasClothing.top;
            MainWindow.Instance.christmasbottomIdBox.Value = value.ChristmasClothing.bottom;
            MainWindow.Instance.christmasglassesIdBox.Value = value.ChristmasClothing.glasses;
            MainWindow.Instance.christmashatIdBox.Value = value.ChristmasClothing.hat;
            MainWindow.Instance.primaryIdBox.Value = value.EquipPrimary;
            MainWindow.Instance.secondaryIdBox.Value = value.EquipSecondary;
            MainWindow.Instance.tertiaryIdBox.Value = value.EquipTertiary;
            MainWindow.Instance.faceImageIndex.Value = value.FaceID;
            MainWindow.Instance.beardImageIndex.Value = value.Beard;
            MainWindow.Instance.hairImageIndex.Value = value.Haircut;
            MainWindow.Instance.apparelLeftHandedCheckbox.IsChecked = value.LeftHanded;
            MainWindow.Instance.equipSlotBox.SelectedIndex = 0;
            MainWindow.Instance.apparelPoseBox.SelectedIndex = 0;
            MainWindow.Instance.skinColorPicker.SelectedColor = new Coloring.Color(0, 0, 0);
            MainWindow.Instance.hairColorPicker.SelectedColor = new Coloring.Color(0, 0, 0);
            conditions = value.visibilityConditions;
            loadedGUID = value.guid;
        }
        public void Save()
        {
            var character = Current;
            if (character.ID == 0)
            {
                App.NotificationManager.Notify(LocalizationManager.Current.Notification["Character_ID_Zero"]);
                return;
            }
            var o = MainWindow.CurrentProject.data.characters.Where(d => d.ID == character.ID);
            if (o.Count() > 0)
                MainWindow.CurrentProject.data.characters.Remove(o.ElementAt(0));
            MainWindow.CurrentProject.data.characters.Add(character);
            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Character_Saved"]);
            MainWindow.CurrentProject.isSaved = false;
            App.Logger.LogInfo($"Character {character.ID} saved!");
        }
        private string loadedGUID = Guid.NewGuid().ToString("N");
        public List<Condition> conditions;
        public UserColorsList UserColors { get; private set; } = new UserColorsList();

        public void SendPresence()
        {
            RichPresence presence = new RichPresence
            {
                Timestamps = new Timestamps
                {
                    StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                },
                Assets = new Assets
                {
                    SmallImageKey = "icon_info_outlined",
                    SmallImageText = $"Characters: {MainWindow.CurrentProject.data.characters.Count}"
                }//,
                //Details = $"Current NPC: {MainWindow.CharacterEditor.Current.editorName}",
                //State = $"Display Name: {MainWindow.CharacterEditor.Current.displayName}"
            };
            MainWindow.DiscordManager.SendPresence(presence);
        }
        internal void SkinColorPicker_SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance.skinColorPicker.SelectedColor != null)
                SaveColor(((Coloring.Color)MainWindow.Instance.skinColorPicker.SelectedColor.Value).ToHEX());
        }
        internal void HairColorPicker_SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance.hairColorPicker.SelectedColor != null)
                SaveColor(((Coloring.Color)MainWindow.Instance.hairColorPicker.SelectedColor.Value).ToHEX());
        }
        internal void SaveColor(string hex)
        {
            if (UserColors.data.Contains(hex))
                return;
            UserColors.data = UserColors.data.Prepend(hex).ToArray();
            UserColors.Save();
            var color = new Coloring.Color(hex);
            var colorItem = new Xceed.Wpf.Toolkit.ColorItem(color, hex);
            MainWindow.Instance.skinColorPicker.AvailableColors.Insert(0, colorItem);
        }
        internal void RemoveColor(string hex)
        {
            if (!UserColors.data.Contains(hex))
                return;
            UserColors.data = UserColors.data.Where(d => d != hex).ToArray();
            UserColors.Save();
            UpdateColorPickerFromBuffer();
        }
        internal void UpdateColorPickerFromBuffer()
        {
            MainWindow.Instance.skinColorPicker.AvailableColors.Clear();
            MainWindow.Instance.hairColorPicker.AvailableColors.Clear();
            foreach (var k in UserColors.data)
            {
                MainWindow.Instance.skinColorPicker.AvailableColors.Add(new Xceed.Wpf.Toolkit.ColorItem(new Coloring.Color(k), k));
            }
        }
        internal void SkinColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Current.SkinColor = (e.NewValue ?? new Coloring.Color(0, 0, 0));
            MainWindow.Instance.faceImageBorder.Background = new SolidColorBrush(Current.SkinColor);
            if (Coloring.ColorConverter.ColorToHSV(Current.SkinColor).V <= 0.1d)
            {
                var effect = new System.Windows.Media.Effects.DropShadowEffect
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
        }
        internal void HairColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Current.HairColor = (e.NewValue ?? new Coloring.Color(0, 0, 0));
            SolidColorBrush color = new SolidColorBrush(Current.HairColor);
            MainWindow.Instance.beardRenderGrid.DataContext = color;
            MainWindow.Instance.hairRenderGrid.DataContext = color;
        }
        internal void Apparel_Face_Random_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.faceImageIndex.Value = new Random().Next(0, MainWindow.faceAmount);
        }
        internal void Apparel_Beard_Random_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.beardImageIndex.Value = new Random().Next(0, MainWindow.beardAmount);
        }
        internal void Apparel_Hair_Random_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.hairImageIndex.Value = new Random().Next(0, MainWindow.haircutAmount);
        }
        internal void FaceImageIndex_Changed(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            MainWindow.Instance.faceImageControl.Source = ("Resources/Unturned/Faces/" + e.NewValue + ".png").GetImageSource();
            MainWindow.CurrentProject.isSaved = false;
        }
        internal void HairImageIndex_Changed(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            foreach (UIElement ui in MainWindow.Instance.hairRenderGrid.Children)
            {
                if (ui is Canvas c)
                {
                    c.Visibility = Visibility.Collapsed;
                }
            }
            MainWindow.Instance.hairRenderGrid.Children[(int)e.NewValue].Visibility = Visibility.Visible;
            MainWindow.CurrentProject.isSaved = false;
        }
        internal void BeardImageIndex_Changed(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            foreach (UIElement ui in MainWindow.Instance.beardRenderGrid.Children)
            {
                if (ui is Canvas c)
                {
                    c.Visibility = Visibility.Collapsed;
                }
            }
            MainWindow.Instance.beardRenderGrid.Children[(int)e.NewValue].Visibility = Visibility.Visible;
            MainWindow.CurrentProject.isSaved = false;
        }
        internal void Char_EditConditions_Button_Click(object sender, RoutedEventArgs e)
        {
            Universal_ListView ulv = new Universal_ListView(conditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, false)).ToList(), Universal_ItemList.ReturnType.Condition);
            ulv.ShowDialog();
            conditions = ulv.Values.Cast<Condition>().ToList();
            MainWindow.CurrentProject.isSaved = false;
        }
    }
}
