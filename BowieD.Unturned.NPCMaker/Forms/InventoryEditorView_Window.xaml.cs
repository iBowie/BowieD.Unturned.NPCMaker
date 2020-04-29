using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
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
    /// Interaction logic for InventoryEditorView_Window.xaml
    /// </summary>
    public partial class InventoryEditorView_Window : Window
    {
        public InventoryEditorView_Window(Simulation simulation)
        {
            InitializeComponent();

            DataContext = this;

            Simulation = simulation;
        }

        public Simulation Simulation { get; }
        public List<Simulation.Item> Items => Simulation.Items;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        ask:
            MultiFieldInputView_Dialog mfiv = new MultiFieldInputView_Dialog();
            if (mfiv.ShowDialog(new string[3] { LocalizationManager.Current.Simulation["Inventory"]["Item_ID"], LocalizationManager.Current.Simulation["Inventory"]["Item_Amount"], LocalizationManager.Current.Simulation["Inventory"]["Item_Quality"] }, LocalizationManager.Current.Simulation["Inventory"]["Item_Add"]) == true)
            {
                var values = mfiv.Values;
                if (ushort.TryParse(values[0], out ushort itemID) && byte.TryParse(values[1], out byte itemAmount) && byte.TryParse(values[2], out byte itemQuality))
                {
                    Simulation.Item i = new Simulation.Item()
                    {
                        ID = itemID,
                        Amount = itemAmount,
                        Quality = itemQuality
                    };
                    Items.Add(i);
                    list.ItemsSource = Items;
                }
                else
                    goto ask;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        ask:
            MultiFieldInputView_Dialog mfiv = new MultiFieldInputView_Dialog();
            if (mfiv.ShowDialog(new string[3] { LocalizationManager.Current.Simulation["Inventory"]["Item_ID"], LocalizationManager.Current.Simulation["Inventory"]["Item_Remove_Count"], LocalizationManager.Current.Simulation["Inventory"]["Item_Quality"] }, LocalizationManager.Current.Simulation["Inventory"]["Item_Remove"]) == true)
            {
                var values = mfiv.Values;
                if (ushort.TryParse(values[0], out ushort itemID) && byte.TryParse(values[1], out byte itemAmount))
                {
                    var found = Items.Where(d => d.ID == itemID).Take(itemAmount).ToList();
                    for (int i = 0; i < found.Count; i++)
                        Items.Remove(found[i]);
                    list.ItemsSource = Items;
                }
                else
                    goto ask;
            }
        }
    }
}
