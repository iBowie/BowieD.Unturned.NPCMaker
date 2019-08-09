using BowieD.Unturned.NPCMaker.Configuration;
using System;
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
            string r = (string)FindResource("about_Text");
            r = r.Replace("%version%", App.Version.ToString());
            r = r.Replace(@"\n", Environment.NewLine);
            mainText.Text = r;
            double scale = AppConfig.Instance.scale;
            this.Height *= scale;
            this.Width *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
        }
    }
}
