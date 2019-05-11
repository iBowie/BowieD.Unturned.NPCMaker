using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.BetterForms
{
    /// <summary>
    /// Логика взаимодействия для Universal_RewardEditor.xaml
    /// </summary>
    public partial class Universal_RewardEditor : Window
    {
        public Universal_RewardEditor(Reward reward = null, bool viewLocalization = false)
        {
            InitializeComponent();
            double scale = Config.Configuration.Properties.scale;
            viewLocalizationField = viewLocalization;
            ClearParameters();
            this.Height *= scale;
            this.Width *= scale;
            baseHeight = Height;
            heightDelta *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
            bool _chosen = false;
            int _index = 0;
            foreach (Type t in Reward.GetTypes())
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = LocUtil.LocalizeReward($"Reward_Type_{t.Name}");
                cbi.Tag = t;
                typeBox.Items.Add(cbi);
                if (!_chosen && reward != null && reward.GetType() == t)
                {
                    typeBox.SelectedIndex = _index;
                    _chosen = true;
                    var fieldControls = Util.FindVisualChildren<FrameworkElement>(variablesGrid).
                        Where(d => d.Tag != null && d.Tag.ToString().StartsWith("variable::"));
                    foreach (var fControl in fieldControls)
                    {
                        SetValueToControl(fControl, reward.GetType().GetField(fControl.Tag.ToString().Substring(10)).GetValue(reward));
                    }
                }
                _index++;
            }
            saveButton.IsEnabled = reward != null;
        }

        public Reward Result { get; private set; }

        #region DESIGN VARS
        private readonly double baseHeight = 178;
        private readonly double heightDelta = 35;
        private readonly bool viewLocalizationField = false;
        #endregion
        #region METHODS
        internal void ClearParameters()
        {
            variablesGrid.Children.Clear();
            this.Height = baseHeight;
        }
        #endregion

        private Type _CurrentRewardType = null;
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reward returnReward = Activator.CreateInstance(_CurrentRewardType) as Reward;
                Dictionary<string, object> _values = new Dictionary<string, object>();
                var controls = Util.FindVisualChildren<FrameworkElement>(variablesGrid).Where(d => d.Tag != null && d.Tag.ToString().StartsWith("variable::"));
                foreach (var c in controls)
                {
                    _values.Add(c.Tag.ToString().Substring(10), GetValueFromControl(c));
                }
                foreach (var k in _values)
                {
                    var field = returnReward.GetType().GetField(k.Key);
                    field.SetValue(returnReward, Convert.ChangeType(k.Value, field.FieldType));
                }
                Result = returnReward;
                DialogResult = true;
                Close();
            }
            catch { MessageBox.Show(LocUtil.LocalizeInterface("rewardEditor_Fail")); }
        }
        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveButton.IsEnabled = true;
            if (e.AddedItems.Count == 0)
                return;
            var type = (typeBox.SelectedItem as ComboBoxItem).Tag as Type;
            Reward newCondition = (Reward)Activator.CreateInstance(type);
            _CurrentRewardType = type;
            ClearParameters();
            int mult = type.GetFields().Length;
            foreach (var c in newCondition.GetControls())
            {
                variablesGrid.Children.Add(c);
            }
            if (!viewLocalizationField)
                GetLocalizationControl().Visibility = Visibility.Collapsed;
            double newHeight = (baseHeight + (heightDelta * (mult + (mult > 1 ? 1 : 0))));
            if (Config.Configuration.Properties.animateControls)
            {
                DoubleAnimation anim = new DoubleAnimation(Height, newHeight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                BeginAnimation(HeightProperty, anim);
            }
            else
            {
                Height = newHeight;
            }
        }
        private void SetValueToControl(FrameworkElement element, object value)
        {
            switch (element)
            {
                case MahApps.Metro.Controls.NumericUpDown nud:
                    nud.Value = Convert.ToDouble(value);
                    break;
                case CheckBox c:
                    c.IsChecked = value as bool?;
                    break;
                case TextBox textBox:
                    textBox.Text = value as string;
                    break;
                case ComboBox comboBox:
                    for (int k = 0; k < comboBox.Items.Count; k++)
                    {
                        if ((comboBox.Items[k] as ComboBoxItem).Tag.Equals(value))
                        {
                            comboBox.SelectedIndex = k;
                            break;
                        }
                    }
                    break;
            }
        }
        private object GetValueFromControl(FrameworkElement element)
        {
            switch (element)
            {
                case MahApps.Metro.Controls.NumericUpDown nud:
                    return nud.Value.HasValue ? nud.Value.Value : 0;
                case CheckBox checkBox:
                    return checkBox.IsChecked.HasValue ? checkBox.IsChecked.Value : false;
                case TextBox textBox:
                    return textBox.Text;
                case ComboBox comboBox:
                    return (comboBox.SelectedItem as ComboBoxItem).Tag;
                default:
                    return null;
            }
        }
        private FrameworkElement GetLocalizationControl()
        {
            var control = Util.FindVisualChildren<FrameworkElement>(variablesGrid).First(d => d.Tag != null && d.Tag.ToString() == "variable::Localization");
            return Util.FindParent<Border>(control);
        }
    }
}
