using BowieD.Unturned.NPCMaker;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
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
                NPCCharacter selectedTabChar = tabItem.DataContext as NPCCharacter;
                if (selectedTabChar != null)
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
            MetroTabItem tabItem = new MetroTabItem();
            tabItem.CloseButtonEnabled = true;
            tabItem.CloseTabCommand = CloseTabCommand;
            tabItem.CloseTabCommandParameter = tabItem;
            var binding = new Binding()
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("UIText")
            };
            Label l = new Label();
            l.SetBinding(Label.ContentProperty, binding);
            tabItem.Header = l;
            tabItem.DataContext = character;
            return tabItem;
        }

        public NPCCharacter Character
        {
            get => _character;
            set
            {
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
        public ushort EquipmentPrimary { get => Character.equipPrimary; set => Character.equipPrimary = value; }
        public ushort EquipmentSecondary { get => Character.equipSecondary; set => Character.equipSecondary = value; }
        public ushort EquipmentTertiary { get => Character.equipTertiary; set => Character.equipTertiary = value; }
        public Equip_Type Equipped { get => Character.equipped; set => Character.equipped = value; }

        private ICommand editVisibilityConditionsCommand;
        private ICommand randomFaceCommand;
        private ICommand randomHairCommand;
        private ICommand randomBeardCommand;
        private ICommand saveColorSkin;
        private ICommand saveColorHair;
        private ICommand regenerateGUIDsCommand;
        private ICommand poseEditorCommand;
        private ICommand closeTabCommand;
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
                        Universal_ListView ulv = new Universal_ListView(Character.visibilityConditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, true)).ToList(), Universal_ItemList.ReturnType.Condition);
                        ulv.Owner = MainWindow.Instance;
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
                                    c.guid = Guid.NewGuid().ToString("N");
                                }
                            }
                        }
                        if (MainWindow.CurrentProject.data.dialogues != null)
                        {
                            foreach (NPCDialogue d in MainWindow.CurrentProject.data.dialogues)
                            {
                                if (d != null)
                                {
                                    d.guid = Guid.NewGuid().ToString("N");
                                }
                            }
                        }
                        if (MainWindow.CurrentProject.data.vendors != null)
                        {
                            foreach (NPCVendor v in MainWindow.CurrentProject.data.vendors)
                            {
                                if (v != null)
                                {
                                    v.guid = Guid.NewGuid().ToString("N");
                                }
                            }
                        }
                        if (MainWindow.CurrentProject.data.quests != null)
                        {
                            foreach (NPCQuest q in MainWindow.CurrentProject.data.quests)
                            {
                                if (q != null)
                                {
                                    q.guid = Guid.NewGuid().ToString("N");
                                }
                            }
                        }
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
    }
}
