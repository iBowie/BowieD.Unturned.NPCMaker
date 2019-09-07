using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Form_About.xaml
    /// </summary>
    public partial class Form_About : Window
    {
        public Form_About()
        {
            InitializeComponent();
            string aboutText = LocalizationManager.Current.Interface.Translate("App_About", LocalizationManager.Current.Author, App.Version, LocalizationManager.Current.LastUpdate);
            mainText.Text = aboutText;
            double scale = AppConfig.Instance.scale;
            this.Height *= scale;
            this.Width *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
        }
    }
}
