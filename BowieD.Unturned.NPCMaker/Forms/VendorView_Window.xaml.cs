using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Markup;
using BowieD.Unturned.NPCMaker.NPC;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Interaction logic for VendorView_Window.xaml
    /// </summary>
    public partial class VendorView_Window : MetroWindow
    {
        static IMarkup formatter = new RichText();

        private bool isCurrency;

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
                    MinHeight = 64,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                };

                Grid g = new Grid();
                g.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });
                g.ColumnDefinitions.Add(new ColumnDefinition());

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

                Label l2 = new Label()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Content = tb2
                };

                TextBlock tb3 = new TextBlock()
                {
                    FontSize = 9
                };

                Label l3 = new Label()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Content = tb3
                };

                Grid g2 = new Grid();
                g2.RowDefinitions.Add(new RowDefinition());
                g2.RowDefinitions.Add(new RowDefinition());
                g2.RowDefinitions.Add(new RowDefinition());

                g2.Children.Add(l);
                g2.Children.Add(l2);
                g2.Children.Add(l3);

                Grid.SetRow(l2, 0);
                Grid.SetRow(l3, 1);
                Grid.SetRow(l, 2);

                g.Children.Add(g2);

                Grid.SetColumn(g2, 1);

                if (item.type == ItemType.ITEM && GameAssetManager.TryGetAsset<GameItemAsset>(item.id, out var asset))
                {
                    g.Children.Add(new Image()
                    {
                        Source = new BitmapImage(asset.ImagePath),
                        Width = 48,
                        Height = 48,
                        Margin = new Thickness(5)
                    });

                    formatter.Markup(tb2, asset.name);
                    formatter.Markup(tb3, asset.itemDescription);
                }
                else
                {
                    formatter.Markup(tb2, LocalizationManager.Current.Simulation["Vendor"].Translate(nameKey, item.id));
                    tb3.Text = string.Empty;
                }

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
                    b.rewards.ForEach((r) =>
                    {
                        r.Give(Simulation);
                    });

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
                        s.rewards.ForEach((r) =>
                        {
                            r.Give(Simulation);
                        });

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

            if (string.IsNullOrEmpty(vendor.currency))
                isCurrency = false;
            else
                isCurrency = true;

            setupCurrency();

            updateCurrency();
        }

        public NPCVendor Vendor { get; }
        public Simulation Simulation { get; }

        private uint getCurrency()
        {
            if (isCurrency)
            {
                if (!Simulation.Currencies.TryGetValue(Vendor.currency, out uint value))
                {
                    value = 0;
                }

                return value;
            }
            else
            {
                return Simulation.Experience;
            }
        }

        private void changeCurrency(uint delta, bool spend)
        {
            if (!isCurrency) // experience
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

        private void setupCurrency()
        {
            if (isCurrency)
            {
                currencyTextLabel.HorizontalAlignment = HorizontalAlignment.Right;

                if (Guid.TryParse(Vendor.currency, out var curGuid) && GameAssetManager.TryGetAsset<GameCurrencyAsset>(curGuid, out var asset))
                {
                    currencyIcons.Visibility = Visibility.Visible;

                    foreach (var entry in asset.entries.OrderBy(d => d.Value))
                    {
                        Grid eG = new Grid();

                        Image eIcon = new Image()
                        {
                            Width = 32,
                            Height = 32,
                            Margin = new Thickness(2.5)
                        };

                        if (Guid.TryParse(entry.ItemGUID, out var itemGuid) && GameAssetManager.TryGetAsset<GameItemAsset>(itemGuid, out var itemAsset))
                        {
                            eIcon.Source = ThumbnailManager.CreateThumbnail(itemAsset.ImagePath);
                        }

                        Label eL = new Label()
                        {
                            VerticalAlignment = VerticalAlignment.Bottom,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };

                        TextBlock eT = new TextBlock()
                        {
                            TextAlignment = TextAlignment.Center,
                            Text = entry.Value.ToString()
                        };

                        eL.Content = eT;

                        eG.Children.Add(eIcon);
                        eG.Children.Add(eL);

                        currencyIcons.Children.Add(eG);
                    }
                }
            }
            else
            {
                currencyIcons.Visibility = Visibility.Collapsed;
                currencyTextLabel.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }

        private void updateCurrency()
        {
            string translateKey;
            uint value;
            if (!isCurrency) // experience
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

            if (isCurrency && GameAssetManager.TryGetAsset<GameCurrencyAsset>(Guid.Parse(Vendor.currency), out var asset))
            {
                currencyText.Text = string.Format(asset.valueFormat, value);
            }
            else
            {
                formatter.Markup(currencyText, LocalizationManager.Current.Simulation["Vendor"].Translate(translateKey, value));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
