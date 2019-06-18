using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BowieD.Unturned.NPCMaker.BetterControls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using DiscordRPC;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.Editors
{
    public class CharacterEditor : IEditor<NPCCharacter>
    {
        private Func<Equip_Type> getEquipType = new Func<Equip_Type>(() =>
        {
            var item = MainWindow.Instance.equipSlotBox.SelectedItem as ComboBoxItem;
            if (item == null)
                return Equip_Type.None;
            var val = (Equip_Type)item.Tag;
            return val;
        });
        private Func<NPC_Pose> getPose = new Func<NPC_Pose>(() =>
        {
            var item = MainWindow.Instance.apparelPoseBox.SelectedItem as ComboBoxItem;
            if (item == null)
                return NPC_Pose.Stand;
            var val = (NPC_Pose)item.Tag;
            return val;
        });
        private Func<NPCColor> getHairColor = new Func<NPCColor>(() =>
        {
            //string text = MainWindow.Instance.apparelHairColorBox.Text;
            //BrushConverter bc = new BrushConverter();
            //if (bc.IsValid(text))
            //    return NPCColor.FromHEX(text);
            return new NPCColor(0, 0, 0);
        });
        private Func<NPCColor> getSkinColor = new Func<NPCColor>(() =>
        {
            //string text = MainWindow.Instance.apparelSkinColorBox.Text;
            //BrushConverter bc = new BrushConverter();
            //if (bc.IsValid(text))
            //    return NPCColor.FromHEX(text);
            return new NPCColor(0, 0, 0);
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
            MainWindow.Instance.apparelSkinColorBox.TextChanged += ApparelSkinColorBox_TextChanged;
            MainWindow.Instance.apparelHairColorBox.TextChanged += ApparelHairColorBox_TextChanged;
            MainWindow.Instance.apparelHairRandomize.Click += Apparel_Hair_Random_Button_Click;
            MainWindow.Instance.apparelBeardRandomize.Click += Apparel_Beard_Random_Button_Click;
            MainWindow.Instance.apparelFaceRandomize.Click += Apparel_Face_Random_Button_Click;
            MainWindow.Instance.visibilityCondsButton.Click += Char_EditConditions_Button_Click;
            MainWindow.Instance.apparelSkinColorBox.Text = "#000000";
            MainWindow.Instance.apparelHairColorBox.Text = "#000000";
            conditions = new List<Condition>();
        }
        public NPCCharacter Current
        {
            get
            {
                return new NPCCharacter()
                {
                    id = (ushort)(MainWindow.Instance.txtID.Value ?? 0),
                    beard = (byte)(MainWindow.Instance.beardImageIndex.Value ?? 0),
                    christmasClothing = new NPCClothing()
                    {
                        backpack = (ushort)(MainWindow.Instance.christmasbackpackIdBox.Value ?? 0),
                        bottom = (ushort)(MainWindow.Instance.christmasbottomIdBox.Value ?? 0),
                        glasses = (ushort)(MainWindow.Instance.christmasglassesIdBox.Value ?? 0),
                        hat = (ushort)(MainWindow.Instance.christmashatIdBox.Value ?? 0),
                        mask = (ushort)(MainWindow.Instance.christmasmaskIdBox.Value ?? 0),
                        top = (ushort)(MainWindow.Instance.christmastopIdBox.Value ?? 0),
                        vest = (ushort)(MainWindow.Instance.christmasvestIdBox.Value ?? 0)
                    },
                    halloweenClothing = new NPCClothing()
                    {
                        backpack = (ushort)(MainWindow.Instance.halloweenbackpackIdBox.Value ?? 0),
                        bottom = (ushort)(MainWindow.Instance.halloweenbottomIdBox.Value ?? 0),
                        glasses = (ushort)(MainWindow.Instance.halloweenglassesIdBox.Value ?? 0),
                        hat = (ushort)(MainWindow.Instance.halloweenhatIdBox.Value ?? 0),
                        mask = (ushort)(MainWindow.Instance.halloweenmaskIdBox.Value ?? 0),
                        top = (ushort)(MainWindow.Instance.halloweentopIdBox.Value ?? 0),
                        vest = (ushort)(MainWindow.Instance.halloweenvestIdBox.Value ?? 0)
                    },
                    clothing = new NPCClothing()
                    {
                        backpack = (ushort)(MainWindow.Instance.backpackIdBox.Value ?? 0),
                        bottom = (ushort)(MainWindow.Instance.bottomIdBox.Value ?? 0),
                        glasses = (ushort)(MainWindow.Instance.glassesIdBox.Value ?? 0),
                        hat = (ushort)(MainWindow.Instance.hatIdBox.Value ?? 0),
                        mask = (ushort)(MainWindow.Instance.maskIdBox.Value ?? 0),
                        top = (ushort)(MainWindow.Instance.topIdBox.Value ?? 0),
                        vest = (ushort)(MainWindow.Instance.vestIdBox.Value ?? 0)
                    },
                    displayName = MainWindow.Instance.txtDisplayName.Text ?? "",
                    editorName = MainWindow.Instance.txtEditorName.Text ?? "",
                    equipped = getEquipType.Invoke(),
                    pose = getPose.Invoke(),
                    equipPrimary = (ushort)(MainWindow.Instance.primaryIdBox.Value ?? 0),
                    equipSecondary = (ushort)(MainWindow.Instance.secondaryIdBox.Value ?? 0),
                    equipTertiary = (ushort)(MainWindow.Instance.tertiaryIdBox.Value ?? 0),
                    face = (byte)(MainWindow.Instance.faceImageIndex.Value ?? 0),
                    haircut = (byte)(MainWindow.Instance.hairImageIndex.Value ?? 0),
                    guid = loadedGUID,
                    hairColor = getHairColor.Invoke(),
                    skinColor = getSkinColor.Invoke(),
                    leftHanded = MainWindow.Instance.apparelLeftHandedCheckbox.IsChecked == true,
                    startDialogueId = (ushort)(MainWindow.Instance.txtStartDialogueID.Value ?? 0),
                    visibilityConditions = conditions
                };
            }
            set
            {
                Reset();
                MainWindow.Instance.txtID.Value = value.id;
                MainWindow.Instance.txtStartDialogueID.Value = value.startDialogueId;
                MainWindow.Instance.txtDisplayName.Text = value.displayName;
                MainWindow.Instance.txtEditorName.Text = value.editorName;
                //MainWindow.Instance.apparelSkinColorBox.Text = value.skinColor.HEX;
                //MainWindow.Instance.apparelHairColorBox.Text = value.hairColor.HEX;
                MainWindow.Instance.backpackIdBox.Value = value.clothing.backpack;
                MainWindow.Instance.maskIdBox.Value = value.clothing.mask;
                MainWindow.Instance.vestIdBox.Value = value.clothing.vest;
                MainWindow.Instance.topIdBox.Value = value.clothing.top;
                MainWindow.Instance.bottomIdBox.Value = value.clothing.bottom;
                MainWindow.Instance.glassesIdBox.Value = value.clothing.glasses;
                MainWindow.Instance.hatIdBox.Value = value.clothing.hat;
                MainWindow.Instance.halloweenbackpackIdBox.Value = value.halloweenClothing.backpack;
                MainWindow.Instance.halloweenmaskIdBox.Value = value.halloweenClothing.mask;
                MainWindow.Instance.halloweenvestIdBox.Value = value.halloweenClothing.vest;
                MainWindow.Instance.halloweentopIdBox.Value = value.halloweenClothing.top;
                MainWindow.Instance.halloweenbottomIdBox.Value = value.halloweenClothing.bottom;
                MainWindow.Instance.halloweenglassesIdBox.Value = value.halloweenClothing.glasses;
                MainWindow.Instance.halloweenhatIdBox.Value = value.halloweenClothing.hat;
                MainWindow.Instance.christmasbackpackIdBox.Value = value.christmasClothing.backpack;
                MainWindow.Instance.christmasmaskIdBox.Value = value.christmasClothing.mask;
                MainWindow.Instance.christmasvestIdBox.Value = value.christmasClothing.vest;
                MainWindow.Instance.christmastopIdBox.Value = value.christmasClothing.top;
                MainWindow.Instance.christmasbottomIdBox.Value = value.christmasClothing.bottom;
                MainWindow.Instance.christmasglassesIdBox.Value = value.christmasClothing.glasses;
                MainWindow.Instance.christmashatIdBox.Value = value.christmasClothing.hat;
                MainWindow.Instance.primaryIdBox.Value = value.equipPrimary;
                MainWindow.Instance.secondaryIdBox.Value = value.equipSecondary;
                MainWindow.Instance.tertiaryIdBox.Value = value.equipTertiary;
                MainWindow.Instance.faceImageIndex.Value = value.face;
                MainWindow.Instance.beardImageIndex.Value = value.beard;
                MainWindow.Instance.hairImageIndex.Value = value.haircut;
                MainWindow.Instance.apparelLeftHandedCheckbox.IsChecked = value.leftHanded;
                for (int k = 0; k < MainWindow.Instance.apparelPoseBox.Items.Count; k++)
                {
                    if (((NPC_Pose)(MainWindow.Instance.apparelPoseBox.Items[k] as ComboBoxItem).Tag) == value.pose)
                    {
                        MainWindow.Instance.apparelPoseBox.SelectedIndex = k;
                        break;
                    }
                }
                for (int k = 0; k < MainWindow.Instance.equipSlotBox.Items.Count; k++)
                {
                    if (((Equip_Type)(MainWindow.Instance.equipSlotBox.Items[k] as ComboBoxItem).Tag) == value.equipped)
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
            var ulv = new Universal_ListView(MainWindow.CurrentProject.characters.OrderBy(d => d.id).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Character, false)).ToList(), Universal_ItemList.ReturnType.Character);
            if (ulv.ShowDialog() == true)
            {
                Save();
                Current = ulv.SelectedValue as NPCCharacter;
                Logger.Log($"Opened dialogue {MainWindow.Instance.txtID.Value}");
            }
            MainWindow.CurrentProject.characters = ulv.Values.Cast<NPCCharacter>().ToList();
        }

        public void Reset()
        {
            NPCCharacter value = new NPCCharacter();
            MainWindow.Instance.txtID.Value = value.id;
            MainWindow.Instance.txtStartDialogueID.Value = value.startDialogueId;
            MainWindow.Instance.txtDisplayName.Text = value.displayName;
            MainWindow.Instance.txtEditorName.Text = value.editorName;
            //MainWindow.Instance.apparelSkinColorBox.Text = value.skinColor.HEX;
            //MainWindow.Instance.apparelHairColorBox.Text = value.hairColor.HEX;
            MainWindow.Instance.backpackIdBox.Value = value.clothing.backpack;
            MainWindow.Instance.maskIdBox.Value = value.clothing.mask;
            MainWindow.Instance.vestIdBox.Value = value.clothing.vest;
            MainWindow.Instance.topIdBox.Value = value.clothing.top;
            MainWindow.Instance.bottomIdBox.Value = value.clothing.bottom;
            MainWindow.Instance.glassesIdBox.Value = value.clothing.glasses;
            MainWindow.Instance.hatIdBox.Value = value.clothing.hat;
            MainWindow.Instance.halloweenbackpackIdBox.Value = value.halloweenClothing.backpack;
            MainWindow.Instance.halloweenmaskIdBox.Value = value.halloweenClothing.mask;
            MainWindow.Instance.halloweenvestIdBox.Value = value.halloweenClothing.vest;
            MainWindow.Instance.halloweentopIdBox.Value = value.halloweenClothing.top;
            MainWindow.Instance.halloweenbottomIdBox.Value = value.halloweenClothing.bottom;
            MainWindow.Instance.halloweenglassesIdBox.Value = value.halloweenClothing.glasses;
            MainWindow.Instance.halloweenhatIdBox.Value = value.halloweenClothing.hat;
            MainWindow.Instance.christmasbackpackIdBox.Value = value.christmasClothing.backpack;
            MainWindow.Instance.christmasmaskIdBox.Value = value.christmasClothing.mask;
            MainWindow.Instance.christmasvestIdBox.Value = value.christmasClothing.vest;
            MainWindow.Instance.christmastopIdBox.Value = value.christmasClothing.top;
            MainWindow.Instance.christmasbottomIdBox.Value = value.christmasClothing.bottom;
            MainWindow.Instance.christmasglassesIdBox.Value = value.christmasClothing.glasses;
            MainWindow.Instance.christmashatIdBox.Value = value.christmasClothing.hat;
            MainWindow.Instance.primaryIdBox.Value = value.equipPrimary;
            MainWindow.Instance.secondaryIdBox.Value = value.equipSecondary;
            MainWindow.Instance.tertiaryIdBox.Value = value.equipTertiary;
            MainWindow.Instance.faceImageIndex.Value = value.face;
            MainWindow.Instance.beardImageIndex.Value = value.beard;
            MainWindow.Instance.hairImageIndex.Value = value.haircut;
            MainWindow.Instance.apparelLeftHandedCheckbox.IsChecked = value.leftHanded;
            MainWindow.Instance.equipSlotBox.SelectedIndex = 0;
            MainWindow.Instance.apparelPoseBox.SelectedIndex = 0;
        }

        public void Save()
        {
            var character = Current;
            if (character.id == 0)
            {
                MainWindow.NotificationManager.Notify(LocUtil.LocalizeInterface("character_ID_Zero"));
                return;
            }
            var o = MainWindow.CurrentProject.characters.Where(d => d.id == character.id);
            if (o.Count() > 0)
                MainWindow.CurrentProject.characters.Remove(o.ElementAt(0));
            MainWindow.CurrentProject.characters.Add(character);
            MainWindow.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Character_Saved"));
            MainWindow.isSaved = false;
            Logger.Log($"Character {character.id} saved!");
        }

        private string loadedGUID;
        public List<Condition> conditions;

        public void SendPresence()
        {
            RichPresence presence = new RichPresence();
            presence.Timestamps = new Timestamps();
            presence.Timestamps.StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            presence.Assets = new Assets();
            presence.Assets.SmallImageKey = "icon_info_outlined";
            presence.Assets.SmallImageText = $"Characters: {MainWindow.CurrentProject.characters.Count}";
            presence.Details = $"Current NPC: {MainWindow.CharacterEditor.Current.editorName}";
            presence.State = $"Display Name: {MainWindow.CharacterEditor.Current.displayName}";
            MainWindow.DiscordManager.SendPresence(presence);
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
        internal void ApparelSkinColorBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = MainWindow.Instance.apparelSkinColorBox.Text;
            BrushConverter bc = new BrushConverter();
            if (bc.IsValid(text))
            {
                SolidColorBrush color = bc.ConvertFromString(text) as SolidColorBrush;
                MainWindow.Instance.faceImageBorder.Background = color;
                //NPCColor clr = NPCColor.FromBrush(color);
                //if (clr.HSV.V <= 0.1d)
                //{
                //    var effect = new System.Windows.Media.Effects.DropShadowEffect
                //    {
                //        BlurRadius = 2,
                //        Direction = 0,
                //        Color = Brushes.White.Color,
                //        ShadowDepth = 0
                //    };
                //    MainWindow.Instance.faceImageControl.Effect = effect;
                //}
                //else
                //{
                //    MainWindow.Instance.faceImageControl.Effect = null;
                //}
            }
            else
            {
                MainWindow.Instance.faceImageBorder.Background = Brushes.Transparent;
            }
        }
        internal void ApparelHairColorBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = MainWindow.Instance.apparelHairColorBox.Text;
            BrushConverter bc = new BrushConverter();
            if (bc.IsValid(text))
            {
                SolidColorBrush color = bc.ConvertFromString(text) as SolidColorBrush;
                MainWindow.Instance.beardRenderGrid.DataContext = color;
                MainWindow.Instance.hairRenderGrid.DataContext = color;
            }
            else
            {
                MainWindow.Instance.beardRenderGrid.DataContext = Brushes.Black;
                MainWindow.Instance.hairRenderGrid.DataContext = Brushes.Black;
            }
        }
        internal void FaceImageIndex_Changed(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            MainWindow.Instance.faceImageControl.Source = ("Resources/Unturned/Faces/" + e.NewValue + ".png").GetImageSource();
            MainWindow.isSaved = false;
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
            MainWindow.isSaved = false;
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
            MainWindow.isSaved = false;
        }
        internal void Char_EditConditions_Button_Click(object sender, RoutedEventArgs e)
        {
            Universal_ListView ulv = new Universal_ListView(conditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, false)).ToList(), Universal_ItemList.ReturnType.Condition);
            ulv.ShowDialog();
            conditions = ulv.Values.Cast<Condition>().ToList();
            MainWindow.isSaved = false;
        }
    }
}
