﻿using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Interaction logic for OptionalInt32ValueControl.xaml
    /// </summary>
    public partial class OptionalInt32ValueControl : UserControl
    {
        public OptionalInt32ValueControl()
        {
            InitializeComponent();

            upDown.ValueChanged += UpDown_ValueChanged;
            checkbox.Unchecked += Checkbox_Unchecked;
        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            upDown.Value = null;
        }

        private void UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int? newVal = (int?)e.NewValue;
            if (newVal.HasValue)
            {
                checkbox.IsChecked = true;
            }
            else
            {
                checkbox.IsChecked = false;
            }
        }
    }
}
