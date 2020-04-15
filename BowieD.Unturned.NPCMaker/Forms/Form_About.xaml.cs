using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using System.Windows;
using System.Windows.Media.Animation;

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

            if (AppConfig.Instance.animateControls)
            {
                DoubleAnimation da = new DoubleAnimation(0, 1, new Duration(new System.TimeSpan(0, 0, 1)));
                authorText.BeginAnimation(OpacityProperty, da);
            }

            foreach (var patron in App.Package.Patrons)
                patronsList.Items.Add(patron);
            foreach (var credit in App.Package.Credits)
                creditsList.Items.Add(credit);
        }
    }
}
