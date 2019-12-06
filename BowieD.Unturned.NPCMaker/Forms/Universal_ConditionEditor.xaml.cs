using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Universal_ConditionEditor.xaml
    /// </summary>
    public partial class Universal_ConditionEditor : Window
    {
        public Universal_ConditionEditor(Condition condition = null, bool viewLocalization = false)
        {
            InitializeComponent();
            double scale = AppConfig.Instance.scale;
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
            foreach (Type t in Condition.GetTypes())
            {
                ComboBoxItem cbi = new ComboBoxItem
                {
                    Content = LocalizationManager.Current.Condition[$"Type{Condition.GetLocalizationKey(t.Name)}"],
                    Tag = t
                };
                typeBox.Items.Add(cbi);
                if (!_chosen && condition != null && condition.GetType() == t)
                {
                    typeBox.SelectedIndex = _index;
                    _chosen = true;
                    //var fieldControls = Util.FindVisualChildren<FrameworkElement>(variablesGrid).
                    //    Where(d => d.Tag != null && d.Tag.ToString().StartsWith("variable::"));
                    //foreach (var fControl in fieldControls)
                    //{
                    //    SetValueToControl(fControl, condition.GetType().GetField(fControl.Tag.ToString().Substring(10)).GetValue(condition));
                    //}
                }
                _index++;
            }
            if (condition != null)
                variablesGrid.DataContext = condition;
            saveButton.IsEnabled = condition != null;
        }

        #region DESIGN VARS
        private readonly double baseHeight = 178;
        private readonly double heightDelta = 35;
        private readonly bool viewLocalizationField = false;
        #endregion
        public Condition Result { get; private set; }

        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveButton.IsEnabled = true;
            if (e.AddedItems.Count == 0)
                return;
            var type = (typeBox.SelectedItem as ComboBoxItem).Tag as Type;
            Condition newCondition = (Condition)Activator.CreateInstance(type);
            _CurrentConditionType = type;
            ClearParameters();
            variablesGrid.DataContext = newCondition;
            var controls = newCondition.GetControls();
            int mult = controls.Count();
            foreach (var c in controls)
            {
                variablesGrid.Children.Add(c);
            }
            if (!viewLocalizationField)
                GetLocalizationControl().Visibility = Visibility.Collapsed;
            double newHeight = (baseHeight + (heightDelta * (mult + (mult > 1 ? 1 : 0))));
            if (AppConfig.Instance.animateControls)
            {
                DoubleAnimation anim = new DoubleAnimation(Height, newHeight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                BeginAnimation(HeightProperty, anim);
            }
            else
            {
                Height = newHeight;
            }
        }
        #region METHODS
        internal void ClearParameters()
        {
            variablesGrid.Children.Clear();
            this.Height = baseHeight;
        }
        #endregion

        private Type _CurrentConditionType = null;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Condition returnCondition = Activator.CreateInstance(_CurrentConditionType) as Condition;
                //Dictionary<string, object> _values = new Dictionary<string, object>();
                //var controls = Util.FindVisualChildren<FrameworkElement>(variablesGrid).Where(d => d.Tag != null && d.Tag.ToString().StartsWith("variable::"));
                //foreach (var c in controls)
                //{
                //    _values.Add(c.Tag.ToString().Substring(10), GetValueFromControl(c));
                //}
                //foreach (var k in _values)
                //{
                //    var field = returnCondition.GetType().GetField(k.Key);
                //    field.SetValue(returnCondition, Convert.ChangeType(k.Value, field.FieldType));
                //}
                if (variablesGrid.DataContext == null)
                    DialogResult = false;
                else
                    DialogResult = true;
                Result = variablesGrid.DataContext as Condition;
                Close();
            }
            catch
            {
                MessageBox.Show(LocalizationManager.Current.Interface["Editor_Condition_Fail"]);
            } // write some error message or something like that
        }

        //private void SetValueToControl(FrameworkElement element, object value)
        //{
        //    switch (element)
        //    {
        //        case MahApps.Metro.Controls.NumericUpDown nud:
        //            nud.Value = Convert.ToDouble(value);
        //            break;
        //        case CheckBox c:
        //            c.IsChecked = value as bool?;
        //            break;
        //        case TextBox textBox:
        //            textBox.Text = value as string;
        //            break;
        //        case ComboBox comboBox:
        //            for (int k = 0; k < comboBox.Items.Count; k++)
        //            {
        //                if ((comboBox.Items[k] as ComboBoxItem).Tag.Equals(value))
        //                {
        //                    comboBox.SelectedIndex = k;
        //                    break;
        //                }
        //            }
        //            break;
        //    }
        //}
        //private object GetValueFromControl(FrameworkElement element)
        //{
        //    switch (element)
        //    {
        //        case MahApps.Metro.Controls.NumericUpDown nud:
        //            return nud.Value ?? 0;
        //        case CheckBox checkBox:
        //            return checkBox.IsChecked ?? false;
        //        case TextBox textBox:
        //            return textBox.Text;
        //        case ComboBox comboBox:
        //            return (comboBox.SelectedItem as ComboBoxItem).Tag;
        //        default:
        //            return null;
        //    }
        //}
        private FrameworkElement GetLocalizationControl()
        {
            var control = Util.FindVisualChildren<FrameworkElement>(variablesGrid).First(d => d.Tag != null && d.Tag.ToString() == "variable::Localization");
            return Util.FindParent<Border>(control);
        }
    }
}
