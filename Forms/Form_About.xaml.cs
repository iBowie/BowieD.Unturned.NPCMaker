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
using System.Windows.Shapes;

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
            r = r.Replace("%version%", MainWindow.version.ToString());
            r = r.Replace(@"\n", Environment.NewLine);
            mainText.Text = r;
            double scale = Properties.Settings.Default.scale;
            this.Height *= scale;
            this.Width *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
        }
    }
}
