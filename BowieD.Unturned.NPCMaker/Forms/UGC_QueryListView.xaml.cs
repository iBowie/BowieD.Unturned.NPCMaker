using BowieD.Unturned.NPCMaker.ViewModels;
using BowieD.Unturned.NPCMaker.Workshop;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для UGC_QueryListView.xaml
    /// </summary>
    public partial class UGC_QueryListView : Window
    {
        public UGC_QueryListView(IEnumerable<UGC> ugcs)
        {
            InitializeComponent();

            cancelButton.Command = new BaseCommand(() =>
            {
                DialogResult = false;
                Close();
            });

            foreach (var ugc in ugcs)
            {
                Button b = new Button()
                {
                    Margin = new Thickness(5)
                };

                b.Content = $"{ugc.FileID} - {ugc.Name}";

                b.Command = new BaseCommand(() =>
                {
                    Result = ugc;
                    DialogResult = true;
                    Close();
                });

                stackPanel.Children.Add(b);
            }
        }

        public UGC Result { get; private set; }
    }
}
