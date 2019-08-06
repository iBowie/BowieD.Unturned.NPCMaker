using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Media;
using System;

namespace BowieD.Unturned.NPCMaker.BetterControls
{
    /// <summary>
    /// Логика взаимодействия для NumberBox.xaml
    /// </summary>
    public partial class NumberBox : UserControl
    {
        public NumberBox()
        {
            InitializeComponent();
            PropertyChanged += NumberBox_PropertyChanged;
        }

        private void NumberBox_PropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ValueProperty)
                ValueChanged?.Invoke(this, (long)e.NewValue);
        }

        public long MinValue
        {
            get
            {
                return (long)GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }
        public long MaxValue
        {
            get
            {
                return (long)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }
        public long Value
        {
            get
            {
                return (long)GetValue(ValueProperty);
            }
            set
            {
                mainBox.Text = value.ToString();
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(long), typeof(NumberBox), new PropertyMetadata((long)0));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(long), typeof(NumberBox), new PropertyMetadata(long.MaxValue));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(long), typeof(NumberBox), new PropertyMetadata((long)0));

        internal event DependencyPropertyChangedEventHandler PropertyChanged;

        internal event EventHandler<long> ValueChanged;

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
            base.OnPropertyChanged(e);
        }

        private bool IsTextAllowed(string text)
        {
            if (long.TryParse(text, out long result))
            {
                if (result < MinValue)
                {
                    SystemSounds.Beep.Play();
                    return false;
                }
                if (result > MaxValue)
                {
                    SystemSounds.Beep.Play();
                    return false;
                }
                if (text.Length > (MaxValue.ToString().Length > MinValue.ToString().Length ? MaxValue.ToString().Length : MinValue.ToString().Length))
                {
                    SystemSounds.Beep.Play();
                    return false;
                }
                Value = result;
                return true;
            }
            Value = 0;
            SystemSounds.Beep.Play();
            return false;
        }

        private void MainBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = (sender as TextBox).Text + e.Text;
            IsTextAllowed(text);
            e.Handled = false;
        }

        private void MainBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                    return;
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
