using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Markup;
using System;
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
            try
            {
                var md = new Markdown();
                md.Markup(updateText, UpdateContent);
            }
            catch (Exception ex)
            {
                App.Logger.LogException("Could not apply markdown to update text.", ex: ex);
                updateText.Inlines.Clear();
                updateText.Text = UpdateContent;
            }
            updateTitle.Text = UpdateTitle;
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
