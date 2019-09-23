using BowieD.Unturned.NPCMaker.Localization;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Whats_New.xaml
    /// </summary>
    public partial class Whats_New : Window
    {
        public static string UpdateTitle = "";
        public static string UpdateContent = "";
        public Whats_New()
        {
            InitializeComponent();
            Title = LocalizationManager.Current.Interface["Update_Notes_Title"];
            updateTitle.Text = UpdateTitle;
            updateText.Text = UpdateContent;
            mainButton.Content = LocalizationManager.Current.Interface["Update_Notes_OK"];
            Height = SystemParameters.PrimaryScreenHeight / 2;
            Width = SystemParameters.PrimaryScreenWidth / 2;
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
