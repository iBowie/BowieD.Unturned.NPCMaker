using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Forms
{
    public partial class Message_TreeView : Window
    {
        private int _count;
        public Message_TreeView(int[] arr, int count)
        {
            InitializeComponent();
            _count = count;
            Height *= Config.Configuration.Properties.scale;
            Width *= Config.Configuration.Properties.scale;
            for (int k = 0; k < count; k++)
            {
                var box = new CheckBox();
                box.Content = $"[{k + 1}]";
                box.IsChecked = arr?.Count() == 0 || arr?.Length <= k ? true : arr?[k] == 1;
                if (box.IsChecked == null)
                    box.IsChecked = false;
                mainTreeView.Items.Add(box);
            }
        }

        public int[] AsIntArray
        {
            get
            {
                int[] result = new int[_count];
                for (int k = 0; k < mainTreeView.Items.Count; k++)
                {
                    var box = mainTreeView.Items[k] as CheckBox;
                    if (box.IsChecked == true)
                    {
                        result[k] = 1;
                    }
                }
                return result;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
