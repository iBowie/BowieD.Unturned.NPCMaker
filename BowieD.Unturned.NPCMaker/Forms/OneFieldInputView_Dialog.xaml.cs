using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Interaction logic for OneFieldInputView_Dialog.xaml
    /// </summary>
    public partial class OneFieldInputView_Dialog : MetroWindow
    {
        public OneFieldInputView_Dialog()
        {
            InitializeComponent();
        }

        [Obsolete("Use other overload of this method", true)]
        public new bool? ShowDialog()
        {
            throw new NotImplementedException();
        }
        public bool? ShowDialog(string message, string caption)
        {
            return ShowDialog(message, caption, null);
        }
        public bool? ShowDialog(string message, string caption, string tooltip)
        {
            Title = caption;
            fieldText1.Text = message;
            textBox1.ToolTip = tooltip;

            return base.ShowDialog();
        }

        public string Value
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
