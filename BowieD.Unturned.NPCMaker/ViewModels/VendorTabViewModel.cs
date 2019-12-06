using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class VendorTabViewModel : BaseViewModel
    {
        private NPCVendor _vendor;
        public VendorTabViewModel()
        {
            Vendor = new NPCVendor();
        }
        public NPCVendor Vendor
        {
            get => _vendor;
            set
            {
                _vendor = value;
                UpdateItems();
                OnPropertyChange("");
            }
        }
        public string Comment { get => Vendor.Comment; set => Vendor.Comment = value; }
        public ushort ID { get => Vendor.id; set => Vendor.id = value; }
        public string Title { get => Vendor.vendorTitle; set => Vendor.vendorTitle = value; }
        public string Description { get => Vendor.vendorDescription; set => Vendor.vendorDescription = value; }
        public bool DisableSorting { get => Vendor.disableSorting; set => Vendor.disableSorting = value; }
        public string Currency
        {
            get => Vendor.currency;
            set => Vendor.currency = value;
        }
        public List<VendorItem> Items
        {
            get
            {
                List<VendorItem> res = new List<VendorItem>();
                foreach (var r in MainWindow.Instance.vendorListBuyItems.Children)
                {
                    if (r is Universal_ItemList uil)
                    {
                        res.Add(uil.Value as VendorItem);
                    }
                }
                foreach (var r in MainWindow.Instance.vendorListSellItems.Children)
                {
                    if (r is Universal_ItemList uil)
                    {
                        res.Add(uil.Value as VendorItem);
                    }
                }
                return res;
            }
            set
            {
                Vendor.items = value;
                UpdateItems();
                OnPropertyChange("Items");
            }
        }
        internal void UpdateItems()
        {
            MainWindow.Instance.vendorListBuyItems.Children.Clear();
            MainWindow.Instance.vendorListSellItems.Children.Clear();
            foreach (var item in Vendor.items)
            {
                if (item.isBuy)
                    AddItemBuy(item);
                else
                    AddItemSell(item);
            }
        }
        internal void AddItemBuy(VendorItem item)
        {
            Universal_ItemList uil = new Universal_ItemList(item, Universal_ItemList.ReturnType.VendorItem, false, true)
            {
                Width = 240
            };
            uil.deleteButton.Click += (object sender, RoutedEventArgs e) =>
            {
                RemoveItemBuy(Util.FindParent<Universal_ItemList>(sender as Button));
                Vendor.items = Items;
                UpdateItems();
            };
            uil.moveUpButton.Click += (object sender, RoutedEventArgs e) =>
            {
                var panel = MainWindow.Instance.vendorListBuyItems;
                int index = GetIndexInBuy(uil);
                if (index >= 1)
                {
                    Universal_ItemList next = panel.Children[index - 1] as Universal_ItemList;
                    panel.Children.RemoveAt(index - 1);
                    panel.Children.Insert(index, next);
                    Vendor.items = Items;
                    UpdateItems();
                }
            };
            uil.moveDownButton.Click += (object sender, RoutedEventArgs e) =>
            {
                var panel = MainWindow.Instance.vendorListBuyItems;
                int index = GetIndexInBuy(uil);
                if (index < panel.Children.Count - 1)
                {
                    panel.Children.RemoveAt(index);
                    panel.Children.Insert(index + 1, uil);
                    Vendor.items = Items;
                    UpdateItems();
                }
            };
            MainWindow.Instance.vendorListBuyItems.Children.Add(uil);
        }
        internal void AddItemSell(VendorItem item)
        {
            Universal_ItemList uil = new Universal_ItemList(item, Universal_ItemList.ReturnType.VendorItem, false, true)
            {
                Width = 240
            };
            uil.deleteButton.Click += (object sender, RoutedEventArgs e) =>
            {
                RemoveItemSell(Util.FindParent<Universal_ItemList>(sender as Button));
                Vendor.items = Items;
                UpdateItems();
            };
            uil.moveUpButton.Click += (object sender, RoutedEventArgs e) =>
            {
                var panel = MainWindow.Instance.vendorListSellItems;
                int index = GetIndexInSell(uil);
                if (index >= 1)
                {
                    Universal_ItemList next = panel.Children[index - 1] as Universal_ItemList;
                    panel.Children.RemoveAt(index - 1);
                    panel.Children.Insert(index, next);
                    Vendor.items = Items;
                    UpdateItems();
                }
            };
            uil.moveDownButton.Click += (object sender, RoutedEventArgs e) =>
            {
                var panel = MainWindow.Instance.vendorListSellItems;
                int index = GetIndexInSell(uil);
                if (index < panel.Children.Count - 1)
                {
                    panel.Children.RemoveAt(index);
                    panel.Children.Insert(index + 1, uil);
                    Vendor.items = Items;
                    UpdateItems();
                }
            };
            MainWindow.Instance.vendorListSellItems.Children.Add(uil);
        }
        internal void RemoveItemSell(UIElement item)
        {
            MainWindow.Instance.vendorListSellItems.Children.Remove(item);
            Vendor.items = Items;
            UpdateItems();
        }
        internal void RemoveItemBuy(UIElement item)
        {
            MainWindow.Instance.vendorListBuyItems.Children.Remove(item);
            Vendor.items = Items;
            UpdateItems();
        }
        private int GetIndexInBuy(UIElement element)
        {
            for (int k = 0; k < MainWindow.Instance.vendorListBuyItems.Children.Count; k++)
            {
                if (MainWindow.Instance.vendorListBuyItems.Children[k] == element)
                    return k;
            }
            return -1;
        }
        private int GetIndexInSell(UIElement element)
        {
            for (int k = 0; k < MainWindow.Instance.vendorListSellItems.Children.Count; k++)
            {
                if (MainWindow.Instance.vendorListSellItems.Children[k] == element)
                    return k;
            }
            return -1;
        }
        private ICommand addItemCommand, saveCommand, openCommand, resetCommand;
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
                            Vendor.items.Add(resultedVendorItem);
                            UpdateItems();
                        }
                    });
                }
                return addItemCommand;
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new BaseCommand(() =>
                    {
                        if (ID == 0)
                        {
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Vendor_ID_Zero"]);
                            return;
                        }
                        if (!MainWindow.CurrentProject.data.vendors.Contains(Vendor))
                            MainWindow.CurrentProject.data.vendors.Add(Vendor);
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["Vendor_Saved"]);
                        MainWindow.CurrentProject.isSaved = false;
                        App.Logger.Log($"Vendor {ID} saved!");
                    });
                }
                return saveCommand;
            }
        }
        public ICommand OpenCommand
        {
            get
            {
                if (openCommand == null)
                {
                    openCommand = new BaseCommand(() =>
                    {
                        Universal_ListView ulv = new Universal_ListView(MainWindow.CurrentProject.data.vendors.OrderBy(d => d.id).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Vendor, false)).ToList(), Universal_ItemList.ReturnType.Vendor);
                        if (ulv.ShowDialog() == true)
                        {
                            SaveCommand.Execute(null);
                            Vendor = ulv.SelectedValue as NPCVendor;
                            UpdateItems();
                            App.Logger.Log($"Opened vendor {ID}");
                        }
                        MainWindow.CurrentProject.data.vendors = ulv.Values.Cast<NPCVendor>().ToList();
                    });
                }
                return openCommand;
            }
        }
        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new BaseCommand(() =>
                    {
                        var id = ID;
                        Vendor = new NPCVendor();
                        UpdateItems();
                        App.Logger.Log($"Vendor {id} cleared!");
                    });
                }
                return resetCommand;
            }
        }
    }
}
