using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Markup;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Interaction logic for VendorView_Window.xaml
    /// </summary>
    public partial class VendorView_Window : Window
    {
        static IMarkup formatter = new RichText();

        public VendorView_Window(NPCCharacter character, Simulation simulation, NPCVendor vendor)
        {
            InitializeComponent();

            Vendor = vendor;
            Simulation = simulation;

            formatter.Markup(title, SimulationTool.ReplacePlaceholders(character, simulation, vendor.Title));
            formatter.Markup(desc, SimulationTool.ReplacePlaceholders(character, simulation, vendor.vendorDescription));

            UIElement createElement(VendorItem item)
            {
                Button b = new Button()
                {
                    Margin = new Thickness(2.5),
                    Height = 64,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                };

                Grid g = new Grid();

                TextBlock tb = new TextBlock();
                formatter.Markup(tb, LocalizationManager.Current.Simulation["Vendor"].Translate("Item_Cost", item.cost));

                Label l = new Label()
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Content = tb
                };

                string nameKey;

                switch (item.type)
                {
                    case ItemType.ITEM:
                        nameKey = "Item_Name";
                        break;
                    case ItemType.VEHICLE:
                        nameKey = "Vehicle_Name";
                        break;
                    default:
                        throw new Exception("Invalid ItemType");
                }

                TextBlock tb2 = new TextBlock();
                formatter.Markup(tb2, LocalizationManager.Current.Simulation["Vendor"].Translate(nameKey, item.id));

                Label l2 = new Label()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Content = tb2
                };

                g.Children.Add(l);
                g.Children.Add(l2);

                b.Content = g;

                return b;
            }

            System.Collections.Generic.IEnumerable<VendorItem> buyItems = vendor.BuyItems.Where(d => d.conditions.All(c => c.Check(simulation)));
            System.Collections.Generic.IEnumerable<VendorItem> sellItems = vendor.SellItems.Where(d => d.conditions.All(c => c.Check(simulation)));

            if (!vendor.disableSorting) // enable sorting
            {
                buyItems = buyItems.OrderBy(v => v.id);
                sellItems = sellItems.OrderBy(v => v.id);
            }

            foreach (VendorItem b in buyItems)
            {
                UIElement elem = createElement(b);

                elem.PreviewMouseLeftButtonDown += (sender, e) =>
                {
                    if (Simulation.Items.Any(d => d.ID == b.id))
                    {
                        Simulation.Items.Remove(Simulation.Items.First(d => d.ID == b.id));
                        changeCurrency(b.cost, false);
                    }
                };

                buyingPanel.Children.Add(elem);
            }

            foreach (VendorItem s in sellItems)
            {
                UIElement elem = createElement(s);

                elem.PreviewMouseLeftButtonDown += (sender, e) =>
                {
                    if (getCurrency() >= s.cost)
                    {
                        changeCurrency(s.cost, true);
                        switch (s.type)
                        {
                            case ItemType.ITEM:
                                {
                                    Simulation.Items.Add(new Simulation.Item()
                                    {
                                        ID = s.id,
                                        Amount = 1,
                                        Quality = 100
                                    });
                                }
                                break;
                            case ItemType.VEHICLE:
                                {
                                    MessageBox.Show(LocalizationManager.Current.Simulation["Vendor"].Translate("Vehicle_Spawned", s.id, s.spawnPointID));
                                }
                                break;
                        }
                    }
                };

                sellingPanel.Children.Add(elem);
            }

            updateCurrency();
        }

        public NPCVendor Vendor { get; }
        public Simulation Simulation { get; }

        private uint getCurrency()
        {
            if (string.IsNullOrEmpty(Vendor.currency)) // experience
            {
                return Simulation.Experience;
            }
            else // currency
            {
                if (!Simulation.Currencies.TryGetValue(Vendor.currency, out uint value))
                {
                    value = 0;
                }

                return value;
            }
        }

        private void changeCurrency(uint delta, bool spend)
        {
            if (string.IsNullOrEmpty(Vendor.currency)) // experience
            {
                Simulation.Experience = spend ? Simulation.Experience - delta : Simulation.Experience + delta;
            }
            else // currency
            {
                if (!Simulation.Currencies.TryGetValue(Vendor.currency, out uint value))
                {
                    value = 0;
                }

                Simulation.Currencies[Vendor.currency] = spend ? value - delta : value + delta;
            }
            updateCurrency();
        }

        private void updateCurrency()
        {
            string translateKey;
            uint value;
            if (string.IsNullOrEmpty(Vendor.currency)) // experience
            {
                translateKey = "Pay_Experience";
                value = Simulation.Experience;
            }
            else // currency
            {
                if (!Simulation.Currencies.TryGetValue(Vendor.currency, out value))
                {
                    value = 0;
                }

                translateKey = "Pay_Currency";
            }
            formatter.Markup(currencyText, LocalizationManager.Current.Simulation["Vendor"].Translate(translateKey, value));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
