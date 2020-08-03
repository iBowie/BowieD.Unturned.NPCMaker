using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Interaction logic for OptionalUInt16ValueControl.xaml
    /// </summary>
    public partial class OptionalUInt16ValueControl : UserControl
    {
        public OptionalUInt16ValueControl()
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
            ushort? newVal = (ushort?)e.NewValue;
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
