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
            Reward startReward = reward ?? new Reward();
            SelectRewardType(startReward.Type);
            if (reward != null)
            {
                ClearParameters();
                startReward.Init(this, startReward);
                if (viewLocalization)
                {
                    AddLabel(MainWindow.Localize("rewardEditor_Localization"));
                    AddTextBox(200);
                    SetMainValue(variablesGrid.Children.Count-1, reward.Localization);
                }
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
        internal void ClearParameters()
        {
            variablesGrid.Children.Clear();
            this.Height = baseHeight;
        }
        internal void AddLabel(string text)
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
        internal void AddTextBox(int maxLength)
        {
            variablesGrid.Children.Add(new TextBox() { MaxLength = maxLength, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Margin = elementMargin, Width = 100, Height = elementHeight });
        }
        internal void AddComboBox<T>(IEnumerable<T> Items, string translationKeyFormat)
        {
            variablesGrid.Children.Add(new ComboBox() { ItemsSource = Items.Select(d => new ComboBoxItem() { Content = (string)TryFindResource(string.Format(translationKeyFormat, d.ToString())), Tag = d }), HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Margin = elementMargin, Width = 100, Height = elementHeight });
        }
        internal void AddCheckBox(bool checkState)
        {
            variablesGrid.Children.Add(new CheckBox() { IsChecked = checkState, Margin = elementMargin, VerticalContentAlignment = VerticalAlignment.Center, Height = elementHeight, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right });
        }
        #endregion

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object[] input = Input;
                var rew = Reward.RewardObjects.First(d => d.Type == Selected_Reward).Parse<Reward>(input);
                Result = rew;
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
            Reward newReward = (Reward)Activator.CreateInstance(Reward.GetByType(newSelection));
            ClearParameters();
            newReward.Init(this);
            int mult = newReward.Elements;
            if (viewLocalizationField)
            {
                mult++;
                AddLabel(MainWindow.Localize("rewardEditor_Localization"));
                AddTextBox(200);
            }
            DoubleAnimation anim = new DoubleAnimation(Height, (baseHeight + (heightDelta * (mult + (mult > 1 ? 1 : 0)))), new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            BeginAnimation(HeightProperty, anim);
        }

        private void SelectRewardType(RewardType cType)
        {
            if (cType == RewardType.None)
                typeBox.SelectedIndex = -1;
            else
            {
                for (int k = 0; k < typeBox.Items.Count; k++)
                {
                    if ((typeBox.Items[k] as ComboBoxItem).Tag is RewardType ctype && ctype == cType)
                    {
                        typeBox.SelectedIndex = k;
                        return;
                    }
                }
            }
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
