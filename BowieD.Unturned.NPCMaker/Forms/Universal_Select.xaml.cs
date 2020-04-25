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
            Width *= scale;
            Height *= scale;
            MinWidth *= scale;
            MaxWidth *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
            try
            {
                //Title = string.Format(LocalizationManager.Current.Interface("select_Title"), LocalizationManager.Current.Interface("select_" + returnType.ToString()));
                Title = LocalizationManager.Current.Interface[$"Select_{returnType}_Title"];
            }
            catch { }
            switch (returnType)
            {
                case ReturnType.Character:
                    {
                        foreach (var c in MainWindow.CurrentProject.data.characters)
                        {
                            Add(c, c.UIText);
                        }
                    }
                    break;
                case ReturnType.Dialogue:
                    foreach (NPC.NPCDialogue d in MainWindow.CurrentProject.data.dialogues)
                    {
                        Add(d, d.UIText);
                    }
                    break;
                case ReturnType.Quest:
                    foreach (NPC.NPCQuest q in MainWindow.CurrentProject.data.quests)
                    {
                        Add(q, q.UIText);
                    }
                    break;
                case ReturnType.Vendor:
                    foreach (NPC.NPCVendor v in MainWindow.CurrentProject.data.vendors)
                    {
                        Add(v, v.UIText);
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
