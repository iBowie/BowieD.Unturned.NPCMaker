using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace BowieD.Unturned.NPCMaker.BetterForms
{
    /// <summary>
    /// Логика взаимодействия для Universal_RewardEditor.xaml
    /// </summary>
    public partial class Universal_RewardEditor : Window
    {
        public Universal_RewardEditor(NPC.Reward reward = null, bool viewLocalization = false)
        {
            InitializeComponent();
            double scale = Config.Configuration.Properties.scale;
            this.Height *= scale;
            this.Width *= scale;
            baseHeight = Height;
            heightDelta *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
            viewLocalizationField = viewLocalization;
            saveButton.IsEnabled = false;
            typeBox.ItemsSource = Enum.GetValues(typeof(RewardType)).Cast<RewardType>().Where(d => d != RewardType.None).Select(d => new ComboBoxItem() { Content = (string)TryFindResource($"reward_Type_{d.ToString()}"), Tag = d });

            if (reward != null)
            {
                typeBox.SelectedIndex = (int)reward.Type - 1;
                switch (reward.Type)
                {
                    case RewardType.None:
                        break;
                    case RewardType.Experience:
                        SetMainValue(1, (reward as Experience).Value);
                        break;
                    case RewardType.Reputation:
                        SetMainValue(1, (reward as Reputation).Value);
                        break;
                    case RewardType.Quest:
                        SetMainValue(1, (reward as Quest).Id);
                        break;
                    case RewardType.Item:
                        SetMainValue(1, (reward as Item).Id);
                        SetMainValue(3, (reward as Item).Amount);
                        break;
                    case RewardType.Item_Random:
                        SetMainValue(1, (reward as Item_Random).SpawnID);
                        SetMainValue(3, (reward as Item_Random).Amount);
                        break;
                    case RewardType.Vehicle:
                        SetMainValue(1, (reward as Vehicle).Id);
                        SetMainValue(3, (reward as Vehicle).SpawnPointID);
                        break;
                    case RewardType.Teleport:
                        SetMainValue(1, (reward as Teleport).SpawnpointID);
                        break;
                    case RewardType.Flag_Bool:
                        SetMainValue(1, (reward as Flag_Bool).Id);
                        SetMainValue(3, (reward as Flag_Bool).Value);
                        break;
                    case RewardType.Flag_Short:
                        SetMainValue(1, (reward as Flag_Short).Id);
                        SetMainValue(3, (reward as Flag_Short).Value);
                        SetMainValue(5, (reward as Flag_Short).Modification);
                        break;
                    case RewardType.Flag_Short_Random:
                        SetMainValue(1, (reward as Flag_Short_Random).Id);
                        SetMainValue(3, (reward as Flag_Short_Random).MinValue);
                        SetMainValue(5, (reward as Flag_Short_Random).MaxValue);
                        SetMainValue(7, (reward as Flag_Short_Random).Modification);
                        break;
                    case RewardType.Flag_Math:
                        SetMainValue(1, (reward as Flag_Math).FlagA);
                        SetMainValue(3, (reward as Flag_Math).FlagB);
                        SetMainValue(5, (reward as Flag_Math).Operation);
                        break;
                }
                if (viewLocalization)
                    SetMainValue(variablesGrid.Children.Count-1, reward.Localization);
            }
        }

        public Reward Result { get; private set; }

        #region DESIGN VARS
        private double baseHeight = 178;
        private double heightDelta = 35;
        private double elementHeight = 32;
        private Thickness elementMargin = new Thickness(5, 5, 5, 5);
        private bool viewLocalizationField = false;
        #endregion
        #region METHODS
        private void ClearParameters()
        {
            variablesGrid.Children.Clear();
            this.Height = baseHeight;
        }
        private void AddLabel(string text)
        {
            variablesGrid.Children.Add(new Label() { VerticalContentAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, Content = text, Margin = elementMargin, Height = elementHeight });
            foreach (UIElement ui in variablesGrid.Children)
            {
                if (ui is Label l && (l.ToolTip == null || l.ToolTip.ToString() == "" || l.ToolTip.ToString().Length == 0) && l.Content != null && l.Content.ToString().Length >= 17)
                {
                    l.ToolTip = l.Content;
                    return;
                }
            }
        }
        private void AddTextBox(int maxLength)
        {
            variablesGrid.Children.Add(new TextBox() { MaxLength = maxLength, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Margin = elementMargin, Width = 100, Height = elementHeight });
        }
        private void AddComboBox<T>(IEnumerable<T> Items, string translationKeyFormat)
        {
            variablesGrid.Children.Add(new ComboBox() { ItemsSource = Items.Select(d => new ComboBoxItem() { Content = (string)TryFindResource(string.Format(translationKeyFormat, d.ToString())), Tag = d }), HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Margin = elementMargin, Width = 100, Height = elementHeight });
        }
        private void AddCheckBox(bool checkState)
        {
            variablesGrid.Children.Add(new CheckBox() { IsChecked = checkState, Margin = elementMargin, VerticalContentAlignment = VerticalAlignment.Center, Height = elementHeight, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right });
        }
        #endregion

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object[] input = Input;
                switch (Selected_Reward)
                {
                    case RewardType.None:
                        return;
                    case RewardType.Experience:
                        Experience r1 = new Experience
                        {
                            Value = uint.Parse(input[0].ToString())
                        };
                        Result = r1;
                        break;
                    case RewardType.Flag_Bool:
                        Flag_Bool r2 = new Flag_Bool()
                        {
                            Id = ushort.Parse(input[0].ToString()),
                            Value = (bool)input[1]
                        };
                        Result = r2;
                        break;
                    case RewardType.Flag_Math:
                        Flag_Math r3 = new Flag_Math()
                        {
                            FlagA = ushort.Parse(input[0].ToString()),
                            FlagB = ushort.Parse(input[1].ToString()),
                            Operation = (Operation_Type)input[2]
                        };
                        Result = r3;
                        break;
                    case RewardType.Flag_Short:
                        Flag_Short r4 = new Flag_Short()
                        {
                            Id = ushort.Parse(input[0].ToString()),
                            Value = short.Parse(input[1].ToString()),
                            Modification = (Modification_Type)input[2]
                        };
                        Result = r4;
                        break;
                    case RewardType.Flag_Short_Random:
                        Flag_Short_Random r5 = new Flag_Short_Random()
                        {
                            Id = ushort.Parse(input[0].ToString()),
                            MinValue = short.Parse(input[1].ToString()),
                            MaxValue = short.Parse(input[2].ToString()),
                            Modification = (Modification_Type)input[3]
                        };
                        Result = r5;
                        break;
                    case RewardType.Item:
                        Item r6 = new Item()
                        {
                            Id = ushort.Parse(input[0].ToString()),
                            Amount = uint.Parse(input[1].ToString())
                        };
                        Result = r6;
                        break;
                    case RewardType.Item_Random:
                        Item_Random r7 = new Item_Random()
                        {
                            SpawnID = ushort.Parse(input[0].ToString()),
                            Amount = uint.Parse(input[1].ToString())
                        };
                        Result = r7;
                        break;
                    case RewardType.Quest:
                        Quest r8 = new Quest()
                        {
                            Id = ushort.Parse(input[0].ToString())
                        };
                        Result = r8;
                        break;
                    case RewardType.Reputation:
                        Reputation r9 = new Reputation()
                        {
                            Value = int.Parse(input[0].ToString())
                        };
                        Result = r9;
                        break;
                    case RewardType.Teleport:
                        Teleport r10 = new Teleport()
                        {
                            SpawnpointID = ushort.Parse(input[0].ToString())
                        };
                        Result = r10;
                        break;
                    case RewardType.Vehicle:
                        Vehicle r11 = new Vehicle()
                        {
                            Id = ushort.Parse(input[0].ToString()),
                            SpawnPointID = ushort.Parse(input[1].ToString())
                        };
                        Result = r11;
                        break;
                }
                if (viewLocalizationField)
                    Result.Localization = input[input.Length - 1].ToString();
                DialogResult = true;
                Close();
            }
            catch { } // error messages
        }

        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveButton.IsEnabled = true;
            if (e.AddedItems.Count == 0)
                return;
            ClearParameters();
            RewardType newSelection = Selected_Reward;
            int mult = 0;
            switch (newSelection)
            {
                case RewardType.Experience:
                    mult = 1;

                    AddLabel((string)TryFindResource("rewardEditor_Amount"));
                    AddTextBox(6);
                    break;
                case RewardType.Flag_Bool:
                    mult = 2;

                    AddLabel((string)TryFindResource("rewardEditor_FlagID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("rewardEditor_Value"));
                    AddCheckBox(false);
                    break;
                case RewardType.Flag_Math:
                    mult = 3;

                    AddLabel((string)TryFindResource("rewardEditor_FlagA_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("rewardEditor_FlagB_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("rewardEditor_Operation"));
                    AddComboBox(Enum.GetValues(typeof(Operation_Type)).Cast<Operation_Type>(), "Operation_{0}");
                    break;
                case RewardType.Flag_Short:
                    mult = 3;

                    AddLabel((string)TryFindResource("rewardEditor_FlagID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("rewardEditor_Value"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("rewardEditor_Modification"));
                    AddComboBox(Enum.GetValues(typeof(Modification_Type)).Cast<Modification_Type>(), "Modification_{0}");
                    break;
                case RewardType.Flag_Short_Random:
                    mult = 4;

                    AddLabel((string)TryFindResource("rewardEditor_FlagID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("rewardEditor_MinValue"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("rewardEditor_MaxValue"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("rewardEditor_Modification"));
                    AddComboBox(Enum.GetValues(typeof(Modification_Type)).Cast<Modification_Type>(), "Modification_{0}");
                    break;
                case RewardType.Item:
                    mult = 2;

                    AddLabel((string)TryFindResource("rewardEditor_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("rewardEditor_Amount"));
                    AddTextBox(6);
                    break;
                case RewardType.Item_Random:
                    mult = 2;

                    AddLabel((string)TryFindResource("rewardEditor_SpawnID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("rewardEditor_Amount"));
                    AddTextBox(6);
                    break;
                case RewardType.Quest:
                    mult = 1;

                    AddLabel((string)TryFindResource("rewardEditor_ID"));
                    AddTextBox(5);
                    break;
                case RewardType.Reputation:
                    mult = 1;

                    AddLabel((string)TryFindResource("rewardEditor_Amount"));
                    AddTextBox(5);
                    break;
                case RewardType.Teleport:
                    mult = 1;

                    AddLabel((string)TryFindResource("rewardEditor_SpawnpointID"));
                    AddTextBox(5);
                    break;
                case RewardType.Vehicle:
                    mult = 2;

                    AddLabel((string)TryFindResource("rewardEditor_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("rewardEditor_SpawnpointID"));
                    AddTextBox(5);
                    break;
            }
            if (viewLocalizationField)
            {
                mult++;
                AddLabel((string)TryFindResource("rewardEditor_Localization"));
                AddTextBox(200);
            }
            DoubleAnimation anim = new DoubleAnimation(Height, (baseHeight + (heightDelta * (mult + (mult > 1 ? 1 : 0)))), new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            BeginAnimation(HeightProperty, anim);
        }

        private RewardType Selected_Reward => typeBox.SelectedItem is ComboBoxItem cbi && cbi.Tag is RewardType crt ? crt : RewardType.None;

        private object[] Input
        {
            get
            {
                List<object> list = new List<object>();
                for (int k = 1; k < variablesGrid.Children.Count; k += 2)
                {
                    list.Add(GetMainValue(variablesGrid.Children[k]));
                }
                return list.ToArray();
            }
        }
        private object GetMainValue(UIElement element)
        {
            if (element is TextBox a)
                return a.Text;
            if (element is CheckBox b)
                return b.IsChecked.Value;
            if (element is ComboBox c && c.SelectedItem is ComboBoxItem cc)
                return cc.Tag;
            return null;
        }
        private void SetMainValue(int index, object value)
        {
            UIElement ui = variablesGrid.Children[index];
            if (ui is TextBox a)
                a.Text = value.ToString();
            if (ui is CheckBox b && value is bool bb)
                b.IsChecked = bb;
            if (ui is ComboBox c)
            {
                for (int k = 0; k < c.Items.Count; k++)
                {
                    if (c.Items[k] is ComboBoxItem cc && cc.Tag.Equals(value))
                    {
                        c.SelectedIndex = k;
                        break;
                    }
                }
            }
        }
    }
}
