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
        public Universal_ConditionEditor(Condition condition = null)
        {
            InitializeComponent();
            double scale = AppConfig.Instance.scale;
            ClearParameters();
            Height *= scale;
            Width *= scale;
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
                }
                _index++;
            }
            if (condition != null)
            {
                variablesGrid.DataContext = condition;
            }

            saveButton.IsEnabled = condition != null;
        }

        #region DESIGN VARS
        private readonly double baseHeight = 178;
        private readonly double heightDelta = 35;
        #endregion
        public Condition Result { get; private set; }

        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveButton.IsEnabled = true;
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            Type type = (typeBox.SelectedItem as ComboBoxItem).Tag as Type;
            Condition newCondition = (Condition)Activator.CreateInstance(type);
            _CurrentConditionType = type;
            ClearParameters();
            variablesGrid.DataContext = newCondition;
            System.Collections.Generic.IEnumerable<FrameworkElement> controls = newCondition.GetControls();
            int mult = controls.Count();
            foreach (FrameworkElement c in controls)
            {
                variablesGrid.Children.Add(c);
            }

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
            Height = baseHeight;
        }
        #endregion

        private Type _CurrentConditionType = null;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (variablesGrid.DataContext == null)
                {
                    DialogResult = false;
                }
                else
                {
                    DialogResult = true;
                }

                Result = variablesGrid.DataContext as Condition;
                Close();
            }
            catch
            {
                MessageBox.Show(LocalizationManager.Current.Interface["Editor_Condition_Fail"]);
            } // write some error message or something like that
        }
        private FrameworkElement GetLocalizationControl()
        {
            FrameworkElement control = Util.FindVisualChildren<FrameworkElement>(variablesGrid).First(d => d.Tag != null && d.Tag.ToString() == "variable::Localization");
            return Util.FindParent<Border>(control);
        }
    }
}
