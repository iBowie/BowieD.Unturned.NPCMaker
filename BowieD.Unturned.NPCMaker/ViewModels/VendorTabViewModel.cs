using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
        public ushort ID { get => Vendor.id; set => Vendor.id = value; }
        public string Title { get => Vendor.vendorTitle; set => Vendor.vendorTitle = value; }
        public string Description { get => Vendor.vendorDescription; set => Vendor.vendorDescription = value; }
        public bool DisableSorting { get => Vendor.disableSorting; set => Vendor.disableSorting = value; }
        public string Currency
        {
            get => Vendor.currency;
            set => Vendor.currency = value;
        }
        private ICommand addItemCommand, saveCommand, openCommand, resetCommand, previewCommand;
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
                        UpdateItems();
                        if (!MainWindow.CurrentProject.data.vendors.Contains(Vendor))
                        {
                            MainWindow.CurrentProject.data.vendors.Add(Vendor);
                        }

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
                        ulv.Owner = MainWindow.Instance;
                        if (ulv.ShowDialog() == true)
                        {
                            if (!AppConfig.Instance.automaticallySaveBeforeOpening)
                            {
                                var msgRes = MessageBox.Show(LocalizationManager.Current.Interface["Main_Tab_Vendor_Open_Confirm"], "", MessageBoxButton.YesNoCancel);
                                if (msgRes == MessageBoxResult.Yes)
                                    SaveCommand.Execute(null);
                                else if (msgRes != MessageBoxResult.No)
                                    return;
                            }
                            else
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
                        ushort id = ID;
                        Vendor = new NPCVendor();
                        UpdateItems();
                        App.Logger.Log($"Vendor {id} cleared!");
                    });
                }
                return resetCommand;
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
                        SaveCommand.Execute(null);

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
                Vendor.items = newItems;

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
                Vendor.items = newItems;

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
            MainWindow.Instance.vendorListSellItems.Children.Add(item);
            MainWindow.Instance.vendorListSellItems.UpdateOrderButtons();
        }
        void UpdateItems()
        {
            Vendor.items.Clear();
            foreach (var uie in MainWindow.Instance.vendorListBuyItems.Children)
            {
                if (uie is Universal_ItemList dr)
                {
                    Vendor.items.Add(dr.Value as VendorItem);
                }
            }
            foreach (var uie in MainWindow.Instance.vendorListSellItems.Children)
            {
                if (uie is Universal_ItemList dr)
                {
                    Vendor.items.Add(dr.Value as VendorItem);
                }
            }
            MainWindow.Instance.vendorListBuyItems.UpdateOrderButtons();
            MainWindow.Instance.vendorListSellItems.UpdateOrderButtons();
        }
    }
}
