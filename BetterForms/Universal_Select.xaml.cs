using System.Windows;
using System.Windows.Controls;
using static BowieD.Unturned.NPCMaker.BetterControls.Universal_ItemList;

namespace BowieD.Unturned.NPCMaker.BetterForms
{
    /// <summary>
    /// Логика взаимодействия для Universal_Select.xaml
    /// </summary>
    public partial class Universal_Select : Window
    {
        public Universal_Select(ReturnType returnType)
        {
            InitializeComponent();
            double scale = Config.Configuration.Properties.scale;
            this.Width *= scale;
            this.Height *= scale;
            this.MinWidth *= scale;
            this.MaxWidth *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
            try
            {
                Title = string.Format((string)TryFindResource("select_Title"), (string)TryFindResource("select_" + returnType.ToString()));
            }
            catch { }
            switch (returnType)
            {
                case ReturnType.Dialogue:
                    foreach (var d in MainWindow.CurrentNPC.dialogues)
                    {
                        Add(d, d.ToString());
                    }
                    break;
                case ReturnType.Quest:
                    foreach (var q in MainWindow.CurrentNPC.quests)
                    {
                        Add(q, q.ToString());
                    }
                    break;
                case ReturnType.Vendor:
                    foreach (var v in MainWindow.CurrentNPC.vendors)
                    {
                        Add(v, v.ToString());
                    }
                    break;
            }
        }

        private void Add(object value, string text)
        {
            Button b = new Button
            {
                Height = 23,
                Content = text,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Tag = value
            };
            b.Click += B_Click;
            stackPanel.Children.Add(b);
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            SelectedValue = (sender as Button).Tag;
            Close();
        }

        public object SelectedValue { get; private set; }
    }
}
