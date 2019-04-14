using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BowieD.Unturned.NPCMaker.BetterControls;
using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;

namespace BowieD.Unturned.NPCMaker.Editors
{
    public class CharacterEditor : IEditor<NPCCharacter>
    {
        public CharacterEditor()
        {
            MainWindow.Instance.characterResetButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Reset();
            });
            MainWindow.Instance.characterOpenButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Open();
            });
            MainWindow.Instance.characterSaveButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Save();
            });
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
                    equipped = new Func<Equip_Type>(() =>
                    {
                        var item = MainWindow.Instance.equipSlotBox.SelectedItem as ComboBoxItem;
                        if (item == null)
                            return Equip_Type.None;
                        var val = (Equip_Type)item.Tag;
                        return val;
                    }).Invoke(),
                    pose = new Func<NPC_Pose>(() =>
                    {
                        var item = MainWindow.Instance.apparelPoseBox.SelectedItem as ComboBoxItem;
                        if (item == null)
                            return NPC_Pose.Stand;
                        var val = (NPC_Pose)item.Tag;
                        return val;
                    }).Invoke(),
                    equipPrimary = (ushort)(MainWindow.Instance.primaryIdBox.Value ?? 0),
                    equipSecondary = (ushort)(MainWindow.Instance.secondaryIdBox.Value ?? 0),
                    equipTertiary = (ushort)(MainWindow.Instance.tertiaryIdBox.Value ?? 0),
                    face = (byte)(MainWindow.Instance.faceImageIndex.Value ?? 0),
                    haircut = (byte)(MainWindow.Instance.hairImageIndex.Value ?? 0),
                    guid = loadedGUID,
                    hairColor = new Func<NPCColor>(() =>
                    {
                        string text = MainWindow.Instance.apparelHairColorBox.Text;
                        BrushConverter bc = new BrushConverter();
                        if (bc.IsValid(text))
                            return new NPCColor() { HEX = text };
                        return new NPCColor(0, 0, 0);
                    }).Invoke(),
                    skinColor = new Func<NPCColor>(() =>
                    {
                        string text = MainWindow.Instance.apparelSkinColorBox.Text;
                        BrushConverter bc = new BrushConverter();
                        if (bc.IsValid(text))
                            return new NPCColor() { HEX = text };
                        return new NPCColor(0, 0, 0);
                    }).Invoke(),
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
                MainWindow.Instance.apparelSkinColorBox.Text = value.skinColor.HEX;
                MainWindow.Instance.apparelHairColorBox.Text = value.hairColor.HEX;
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
                conditions = value.visibilityConditions;
                loadedGUID = value.guid;
            }
        }
        public void Open()
        {
            var ulv = new Universal_ListView(MainWindow.CurrentSave.characters.OrderBy(d => d.id).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Character, false)).ToList(), Universal_ItemList.ReturnType.Character);
            if (ulv.ShowDialog() == true)
            {
                Save();
                Current = ulv.SelectedValue as NPCCharacter;
                Logger.Log($"Opened dialogue {MainWindow.Instance.txtID.Value}");
            }
            MainWindow.CurrentSave.characters = ulv.Values.Cast<NPCCharacter>().ToList();
        }

        public void Reset()
        {
            NPCCharacter value = new NPCCharacter();
            MainWindow.Instance.txtID.Value = value.id;
            MainWindow.Instance.txtStartDialogueID.Value = value.startDialogueId;
            MainWindow.Instance.txtDisplayName.Text = value.displayName;
            MainWindow.Instance.txtEditorName.Text = value.editorName;
            MainWindow.Instance.apparelSkinColorBox.Text = value.skinColor.HEX;
            MainWindow.Instance.apparelHairColorBox.Text = value.hairColor.HEX;
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
        }

        public void Save()
        {
            var character = Current;
            if (character.id == 0)
            {
                MainWindow.NotificationManager.Notify(MainWindow.Localize("character_ID_Zero"));
                return;
            }
            var o = MainWindow.CurrentSave.characters.Where(d => d.id == character.id);
            if (o.Count() > 0)
                MainWindow.CurrentSave.characters.Remove(o.ElementAt(0));
            MainWindow.CurrentSave.characters.Add(character);
            MainWindow.NotificationManager.Notify(MainWindow.Localize("notify_Character_Saved"));
            MainWindow.isSaved = false;
            Logger.Log($"Character {character.id} saved!");
        }

        private string loadedGUID;
        public List<NPC.Condition> conditions;
    }
}
