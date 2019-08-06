using BowieD.Unturned.NPCMaker.Localization;
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
        public Whats_New()
        {
            InitializeComponent();
            Title = LocUtil.LocalizeInterface("app_News_Title");
            updateTitle.Text = Util.UpdateManager.Title;
            updateText.Text = Util.UpdateManager.Content;
            mainButton.Content = LocUtil.LocalizeInterface("app_News_OK");
            Height = SystemParameters.PrimaryScreenHeight / 2;
            Width = SystemParameters.PrimaryScreenWidth / 2;
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
