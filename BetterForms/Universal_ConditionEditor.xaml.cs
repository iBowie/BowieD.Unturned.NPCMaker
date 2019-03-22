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
            typeBox.ItemsSource = Condition.ConditionObjects.Select(d => d.Type).Where(d => d != Condition_Type.None).Select(d => new ComboBoxItem() { Content = MainWindow.Localize($"Condition_{d.ToString()}"), Tag = d }).ToList();
            saveButton.IsEnabled = startCondition.Type != Condition_Type.None;
            #region CONDITION INIT
            viewLocalizationField = viewLocalization;
            SelectConditionType(startCondition.Type);
            if (condition != null)
            {
                ClearParameters();
                startCondition.Init(this, startCondition);
                if (viewLocalization)
                {
                    AddLabel(MainWindow.Localize("conditionEditor_Localization"));
                    AddTextBox(200);
                    SetMainValue(variablesGrid.Children.Count - 1, startCondition.Localization);
                }
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
            Condition_Type newSelection = (Condition_Type)(typeBox.SelectedItem as ComboBoxItem).Tag;
            Condition newCondition = (Condition)Activator.CreateInstance(Condition.GetByType(newSelection));
            ClearParameters();
            newCondition.Init(this);
            int mult = newCondition.Elements;
            if (viewLocalizationField)
            {
                mult++;
                AddLabel(MainWindow.Localize("conditionEditor_Localization"));
                AddTextBox(200);
            }
            DoubleAnimation anim = new DoubleAnimation(Height, (baseHeight + (heightDelta * (mult + (mult > 1 ? 1 : 0)))), new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            BeginAnimation(HeightProperty, anim);
        }
        #region METHODS
        internal void ClearParameters()
        {
            variablesGrid.Children.Clear();
            this.Height = baseHeight;
        }
        internal void AddResetLabelAndCheckbox(string currentType = "", bool checkState = false)
        {
            string text = MainWindow.Localize($"conditionEditor_Reset_{currentType}_Title");
            string tooltip = MainWindow.Localize($"conditionEditor_Reset_{currentType}_Tooltip");
            if (text != null)
                AddLabel(text, tooltip != null ? tooltip.Replace("`", Environment.NewLine) : text);
            else
                AddLabel(MainWindow.Localize("conditionEditor_Reset"));
            AddCheckBox(checkState);
        }
        internal void AddLabel(string text, string tooltip = "")
        {
            variablesGrid.Children.Add(new TextBlock() { ToolTip = tooltip ?? null, TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Left, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left, Text = text, Margin = elementMargin, Height = elementHeight });
        }
        internal void AddTextBox(int maxLength)
        {
            variablesGrid.Children.Add(new TextBox() { MaxLength = maxLength, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Margin = elementMargin, Width = 100, Height = elementHeight });
        }
        internal void AddLogicBox()
        {
            AddComboBox(Enum.GetValues(typeof(Logic_Type)).Cast<Logic_Type>(), "Logic_{0}");
        }
        internal void AddComboBox<T>(IEnumerable<T> Items, string translationKeyFormat)
        {
            variablesGrid.Children.Add(new ComboBox() { ItemsSource = Items.Select(d => new ComboBoxItem() { Content = MainWindow.Localize(string.Format(translationKeyFormat, d.ToString())), Tag = d }), HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Margin = elementMargin, Width = 100, Height = elementHeight });
        }
        internal void AddCheckBox(bool checkState)
        {
            variablesGrid.Children.Add(new CheckBox() { IsChecked = checkState, Margin = elementMargin, VerticalContentAlignment = VerticalAlignment.Center, Height = elementHeight, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right });
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object[] input = Input;
                var cond = Condition.ConditionObjects.First(d => d.Type == Selected_Condition).Parse<Condition>(input);
                Result = cond;
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
        internal object GetMainValue(UIElement element)
        {
            if (element is TextBox a)
                return a.Text;
            if (element is CheckBox b)
                return b.IsChecked.Value;
            if (element is ComboBox c && c.SelectedItem is ComboBoxItem cc)
                return cc.Tag;
            return null;
        }
        internal void SetMainValue(int index, object value)
        {
            if (index < 0 || value == null || index > variablesGrid.Children.Count)
                return;
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
