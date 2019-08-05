using System;
using System.Windows;
using System.Windows.Forms;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Whats_New.xaml
    /// </summary>
    public partial class Whats_New : Window
    {
        public Whats_New(string title, string bodyTitle, string mainText, string buttonName)
        {
            InitializeComponent();
            Title = title;
            updateTitle.Text = bodyTitle;
            updateText.Text = mainText.Replace("`", Environment.NewLine);
            mainButton.Content = buttonName;
            Height = SystemParameters.PrimaryScreenHeight / 2;
            Width = SystemParameters.PrimaryScreenWidth / 2;
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
