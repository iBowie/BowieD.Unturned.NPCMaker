using MahApps.Metro.Controls;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для PlainTextWindow.xaml
    /// </summary>
    public partial class PlainTextWindow : MetroWindow
    {
        public PlainTextWindow(string title, string text)
        {
            InitializeComponent();

            this.Title = title;
            this.txt.Text = text;
        }
    }
}
