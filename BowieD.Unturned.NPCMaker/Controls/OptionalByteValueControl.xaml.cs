using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Interaction logic for OptionalByteValueControl.xaml
    /// </summary>
    public partial class OptionalByteValueControl : UserControl
    {
        public OptionalByteValueControl()
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
            byte? newVal = (byte?)e.NewValue;
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
