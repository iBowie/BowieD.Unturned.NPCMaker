using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Condition = BowieD.Unturned.NPCMaker.NPC.Condition;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using System.Windows.Media.Animation;

namespace BowieD.Unturned.NPCMaker.BetterForms
{
    /// <summary>
    /// Логика взаимодействия для Universal_ConditionEditor.xaml
    /// </summary>
    public partial class Universal_ConditionEditor : Window
    {
        public Universal_ConditionEditor(Condition condition = null, bool viewLocalization = false)
        {
            InitializeComponent();
            double scale = Config.Configuration.Properties.scale;
            ClearParameters();
            this.Height *= scale;
            this.Width *= scale;
            baseHeight = Height;
            heightDelta *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
            Condition startCondition = condition ?? new Condition();
            typeBox.ItemsSource = Enum.GetValues(typeof(Condition_Type)).Cast<Condition_Type>().Where(d => d != Condition_Type.None).Select(d => new ComboBoxItem() { Content = (string)TryFindResource($"Condition_{d.ToString()}"), Tag = d }).ToList();
            saveButton.IsEnabled = startCondition.Type != Condition_Type.None;
            #region CONDITION INIT
            viewLocalizationField = viewLocalization;
            SelectConditionType(startCondition.Type);
            switch (startCondition.Type)
            {
                case Condition_Type.Kills_Player:
                    SetMainValue(1, (startCondition as Kills_Players_Cond).ID);
                    SetMainValue(3, (startCondition as Kills_Players_Cond).Value);
                    SetMainValue(5, startCondition.Reset);
                    break;
                case Condition_Type.Experience:
                    SetMainValue(1, (startCondition as Experience_Cond).Logic);
                    SetMainValue(3, (startCondition as Experience_Cond).Value);
                    SetMainValue(5, startCondition.Reset);
                    break;
                case Condition_Type.Flag_Bool:
                    SetMainValue(1, (startCondition as Flag_Bool_Cond).Id);
                    SetMainValue(3, (startCondition as Flag_Bool_Cond).Value);
                    SetMainValue(5, (startCondition as Flag_Bool_Cond).AllowUnset);
                    SetMainValue(7, startCondition.Reset);
                    break;
                case Condition_Type.Flag_Short:
                    SetMainValue(1, (startCondition as Flag_Short_Cond).Id);
                    SetMainValue(3, (startCondition as Flag_Short_Cond).Logic);
                    SetMainValue(5, (startCondition as Flag_Short_Cond).Value);
                    SetMainValue(7, (startCondition as Flag_Short_Cond).AllowUnset);
                    SetMainValue(9, startCondition.Reset);
                    break;
                case Condition_Type.Item:
                    SetMainValue(1, (startCondition as Item_Cond).Id);
                    SetMainValue(3, (startCondition as Item_Cond).Amount);
                    SetMainValue(5, startCondition.Reset);
                    break;
                case Condition_Type.Kills_Animal:
                    SetMainValue(1, (startCondition as Kills_Animal_Cond).ID);
                    SetMainValue(3, (startCondition as Kills_Animal_Cond).Animal);
                    SetMainValue(5, (startCondition as Kills_Animal_Cond).Value);
                    SetMainValue(7, startCondition.Reset);
                    break;
                case Condition_Type.Kills_Horde:
                    SetMainValue(1, (startCondition as Kills_Horde_Cond).ID);
                    SetMainValue(3, (startCondition as Kills_Horde_Cond).Value);
                    SetMainValue(5, (startCondition as Kills_Horde_Cond).Navmesh);
                    SetMainValue(7, startCondition.Reset);
                    break;
                case Condition_Type.Kills_Zombie:
                    SetMainValue(1, (startCondition as Kills_Zombie_Cond).Zombie_Type);
                    SetMainValue(3, (startCondition as Kills_Zombie_Cond).Id);
                    SetMainValue(5, (startCondition as Kills_Zombie_Cond).Amount);
                    SetMainValue(7, (startCondition as Kills_Zombie_Cond).Spawn);
                    SetMainValue(9, (startCondition as Kills_Zombie_Cond).NavMesh);
                    SetMainValue(11, startCondition.Reset);
                    break;
                case Condition_Type.Player_Life_Food:
                    SetMainValue(1, (startCondition as Player_Life_Food_Cond).Logic);
                    SetMainValue(3, (startCondition as Player_Life_Food_Cond).Value);
                    break;
                case Condition_Type.Player_Life_Health:
                    SetMainValue(1, (startCondition as Player_Life_Health_Cond).Logic);
                    SetMainValue(3, (startCondition as Player_Life_Health_Cond).Value);
                    break;
                case Condition_Type.Player_Life_Virus:
                    SetMainValue(1, (startCondition as Player_Life_Virus_Cond).Logic);
                    SetMainValue(3, (startCondition as Player_Life_Virus_Cond).Value);
                    break;
                case Condition_Type.Player_Life_Water:
                    SetMainValue(1, (startCondition as Player_Life_Water_Cond).Logic);
                    SetMainValue(3, (startCondition as Player_Life_Water_Cond).Value);
                    break;
                case Condition_Type.Quest:
                    SetMainValue(1, (startCondition as Quest_Cond).Logic);
                    SetMainValue(3, (startCondition as Quest_Cond).Id);
                    SetMainValue(5, (startCondition as Quest_Cond).Status);
                    SetMainValue(7, startCondition.Reset);
                    break;
                case Condition_Type.Reputation:
                    SetMainValue(1, (startCondition as Reputation_Cond).Logic);
                    SetMainValue(3, (startCondition as Reputation_Cond).Value);
                    break;
                case Condition_Type.Skillset:
                    SetMainValue(1, (startCondition as Skillset_Cond).Logic);
                    SetMainValue(3, (startCondition as Skillset_Cond).Value);
                    break;
                case Condition_Type.Time_Of_Day:
                    SetMainValue(1, (startCondition as Time_Of_Day_Cond).Logic);
                    SetMainValue(3, (startCondition as Time_Of_Day_Cond).Second);
                    break;
            }
            if (viewLocalization)
            {
                SetMainValue(variablesGrid.Children.Count - 1, startCondition.Localization);
            }
            #endregion
        }

        private void SelectConditionType(Condition_Type cType)
        {
            if (cType == Condition_Type.None)
                typeBox.SelectedIndex = -1;
            else
            {
                for (int k = 0; k < typeBox.Items.Count; k++)
                {
                    if ((typeBox.Items[k] as ComboBoxItem).Tag is Condition_Type ctype && ctype == cType)
                    {
                        typeBox.SelectedIndex = k;
                        return;
                    }
                }
            }
        }

        #region DESIGN VARS
        private double baseHeight = 178;
        private double heightDelta = 35;
        //private double elementHeight = 24;
        private double elementHeight = 32;
        private Thickness elementMargin = new Thickness(5, 5, 5, 5);
        private bool viewLocalizationField = false;
        #endregion
        public Condition Result { get; private set; }

        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveButton.IsEnabled = true;
            if (e.AddedItems.Count == 0)
                return;
            ClearParameters();
            Condition_Type newSelection = (Condition_Type)(typeBox.SelectedItem as ComboBoxItem).Tag;
            int mult = 0;
            switch (newSelection)
            {
                case Condition_Type.Kills_Player:
                    mult = 3;

                    AddLabel((string)TryFindResource("conditionEditor_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("conditionEditor_Value"));
                    AddTextBox(5);

                    AddResetLabelAndCheckbox("Kills_Player");
                    break;
                case Condition_Type.Experience:
                    mult = 3;

                    AddLabel((string)TryFindResource("conditionEditor_LogicType"));
                    AddLogicBox();

                    AddLabel((string)TryFindResource("conditionEditor_Amount"));
                    AddTextBox(6);

                    AddResetLabelAndCheckbox("Experience");

                    break;
                case Condition_Type.Flag_Bool:
                    mult = 4;

                    AddLabel((string)TryFindResource("conditionEditor_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("conditionEditor_Value"));
                    AddCheckBox(false);

                    AddLabel((string)TryFindResource("conditionEditor_AllowUnset"));
                    AddCheckBox(true);

                    AddResetLabelAndCheckbox("Flag_Bool");
                    break;
                case Condition_Type.Flag_Short:
                    mult = 5;

                    AddLabel((string)TryFindResource("conditionEditor_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("conditionEditor_LogicType"));
                    AddLogicBox();

                    AddLabel((string)TryFindResource("conditionEditor_Value"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("conditionEditor_AllowUnset"));
                    AddCheckBox(true);

                    AddResetLabelAndCheckbox("Flag_Short");
                    break;
                case Condition_Type.Item:
                    mult = 3;

                    AddLabel((string)TryFindResource("conditionEditor_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("conditionEditor_Amount"));
                    AddTextBox(6);

                    AddResetLabelAndCheckbox("Item");
                    break;
                case Condition_Type.Kills_Animal:
                    mult = 4;

                    AddLabel((string)TryFindResource("conditionEditor_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("conditionEditor_Animal"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("conditionEditor_Amount"));
                    AddTextBox(6);

                    AddResetLabelAndCheckbox("Kills_Animal");
                    break;
                case Condition_Type.Kills_Horde:
                    mult = 4;

                    AddLabel((string)TryFindResource("conditionEditor_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("conditionEditor_Value"));
                    AddTextBox(6);

                    AddLabel((string)TryFindResource("conditionEditor_Navmesh"));
                    AddTextBox(5);

                    AddResetLabelAndCheckbox("Kills_Horde");
                    break;
                case Condition_Type.Kills_Zombie:
                    mult = 6;
                    // zombie id value spawn nav reset
                    AddLabel((string)TryFindResource("conditionEditor_Zombies"));
                    AddComboBox(Enum.GetValues(typeof(Zombie_Type)).Cast<Zombie_Type>(), "Zombie_{0}");

                    AddLabel((string)TryFindResource("conditionEditor_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("conditionEditor_Amount"));
                    AddTextBox(6);

                    AddLabel((string)TryFindResource("conditionEditor_Spawn"));
                    AddCheckBox(false);

                    AddLabel((string)TryFindResource("conditionEditor_Navmesh"));
                    AddTextBox(5);

                    AddResetLabelAndCheckbox("Kills_Zombie");
                    break;
                case Condition_Type.Player_Life_Food:
                    mult = 2;

                    AddLabel((string)TryFindResource("conditionEditor_LogicType"));
                    AddLogicBox();

                    AddLabel((string)TryFindResource("conditionEditor_Value"));
                    AddTextBox(6);
                    break;
                case Condition_Type.Player_Life_Health:
                    mult = 2;

                    AddLabel((string)TryFindResource("conditionEditor_LogicType"));
                    AddLogicBox();

                    AddLabel((string)TryFindResource("conditionEditor_Value"));
                    AddTextBox(6);
                    break;
                case Condition_Type.Player_Life_Virus:
                    mult = 2;

                    AddLabel((string)TryFindResource("conditionEditor_LogicType"));
                    AddLogicBox();

                    AddLabel((string)TryFindResource("conditionEditor_Value"));
                    AddTextBox(6);
                    break;
                case Condition_Type.Player_Life_Water:
                    mult = 2;

                    AddLabel((string)TryFindResource("conditionEditor_LogicType"));
                    AddLogicBox();

                    AddLabel((string)TryFindResource("conditionEditor_Value"));
                    AddTextBox(6);
                    break;
                case Condition_Type.Quest:
                    mult = 4;

                    AddLabel((string)TryFindResource("conditionEditor_LogicType"));
                    AddLogicBox();

                    AddLabel((string)TryFindResource("conditionEditor_ID"));
                    AddTextBox(5);

                    AddLabel((string)TryFindResource("conditionEditor_Status"));
                    AddComboBox(Enum.GetValues(typeof(Quest_Status)).Cast<Quest_Status>(), "QuestStatus_{0}");

                    AddResetLabelAndCheckbox("Quest");
                    break;
                case Condition_Type.Reputation:
                    mult = 2;

                    AddLabel((string)TryFindResource("conditionEditor_LogicType"));
                    AddLogicBox();

                    AddLabel((string)TryFindResource("conditionEditor_Value"));
                    AddTextBox(6);
                    break;
                case Condition_Type.Skillset:
                    mult = 2;

                    AddLabel((string)TryFindResource("conditionEditor_LogicType"));
                    AddLogicBox();

                    AddLabel((string)TryFindResource("conditionEditor_Skillset"));
                    AddComboBox(Enum.GetValues(typeof(ESkillset)).Cast<ESkillset>(), "Skillset_{0}");
                    break;
                case Condition_Type.Time_Of_Day:
                    mult = 2;

                    AddLabel((string)TryFindResource("conditionEditor_LogicType"));
                    AddLogicBox();

                    AddLabel((string)TryFindResource("conditionEditor_Second"));
                    AddTextBox(10);
                    break;
                default:
                    ClearParameters();
                    break;
            }
            if (viewLocalizationField)
            {
                mult++;
                AddLabel((string)TryFindResource("conditionEditor_Localization"));
                AddTextBox(200);
            }
            DoubleAnimation anim = new DoubleAnimation(Height, (baseHeight + (heightDelta * (mult + (mult > 1 ? 1 : 0)))), new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            BeginAnimation(HeightProperty, anim);
        }
        #region METHODS
        private void ClearParameters()
        {
            variablesGrid.Children.Clear();
            this.Height = baseHeight;
        }

        private void AddResetLabelAndCheckbox(string currentType = "", bool checkState = false)
        {
            string text = (string)TryFindResource($"conditionEditor_Reset_{currentType}_Title");
            string tooltip = (string)TryFindResource($"conditionEditor_Reset_{currentType}_Tooltip");
            if (text != null)
                AddLabel(text, tooltip != null ? tooltip.Replace("`", Environment.NewLine) : text);
            else
                AddLabel((string)TryFindResource("conditionEditor_Reset"));
            AddCheckBox(checkState);
        }

        private void AddLabel(string text, string tooltip = "")
        {
            variablesGrid.Children.Add(new TextBlock() { ToolTip = tooltip ?? null, TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Left, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left, Text = text, Margin = elementMargin, Height = elementHeight });
        }

        private void AddTextBox(int maxLength)
        {
            variablesGrid.Children.Add(new TextBox() { MaxLength = maxLength, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Margin = elementMargin, Width = 100, Height = elementHeight });
        }

        private void AddLogicBox()
        {
            AddComboBox(Enum.GetValues(typeof(Logic_Type)).Cast<Logic_Type>(), "Logic_{0}");
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object[] input = Input;
                switch (Selected_Condition)
                {
                    case Condition_Type.None:
                        return;
                    case Condition_Type.Experience:
                        Experience_Cond c1 = new Experience_Cond
                        {
                            Logic = (Logic_Type)input[0],
                            Value = uint.Parse(input[1].ToString()),
                            Reset = (bool)input[2]
                        };
                        Result = c1;
                        break;
                    case Condition_Type.Flag_Bool:
                        Flag_Bool_Cond c2 = new Flag_Bool_Cond
                        {
                            Id = ushort.Parse(input[0].ToString()),
                            Value = (bool)input[1],
                            AllowUnset = (bool)input[2],
                            Reset = (bool)input[3]
                        };
                        Result = c2;
                        break;
                    case Condition_Type.Flag_Short:
                        Flag_Short_Cond c3 = new Flag_Short_Cond
                        {
                            Id = ushort.Parse(input[0].ToString()),
                            Logic = (Logic_Type)input[1],
                            Value = short.Parse(input[2].ToString()),
                            AllowUnset = (bool)input[3],
                            Reset = (bool)input[4]
                        };
                        Result = c3;
                        break;
                    case Condition_Type.Item:
                        Item_Cond c4 = new Item_Cond
                        {
                            Id = ushort.Parse(input[0].ToString()),
                            Amount = uint.Parse(input[1].ToString()),
                            Reset = (bool)input[2]
                        };
                        Result = c4;
                        break;
                    case Condition_Type.Kills_Animal:
                        Kills_Animal_Cond c5 = new Kills_Animal_Cond
                        {
                            ID = short.Parse(input[0].ToString()),
                            Animal = ushort.Parse(input[1].ToString()),
                            Value = uint.Parse(input[2].ToString()),
                            Reset = (bool)input[3]
                        };
                        Result = c5;
                        break;
                    case Condition_Type.Kills_Horde:
                        Kills_Horde_Cond c6 = new Kills_Horde_Cond
                        {
                            ID = short.Parse(input[0].ToString()),
                            Value = uint.Parse(input[1].ToString()),
                            Navmesh = ushort.Parse(input[2].ToString()),
                            Reset = (bool)input[3]
                        };
                        Result = c6;
                        break;
                    case Condition_Type.Kills_Zombie:
                        Kills_Zombie_Cond c7 = new Kills_Zombie_Cond
                        {
                            Zombie_Type = (Zombie_Type)input[0],
                            Id = ushort.Parse(input[1].ToString()),
                            Amount = uint.Parse(input[2].ToString()),
                            Spawn = (bool)input[3],
                            NavMesh = ushort.Parse(input[4].ToString()),
                            Reset = (bool)input[5]
                        };
                        Result = c7;
                        break;
                    case Condition_Type.Player_Life_Food:
                        Player_Life_Food_Cond c8 = new Player_Life_Food_Cond
                        {
                            Logic = (Logic_Type)input[0],
                            Value = ushort.Parse(input[1].ToString())
                        };
                        Result = c8;
                        break;
                    case Condition_Type.Player_Life_Health:
                        Player_Life_Health_Cond c9 = new Player_Life_Health_Cond
                        {
                            Logic = (Logic_Type)input[0],
                            Value = ushort.Parse(input[1].ToString())
                        };
                        Result = c9;
                        break;
                    case Condition_Type.Player_Life_Virus:
                        Player_Life_Virus_Cond c10 = new Player_Life_Virus_Cond
                        {
                            Logic = (Logic_Type)input[0],
                            Value = ushort.Parse(input[1].ToString())
                        };
                        Result = c10;
                        break;
                    case Condition_Type.Player_Life_Water:
                        Player_Life_Water_Cond c11 = new Player_Life_Water_Cond
                        {
                            Logic = (Logic_Type)input[0],
                            Value = ushort.Parse(input[1].ToString())
                        };
                        Result = c11;
                        break;
                    case Condition_Type.Quest:
                        Quest_Cond c12 = new Quest_Cond
                        {
                            Logic = (Logic_Type)input[0],
                            Id = ushort.Parse(input[1].ToString()),
                            Status = (Quest_Status)input[2],
                            Reset = (bool)input[3]
                        };
                        Result = c12;
                        break;
                    case Condition_Type.Reputation:
                        Reputation_Cond c13 = new Reputation_Cond
                        {
                            Logic = (Logic_Type)input[0],
                            Value = int.Parse(input[1].ToString())
                        };
                        Result = c13;
                        break;
                    case Condition_Type.Skillset:
                        Skillset_Cond c14 = new Skillset_Cond
                        {
                            Logic = (Logic_Type)input[0],
                            Value = (ESkillset)input[1]
                        };
                        Result = c14;
                        break;
                    case Condition_Type.Time_Of_Day:
                        Time_Of_Day_Cond c15 = new Time_Of_Day_Cond
                        {
                            Logic = (Logic_Type)input[0],
                            Second = ulong.Parse(input[1].ToString())
                        };
                        Result = c15;
                        break;
                    case Condition_Type.Kills_Player:
                        Kills_Players_Cond c16 = new Kills_Players_Cond()
                        {
                            ID = ushort.Parse(input[0].ToString()),
                            Value = short.Parse(input[1].ToString()),
                            Reset = (bool)input[2]
                        };
                        Result = c16;
                        break;
                }
                if (viewLocalizationField)
                    Result.Localization = input[input.Length - 1].ToString();
                DialogResult = true;
                Close();
            }
            catch {  } // write some error message or something like that
        }

        private Condition_Type Selected_Condition => (typeBox.SelectedItem as ComboBoxItem).Tag is Condition_Type condition ? condition : Condition_Type.None;

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
            try
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
            catch { }
        }
    }
}
