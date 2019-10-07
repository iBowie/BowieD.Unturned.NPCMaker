using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Data;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
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
        public CharacterTabViewModel()
        {
            Character = new NPCCharacter();
            UserColors = new UserColorsList();
            UserColors.Load(new string[0]);
            UpdateColorPicker();
        }
        public NPCCharacter Character { get; set; }
        public string DisplayName { get => Character.displayName; set => Character.displayName = value; }
        public string EditorName { get => Character.editorName; set => Character.editorName = value; }
        public ushort ID { get => Character.id; set => Character.id = value; }
        public ushort DialogueID { get => Character.startDialogueId; set => Character.startDialogueId = value; }
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
        public bool IsLeftHanded
        {
            get => Character.leftHanded;
            set => Character.leftHanded = value;
        }
        public NPC_Pose Pose
        {
            get => Character.pose;
            set => Character.pose = value;
        }

        private ICommand saveCommand;
        private ICommand openCommand;
        private ICommand resetCommand;
        private ICommand editVisibilityConditionsCommand;
        private ICommand randomFaceCommand;
        private ICommand randomHairCommand;
        private ICommand randomBeardCommand;
        private ICommand saveColorSkin;
        private ICommand saveColorHair;
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
                            OnPropertyChange("");
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
                        OnPropertyChange("");
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
