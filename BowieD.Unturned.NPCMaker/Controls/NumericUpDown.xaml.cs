using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
        }

        public int MaxValue { get; set; } = 100;
        public int MinValue { get; set; } = 0;
        public int Value { get => value; set { if (value > MaxValue) value = MaxValue; if (value < MinValue) value = MinValue; txtBox.Text = value.ToString(); this.value = value; } }
        private int value;

        private void Operation_Minus(object sender, RoutedEventArgs e)
        {
            if (Value == MinValue)
                return;
            Value--;
            OnValueChanged?.Invoke(Value);
        }

        private void Operation_Plus(object sender, RoutedEventArgs e)
        {
            if (Value == MaxValue)
                return;
            Value++;
            OnValueChanged?.Invoke(Value);
        }

        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newText = "";
            foreach (char c in txtBox.Text)
            {
                if (c.IsDigit())
                {
                    newText += c;
                }
            }
            Value = int.Parse(newText.Length == 0 ? "0" : newText);
            OnValueChanged?.Invoke(Value);
        }

        public ValueChanged OnValueChanged { get; set; } = null;

        public delegate void ValueChanged(int newValue);

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                Operation_Plus(sender, null);
            else
                Operation_Minus(sender, null);
        }
    }
}
