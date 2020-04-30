using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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

            foreach (var i in Items)
                list.Items.Add(i);
        }

        public Simulation Simulation { get; }
        public List<Simulation.Item> Items => Simulation.Items;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        ask:
            MultiFieldInputView_Dialog mfiv = new MultiFieldInputView_Dialog();
            if (mfiv.ShowDialog(new string[3] { LocalizationManager.Current.Simulation["Inventory"]["Item_ID"], LocalizationManager.Current.Simulation["Inventory"]["Item_Amount"], LocalizationManager.Current.Simulation["Inventory"]["Item_Quality"] }, LocalizationManager.Current.Simulation["Inventory"]["Item_Add"]) == true)
            {
                string[] values = mfiv.Values;
                if (ushort.TryParse(values[0], out ushort itemID) && byte.TryParse(values[1], out byte itemAmount) && byte.TryParse(values[2], out byte itemQuality))
                {
                    Simulation.Item i = new Simulation.Item()
                    {
                        ID = itemID,
                        Amount = itemAmount,
                        Quality = itemQuality
                    };
                    Items.Add(i);
                    list.Items.Add(i);
                }
                else
                {
                    goto ask;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        ask:
            MultiFieldInputView_Dialog mfiv = new MultiFieldInputView_Dialog();
            if (mfiv.ShowDialog(new string[3] { LocalizationManager.Current.Simulation["Inventory"]["Item_ID"], LocalizationManager.Current.Simulation["Inventory"]["Item_Remove_Count"], LocalizationManager.Current.Simulation["Inventory"]["Item_Quality"] }, LocalizationManager.Current.Simulation["Inventory"]["Item_Remove"]) == true)
            {
                string[] values = mfiv.Values;
                if (ushort.TryParse(values[0], out ushort itemID) && byte.TryParse(values[1], out byte itemAmount))
                {
                    List<Simulation.Item> found = Items.Where(d => d.ID == itemID).Take(itemAmount).ToList();
                    for (int i = 0; i < found.Count; i++)
                    {
                        Items.Remove(found[i]);
                    }

                    list.Items.Clear();

                    foreach (var i in Items)
                        list.Items.Add(i);
                }
                else
                {
                    goto ask;
                }
            }
        }
    }
}
