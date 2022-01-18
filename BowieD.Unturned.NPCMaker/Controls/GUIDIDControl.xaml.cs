using BowieD.Unturned.NPCMaker.NPC;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для GUIDIDControl.xaml
    /// </summary>
    public partial class GUIDIDControl : UserControl
    {
        public delegate void ValueChangedDelegate(object sender, GUIDIDBridge newValue);

        public event ValueChangedDelegate ValueChanged;

        public GUIDIDControl()
        {
            InitializeComponent();

            mainGrid.DataContext = this;
        }

        public GUIDIDBridge Value
        {
            get => (GUIDIDBridge)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(GUIDIDBridge), typeof(GUIDIDControl), new FrameworkPropertyMetadata(default(GUIDIDBridge), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValuePropertyChangedCallback));

        private static void ValuePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is GUIDIDControl control && e.NewValue is GUIDIDBridge bridge)
            {
                control.txt.TextChanged -= control.txt_TextChanged;
                
                if (bridge.Guid.HasValue)
                {
                    control.txt.Text = bridge.Guid.Value.ToString("N");
                    control.mainGrid.BorderBrush = Brushes.Transparent;
                }
                else if (bridge.ID.HasValue)
                {
                    control.txt.Text = bridge.ID.Value.ToString();
                    control.mainGrid.BorderBrush = Brushes.Transparent;
                }
                else
                {
                    control.txt.Text = string.Empty;
                    control.mainGrid.BorderBrush = Brushes.Red;
                }
                
                control.txt.TextChanged += control.txt_TextChanged;

                control.ValueChanged?.Invoke(control, bridge);
            }
        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            Value = GUIDIDBridge.Parse(txt.Text);
        }
    }
}
