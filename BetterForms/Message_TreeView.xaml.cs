using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.BetterForms
{
    /// <summary>
    /// Логика взаимодействия для Message_TreeView.xaml
    /// </summary>
    public partial class Message_TreeView : Window
    {
        public Message_TreeView(int[] arr, int count)
        {
            InitializeComponent();
            gridScale.ScaleX = Properties.Settings.Default.scale;
            gridScale.ScaleY = Properties.Settings.Default.scale;
            for (int k = 0; k < count; k++)
            {
                mainTreeView.Items.Add(new CheckBox() { Content = $"[{k+1}]", IsChecked = arr?.Count() == 0 ? true : arr?.Contains(k) });
            }
        }

        public int[] AsIntArray
        {
            get
            {
                int[] arr = new int[mainTreeView.Items.Count];
                arr = arr.Select(d => -1).ToArray();
                for (int k = 0; k < mainTreeView.Items.Count; k++)
                {
                    CheckBox elem = mainTreeView.Items[k] as CheckBox;
                    if (elem.IsChecked == true)
                        arr[k] = k-1;
                }
                arr = arr.Where(d => d >= 0).ToArray();
                return arr;
            }
        }

        public bool SaveApply { get; set; } = false;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveApply = true;
            Close();
        }
    }
}
