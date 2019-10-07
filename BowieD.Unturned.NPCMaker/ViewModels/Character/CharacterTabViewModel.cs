using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Data;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.ViewModels.Character
{
    public sealed class CharacterTabViewModel : BaseViewModel
    {
        private NPCCharacter _character;
        public CharacterTabViewModel()
        {
            Character = new NPCCharacter();
            UserColors = new UserColorsList();
            UserColors.Load(new string[0]);
            UpdateColorPicker();
        }
        public NPCCharacter Character
        {
            get => _character;
            set
            {
                _character = value;
                OnPropertyChange("");
            }
        }
        public string DisplayName { get => Character.displayName; set => Character.displayName = value; }
        public string EditorName { get => Character.editorName; set => Character.editorName = value; }
        public ushort ID { get => Character.id; set => Character.id = value; }
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
                    var effect = new DropShadowEffect
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
            }
        }
        public bool IsLeftHanded { get => Character.leftHanded; set => Character.leftHanded = value; }
        public NPC_Pose Pose { get => Character.pose; set => Character.pose = value; }
        public ushort DefaultHat { get => Character.clothing.hat; set => Character.clothing.hat = value; }
        public ushort DefaultMask { get => Character.clothing.mask; set => Character.clothing.mask = value; }
        public ushort DefaultGlasses { get => Character.clothing.glasses; set => Character.clothing.glasses = value; }
        public ushort DefaultShirt { get => Character.clothing.top; set => Character.clothing.top = value; }
        public ushort DefaultPants { get => Character.clothing.bottom; set => Character.clothing.bottom = value; }
        public ushort DefaultVest { get => Character.clothing.vest; set => Character.clothing.vest= value; }
        public ushort DefaultBackpack { get => Character.clothing.backpack; set => Character.clothing.backpack = value; }
        public ushort ChristmasHat { get => Character.christmasClothing.hat; set => Character.christmasClothing.hat = value; }
        public ushort ChristmasMask { get => Character.christmasClothing.mask; set => Character.christmasClothing.mask = value; }
        public ushort ChristmasGlasses { get => Character.christmasClothing.glasses; set => Character.christmasClothing.glasses = value; }
        public ushort ChristmasShirt { get => Character.christmasClothing.top; set => Character.christmasClothing.top = value; }
        public ushort ChristmasPants { get => Character.christmasClothing.bottom; set => Character.christmasClothing.bottom = value; }
        public ushort ChristmasVest { get => Character.christmasClothing.vest; set => Character.christmasClothing.vest = value; }
        public ushort ChristmasBackpack { get => Character.christmasClothing.backpack; set => Character.christmasClothing.backpack = value; }
        public ushort HalloweenHat { get => Character.halloweenClothing.hat; set => Character.halloweenClothing.hat = value; }
        public ushort HalloweenMask { get => Character.halloweenClothing.mask; set => Character.halloweenClothing.mask = value; }
        public ushort HalloweenGlasses { get => Character.halloweenClothing.glasses; set => Character.halloweenClothing.glasses = value; }
        public ushort HalloweenShirt { get => Character.halloweenClothing.top; set => Character.halloweenClothing.top = value; }
        public ushort HalloweenPants { get => Character.halloweenClothing.bottom; set => Character.halloweenClothing.bottom = value; }
        public ushort HalloweenVest { get => Character.halloweenClothing.vest; set => Character.halloweenClothing.vest = value; }
        public ushort HalloweenBackpack { get => Character.halloweenClothing.backpack; set => Character.halloweenClothing.backpack = value; }
        public ushort EquipmentPrimary { get => Character.equipPrimary; set => Character.equipPrimary = value; }
        public ushort EquipmentSecondary { get => Character.equipSecondary; set => Character.equipSecondary = value; }
        public ushort EquipmentTertiary { get => Character.equipTertiary; set => Character.equipTertiary = value; }
        public Equip_Type Equipped { get => Character.equipped; set => Character.equipped = value; }

        private ICommand saveCommand;
        private ICommand openCommand;
        private ICommand resetCommand;
        private ICommand editVisibilityConditionsCommand;
        private ICommand randomFaceCommand;
        private ICommand randomHairCommand;
        private ICommand randomBeardCommand;
        private ICommand saveColorSkin;
        private ICommand saveColorHair;
        private ICommand regenerateGUIDsCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new BaseCommand(() =>
                    {
                        if (Character.id == 0)
                        {
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Character_ID_Zero"]);
                            return;
                        }
                        MainWindow.CurrentProject.data.characters.RemoveAll(d => d.id == Character.id);
                        MainWindow.CurrentProject.data.characters.Add(Character);
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["Character_Saved"]);
                        MainWindow.CurrentProject.isSaved = false;
                        App.Logger.LogInfo($"Character {Character.id} saved!");
                    });
                }
                return saveCommand;
            }
        }
        public ICommand OpenCommand
        {
            get
            {
                if (openCommand == null)
                {
                    openCommand = new BaseCommand(() =>
                    {
                        var ulv = new Universal_ListView(MainWindow.CurrentProject.data.characters.OrderBy(d => d.id).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Character, false)).ToList(), Universal_ItemList.ReturnType.Character);
                        if (ulv.ShowDialog() == true)
                        {
                            SaveCommand.Execute(null);
                            Character = ulv.SelectedValue as NPCCharacter;
                            App.Logger.LogInfo($"Opened character {ID}");
                        }
                        MainWindow.CurrentProject.data.characters = ulv.Values.Cast<NPCCharacter>().ToList();
                    });
                }
                return openCommand;
            }
        }
        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new BaseCommand(() =>
                    {
                        Character = new NPCCharacter();
                    });
                }
                return resetCommand;
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
                        Universal_ListView ulv = new Universal_ListView(Character.visibilityConditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, false)).ToList(), Universal_ItemList.ReturnType.Condition);
                        ulv.ShowDialog();
                        Character.visibilityConditions = ulv.Values.Cast<Condition>().ToList();
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
        public ICommand SaveColorSkin
        {
            get
            {
                if (saveColorSkin == null)
                {
                    saveColorSkin = new BaseCommand(() =>
                    {
                        if (MainWindow.Instance.skinColorPicker.SelectedColor != null)
                            SaveColor(((Coloring.Color)MainWindow.Instance.skinColorPicker.SelectedColor.Value).ToHEX());
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
                            SaveColor(((Coloring.Color)MainWindow.Instance.hairColorPicker.SelectedColor.Value).ToHEX());
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
                                    c.guid = Guid.NewGuid().ToString("N");
                            }
                        }
                        if (MainWindow.CurrentProject.data.dialogues != null)
                        {
                            foreach (NPCDialogue d in MainWindow.CurrentProject.data.dialogues)
                            {
                                if (d != null)
                                    d.guid = Guid.NewGuid().ToString("N");
                            }
                        }
                        if (MainWindow.CurrentProject.data.vendors != null)
                        {
                            foreach (NPCVendor v in MainWindow.CurrentProject.data.vendors)
                            {
                                if (v != null)
                                    v.guid = Guid.NewGuid().ToString("N");
                            }
                        }
                        if (MainWindow.CurrentProject.data.quests != null)
                        {
                            foreach (NPCQuest q in MainWindow.CurrentProject.data.quests)
                            {
                                if (q != null)
                                    q.guid = Guid.NewGuid().ToString("N");
                            }
                        }
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["App_GUID_Regenerated"]);
                    });
                }
                return regenerateGUIDsCommand;
            }
        }
        private void SaveColor(string hex)
        {
            if (UserColors.data.Contains(hex))
                return;
            UserColors.data = UserColors.data.Prepend(hex).ToArray();
            UserColors.Save();
            var color = new Coloring.Color(hex);
            var colorItem = new Xceed.Wpf.Toolkit.ColorItem(color, hex);
            MainWindow.Instance.skinColorPicker.AvailableColors.Insert(0, colorItem);
        }
        private readonly UserColorsList UserColors;
        private void UpdateColorPicker()
        {
            MainWindow.Instance.skinColorPicker.AvailableColors.Clear();
            MainWindow.Instance.hairColorPicker.AvailableColors.Clear();
            foreach (var k in UserColors.data)
            {
                MainWindow.Instance.skinColorPicker.AvailableColors.Add(new Xceed.Wpf.Toolkit.ColorItem(new Coloring.Color(k), k));
            }
        }
    }
}
