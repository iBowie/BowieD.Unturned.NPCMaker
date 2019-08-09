using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using System.Windows;
using System.Windows.Controls;
using static BowieD.Unturned.NPCMaker.Controls.Universal_ItemList;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Universal_Select.xaml
    /// </summary>
    public partial class Universal_Select : Window
    {
        public Universal_Select(ReturnType returnType)
        {
            InitializeComponent();
            double scale = AppConfig.Instance.scale;
            this.Width *= scale;
            this.Height *= scale;
            this.MinWidth *= scale;
            this.MaxWidth *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
            try
            {
                Title = string.Format(LocUtil.LocalizeInterface("select_Title"), LocUtil.LocalizeInterface("select_" + returnType.ToString()));
            }
            catch { }
            switch (returnType)
            {
                case ReturnType.Dialogue:
                    foreach (var d in MainWindow.CurrentProject.data.dialogues)
                    {
                        Add(d, d.DisplayName);
                    }
                    break;
                case ReturnType.Quest:
                    foreach (var q in MainWindow.CurrentProject.data.quests)
                    {
                        Add(q, q.DisplayName);
                    }
                    break;
                case ReturnType.Vendor:
                    foreach (var v in MainWindow.CurrentProject.data.vendors)
                    {
                        Add(v, v.DisplayName);
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
