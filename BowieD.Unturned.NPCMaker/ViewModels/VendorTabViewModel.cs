using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class VendorTabViewModel : BaseViewModel, ITabEditor, INPCTab
    {
        private NPCVendor _vendor;
        public VendorTabViewModel()
        {
            MainWindow.Instance.vendorTabSelect.SelectionChanged += VendorTabSelect_SelectionChanged;
            MainWindow.Instance.vendorTabButtonAdd.Click += VendorTabButtonAdd_Click;
            NPCVendor empty = new NPCVendor();
            Vendor = empty;
            UpdateTabs();

            ContextMenu cmenu2 = new ContextMenu();

            cmenu2.Items.Add(ContextHelper.CreateAddFromTemplateButton(typeof(VendorItem), (result) =>
            {
                if (result is VendorItem item)
                {
                    if (item.isBuy)
                        AddItemBuy(item);
                    else
                        AddItemSell(item);
                }
            }));

            MainWindow.Instance.vendorAddItemButton.ContextMenu = cmenu2;

            ContextMenu cmenu3 = new ContextMenu();

            cmenu3.Items.Add(ContextHelper.CreateAddFromTemplateButton(typeof(NPCVendor), (result) =>
            {
                if (result is NPCVendor item)
                {
                    MainWindow.CurrentProject.data.vendors.Add(item);
                    MetroTabItem tabItem = CreateTab(item);
                    MainWindow.Instance.vendorTabSelect.Items.Add(tabItem);
                    MainWindow.Instance.vendorTabSelect.SelectedIndex = MainWindow.Instance.vendorTabSelect.Items.Count - 1;
                }
            }));

            MainWindow.Instance.vendorTitleTxtBox.ContextMenu = ContextHelper.CreateContextMenu(ContextHelper.EContextOption.Group_Rich);
            MainWindow.Instance.vendorDescTxtBox.ContextMenu = ContextHelper.CreateContextMenu(ContextHelper.EContextOption.Group_Rich);
        }
        private void VendorTabButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NPCVendor item = new NPCVendor();
            MainWindow.CurrentProject.data.vendors.Add(item);
            MetroTabItem tabItem = CreateTab(item);
            MainWindow.Instance.vendorTabSelect.Items.Add(tabItem);
            MainWindow.Instance.vendorTabSelect.SelectedIndex = MainWindow.Instance.vendorTabSelect.Items.Count - 1;
        }
        private void VendorTabSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = MainWindow.Instance.vendorTabSelect;
            if (tab.SelectedItem != null && tab.SelectedItem is TabItem tabItem && tabItem.DataContext != null)
            {
                NPCVendor selectedTabChar = tabItem.DataContext as NPCVendor;
                if (selectedTabChar != null)
                    Vendor = selectedTabChar;
            }

            if (tab.SelectedItem is null)
            {
                MainWindow.Instance.vendorTabGrid.IsEnabled = false;
                MainWindow.Instance.vendorTabGrid.Visibility = Visibility.Collapsed;
                MainWindow.Instance.vendorTabGridNoSelection.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.vendorTabGrid.IsEnabled = true;
                MainWindow.Instance.vendorTabGrid.Visibility = Visibility.Visible;
                MainWindow.Instance.vendorTabGridNoSelection.Visibility = Visibility.Collapsed;
            }
        }
        public void Save()
        {
            if (!(_vendor is null))
            {
                UpdateItems();

                var tab = MainWindow.Instance.vendorTabSelect;
                if (tab.SelectedItem != null && tab.SelectedItem is TabItem tabItem && tabItem.DataContext != null)
                {
                    tabItem.DataContext = _vendor;
                }
            }
        }
        public void Reset() { }
        public void UpdateTabs()
        {
            var tab = MainWindow.Instance.vendorTabSelect;
            tab.Items.Clear();
            int selected = -1;
            for (int i = 0; i < MainWindow.CurrentProject.data.vendors.Count; i++)
            {
                var vendor = MainWindow.CurrentProject.data.vendors[i];
                if (vendor == _vendor)
                    selected = i;
                MetroTabItem tabItem = CreateTab(vendor);
                tab.Items.Add(tabItem);
            }
            if (selected != -1)
                tab.SelectedIndex = selected;

            if (tab.SelectedItem is null)
            {
                MainWindow.Instance.vendorTabGrid.IsEnabled = false;
                MainWindow.Instance.vendorTabGrid.Visibility = Visibility.Collapsed;
                MainWindow.Instance.vendorTabGridNoSelection.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.vendorTabGrid.IsEnabled = true;
                MainWindow.Instance.vendorTabGrid.Visibility = Visibility.Visible;
                MainWindow.Instance.vendorTabGridNoSelection.Visibility = Visibility.Collapsed;
            }
        }

        private MetroTabItem CreateTab(NPCVendor vendor)
        {
            MetroTabItem tabItem = new MetroTabItem();
            tabItem.CloseButtonEnabled = true;
            tabItem.CloseTabCommand = CloseTabCommand;
            tabItem.CloseTabCommandParameter = tabItem;
            var binding = new Binding()
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("UIText")
            };
            Label l = new Label();
            l.SetBinding(Label.ContentProperty, binding);
            tabItem.Header = l;
            tabItem.DataContext = vendor;

            var cmenu = new ContextMenu();
            List<MenuItem> cmenuItems = new List<MenuItem>()
            {
                ContextHelper.CreateCopyButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    MetroTabItem target = context.PlacementTarget as MetroTabItem;
                    ClipboardManager.SetObject(Universal_ItemList.ReturnType.Vendor, target.DataContext);
                }),
                ContextHelper.CreateDuplicateButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    MetroTabItem target = context.PlacementTarget as MetroTabItem;
                    var cloned = (target.DataContext as NPCVendor).Clone();

                    MainWindow.CurrentProject.data.vendors.Add(cloned);
                    MetroTabItem ti = CreateTab(cloned);
                    MainWindow.Instance.vendorTabSelect.Items.Add(ti);
                }),
                ContextHelper.CreatePasteButton((object sender, RoutedEventArgs e) =>
                {
                    if (ClipboardManager.TryGetObject(ClipboardManager.VendorFormat, out var obj) && !(obj is null) && obj is NPCVendor cloned)
                    {
                        MainWindow.CurrentProject.data.vendors.Add(cloned);
                        MetroTabItem ti = CreateTab(cloned);
                        MainWindow.Instance.vendorTabSelect.Items.Add(ti);
                    }
                })
            };

            foreach (var cmenuItem in cmenuItems)
                cmenu.Items.Add(cmenuItem);

            tabItem.ContextMenu = cmenu;
            return tabItem;
        }

        public NPCVendor Vendor
        {
            get
            {
                Save();

                return _vendor;
            }
            set
            {
                Save();

                _vendor = value;

                MainWindow.Instance.vendorListBuyItems.Children.Clear();
                MainWindow.Instance.vendorListSellItems.Children.Clear();
                foreach (var c in value.items)
                {
                    if (c.isBuy)
                        AddItemBuy(new Universal_ItemList(c, true));
                    else
                        AddItemSell(new Universal_ItemList(c, true));
                }

                OnPropertyChange("");
            }
        }
        public string Comment { get => Vendor.Comment; set => Vendor.Comment = value; }
        public ushort ID { get => Vendor.ID; set => Vendor.ID = value; }
        public string Title { get => Vendor.Title; set => Vendor.Title = value; }
        public string Description { get => Vendor.vendorDescription; set => Vendor.vendorDescription = value; }
        public bool DisableSorting { get => Vendor.disableSorting; set => Vendor.disableSorting = value; }
        public string Currency
        {
            get => Vendor.currency;
            set => Vendor.currency = value;
        }
        private ICommand addItemCommand, previewCommand;
        private ICommand closeTabCommand;
        private ICommand sortIDA, sortIDD, sortTitleA, sortTitleD;
        public ICommand SortIDAscending
        {
            get
            {
                if (sortIDA == null)
                {
                    sortIDA = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.vendors = MainWindow.CurrentProject.data.vendors.OrderBy(d => d.ID).ToList();
                        UpdateTabs();
                    });
                }
                return sortIDA;
            }
        }
        public ICommand SortIDDescending
        {
            get
            {
                if (sortIDD == null)
                {
                    sortIDD = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.vendors = MainWindow.CurrentProject.data.vendors.OrderByDescending(d => d.ID).ToList();
                        UpdateTabs();
                    });
                }
                return sortIDD;
            }
        }
        public ICommand SortTitleAscending
        {
            get
            {
                if (sortTitleA == null)
                {
                    sortTitleA = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.vendors = MainWindow.CurrentProject.data.vendors.OrderBy(d => d.Title).ToList();
                        UpdateTabs();
                    });
                }
                return sortTitleA;
            }
        }
        public ICommand SortTitleDescending
        {
            get
            {
                if (sortTitleD == null)
                {
                    sortTitleD = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.vendors = MainWindow.CurrentProject.data.vendors.OrderByDescending(d => d.Title).ToList();
                        UpdateTabs();
                    });
                }
                return sortTitleD;
            }
        }
        public ICommand CloseTabCommand
        {
            get
            {
                if (closeTabCommand == null)
                {
                    closeTabCommand = new BaseCommand((sender) =>
                    {
                        var tab = (sender as MetroTabItem);
                        MainWindow.CurrentProject.data.vendors.Remove(tab.DataContext as NPCVendor);
                        MainWindow.Instance.vendorTabSelect.Items.Remove(sender);
                    });
                }
                return closeTabCommand;
            }
        }
        public ICommand AddItemCommand
        {
            get
            {
                if (addItemCommand == null)
                {
                    addItemCommand = new BaseCommand(() =>
                    {
                        Universal_VendorItemEditor uvie = new Universal_VendorItemEditor();
                        if (uvie.ShowDialog() == true)
                        {
                            VendorItem resultedVendorItem = uvie.Result;
                            Universal_ItemList uil = new Universal_ItemList(resultedVendorItem, true);
                            if (resultedVendorItem.isBuy)
                                AddItemBuy(uil);
                            else
                                AddItemSell(uil);
                        }
                    });
                }
                return addItemCommand;
            }
        }
        public ICommand PreviewCommand
        {
            get
            {
                if (previewCommand == null)
                {
                    previewCommand = new BaseCommand(() =>
                    {
                        Simulation simulation = new Simulation();

                        MessageBox.Show(LocalizationManager.Current.Interface.Translate("Main_Tab_Vendor_Preview_Message"));

                        SimulationView_Window sim = new SimulationView_Window(null, simulation);
                        sim.Owner = MainWindow.Instance;
                        sim.ShowDialog();

                        VendorView_Window dvw = new VendorView_Window(null, simulation, Vendor);
                        dvw.Owner = MainWindow.Instance;
                        dvw.ShowDialog();
                    });
                }
                return previewCommand;
            }
        }

        public void RemoveItemBuy(UIElement element)
        {
            MainWindow.Instance.vendorListBuyItems.Children.Remove(element);
            UpdateItems();
        }
        public void RemoveItemSell(UIElement element)
        {
            MainWindow.Instance.vendorListSellItems.Children.Remove(element);
            UpdateItems();
        }

        public void AddItemBuy(VendorItem item)
        {
            Universal_ItemList uil = new Universal_ItemList(item, true);
            AddItemBuy(uil);
        }
        void AddItemBuy(Universal_ItemList item)
        {
            if (item.Type != Universal_ItemList.ReturnType.VendorItem)
                throw new ArgumentException($"Expected VendorItem, got {item.Type}");

            item.deleteButton.Click += (sender, e) =>
            {
                var current = (sender as UIElement).TryFindParent<Universal_ItemList>();
                var panel = MainWindow.Instance.vendorListBuyItems;
                foreach (var ui in panel.Children)
                {
                    var dr = ui as Universal_ItemList;
                    if (dr.Equals(current))
                    {
                        panel.Children.Remove(dr);
                        break;
                    }
                }
                List<VendorItem> newItems = new List<VendorItem>();
                foreach (UIElement ui in panel.Children)
                {
                    if (ui is Universal_ItemList dr)
                    {
                        newItems.Add(dr.Value as VendorItem);
                    }
                }
                _vendor.items = newItems;

                panel.UpdateOrderButtons();
            };
            item.moveUpButton.Click += (sender, e) =>
            {
                MainWindow.Instance.vendorListBuyItems.MoveUp((sender as UIElement).TryFindParent<Universal_ItemList>());
                UpdateItems();
            };
            item.moveDownButton.Click += (sender, e) =>
            {
                MainWindow.Instance.vendorListBuyItems.MoveDown((sender as UIElement).TryFindParent<Universal_ItemList>());
                UpdateItems();
            };

            var cmenu = new ContextMenu();
            List<MenuItem> cmenuItems = new List<MenuItem>()
            {
                ContextHelper.CreateCopyButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    Universal_ItemList target = context.PlacementTarget as Universal_ItemList;
                    ClipboardManager.SetObject(Universal_ItemList.ReturnType.VendorItem, target.Value);
                }),
                ContextHelper.CreateDuplicateButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    Universal_ItemList target = context.PlacementTarget as Universal_ItemList;
                    var cloned = (target.Value as VendorItem).Clone();

                    switch (cloned.type)
                    {
                        case ItemType.ITEM:
                            cloned.isBuy = true;
                            AddItemBuy(cloned);
                            break;
                    }
                }),
                ContextHelper.CreatePasteButton((object sender, RoutedEventArgs e) =>
                {
                    if (ClipboardManager.TryGetObject(ClipboardManager.VendorItemFormat, out var obj) && !(obj is null) && obj is VendorItem cloned)
                    {
                        switch (cloned.type)
                        {
                            case ItemType.ITEM:
                                cloned.isBuy = true;
                                AddItemBuy(cloned);
                                break;
                        }
                    }
                })
            };

            foreach (var cmenuItem in cmenuItems)
                cmenu.Items.Add(cmenuItem);

            item.ContextMenu = cmenu;

            item.copyButton.Visibility = Visibility.Collapsed;

            MainWindow.Instance.vendorListBuyItems.Children.Add(item);
            MainWindow.Instance.vendorListBuyItems.UpdateOrderButtons();
        }
        public void AddItemSell(VendorItem item)
        {
            Universal_ItemList uil = new Universal_ItemList(item, true);
            AddItemSell(uil);
        }
        void AddItemSell(Universal_ItemList item)
        {
            if (item.Type != Universal_ItemList.ReturnType.VendorItem)
                throw new ArgumentException($"Expected VendorItem, got {item.Type}");

            item.deleteButton.Click += (sender, e) =>
            {
                var current = (sender as UIElement).TryFindParent<Universal_ItemList>();
                var panel = MainWindow.Instance.vendorListSellItems;
                foreach (var ui in panel.Children)
                {
                    var dr = ui as Universal_ItemList;
                    if (dr.Equals(current))
                    {
                        panel.Children.Remove(dr);
                        break;
                    }
                }
                List<VendorItem> newItems = new List<VendorItem>();
                foreach (UIElement ui in panel.Children)
                {
                    if (ui is Universal_ItemList dr)
                    {
                        newItems.Add(dr.Value as VendorItem);
                    }
                }
                _vendor.items = newItems;

                panel.UpdateOrderButtons();
            };
            item.moveUpButton.Click += (sender, e) =>
            {
                MainWindow.Instance.vendorListSellItems.MoveUp((sender as UIElement).TryFindParent<Universal_ItemList>());
                UpdateItems();
            };
            item.moveDownButton.Click += (sender, e) =>
            {
                MainWindow.Instance.vendorListSellItems.MoveDown((sender as UIElement).TryFindParent<Universal_ItemList>());
                UpdateItems();
            };

            item.copyButton.Visibility = Visibility.Collapsed;

            var cmenu = new ContextMenu();
            List<MenuItem> cmenuItems = new List<MenuItem>()
            {
                ContextHelper.CreateCopyButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    Universal_ItemList target = context.PlacementTarget as Universal_ItemList;
                    ClipboardManager.SetObject(Universal_ItemList.ReturnType.VendorItem, target.Value);
                }),
                ContextHelper.CreateDuplicateButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    Universal_ItemList target = context.PlacementTarget as Universal_ItemList;
                    var cloned = (target.Value as VendorItem).Clone();

                    switch (cloned.type)
                    {
                        case ItemType.ITEM:
                        case ItemType.VEHICLE:
                            cloned.isBuy = false;
                            AddItemSell(cloned);
                            break;
                    }
                }),
                ContextHelper.CreatePasteButton((object sender, RoutedEventArgs e) =>
                {
                    if (ClipboardManager.TryGetObject(ClipboardManager.VendorItemFormat, out var obj) && !(obj is null) && obj is VendorItem cloned)
                    {
                        switch (cloned.type)
                        {
                            case ItemType.ITEM:
                            case ItemType.VEHICLE:
                                cloned.isBuy = false;
                                AddItemSell(cloned);
                                break;
                        }
                    }
                })
            };

            foreach (var cmenuItem in cmenuItems)
                cmenu.Items.Add(cmenuItem);

            item.ContextMenu = cmenu;
            MainWindow.Instance.vendorListSellItems.Children.Add(item);
            MainWindow.Instance.vendorListSellItems.UpdateOrderButtons();
        }
        void UpdateItems()
        {
            _vendor.items.Clear();
            foreach (var uie in MainWindow.Instance.vendorListBuyItems.Children)
            {
                if (uie is Universal_ItemList dr)
                {
                    _vendor.items.Add(dr.Value as VendorItem);
                }
            }
            foreach (var uie in MainWindow.Instance.vendorListSellItems.Children)
            {
                if (uie is Universal_ItemList dr)
                {
                    _vendor.items.Add(dr.Value as VendorItem);
                }
            }
            MainWindow.Instance.vendorListBuyItems.UpdateOrderButtons();
            MainWindow.Instance.vendorListSellItems.UpdateOrderButtons();
        }
    }
}
