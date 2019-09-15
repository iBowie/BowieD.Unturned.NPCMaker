using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Editors
{
    public class VendorEditor : IEditor<NPCVendor>
    {
        public VendorEditor()
        {
            MainWindow.Instance.vendorAddItemButton.Click += (object sender, RoutedEventArgs e) =>
            {
                Universal_VendorItemEditor uvie = new Universal_VendorItemEditor();
                if (uvie.ShowDialog() == true)
                {
                    VendorItem resultedVendorItem = uvie.Result;
                    if (resultedVendorItem.isBuy)
                    {
                        AddItemBuy(resultedVendorItem);
                        App.Logger.LogInfo($"Added item {resultedVendorItem.id} in buy list");
                    }
                    else
                    {
                        AddItemSell(resultedVendorItem);
                        App.Logger.LogInfo($"Added item {resultedVendorItem.id} in sell list");
                    }
                }
            };
            MainWindow.Instance.vendorSaveButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Save();
                SendPresence();
            });
            MainWindow.Instance.vendorOpenButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Open();
                SendPresence();
            });
            MainWindow.Instance.vendorResetButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Reset();
                SendPresence();
            });
        }

        public NPCVendor Current
        {
            get
            {
                List<VendorItem> items = new List<VendorItem>();
                foreach (UIElement ui in MainWindow.Instance.vendorListBuyItems.Children)
                {
                    items.Add((ui as Universal_ItemList).Value as VendorItem);
                }
                foreach (UIElement ui in MainWindow.Instance.vendorListSellItems.Children)
                {
                    items.Add((ui as Universal_ItemList).Value as VendorItem);
                }
                // App.Logger.LogInfo($"Built vendor {MainWindow.Instance.vendorIdTxtBox.Value}");
                return new NPCVendor()
                {
                    items = items,
                    id = (ushort)(MainWindow.Instance.vendorIdTxtBox.Value ?? 0),
                    vendorDescription = MainWindow.Instance.vendorDescTxtBox.Text,
                    vendorTitle = MainWindow.Instance.vendorTitleTxtBox.Text,
                    Comment = MainWindow.Instance.vendor_commentbox.Text,
                    disableSorting = MainWindow.Instance.vendorDisableSortingBox.IsChecked ?? false
                };
            }
            set
            {
                Reset();
                foreach (VendorItem item in value.BuyItems)
                {
                    AddItemBuy(item);
                }
                foreach (VendorItem item in value.SellItems)
                {
                    AddItemSell(item);
                }
                MainWindow.Instance.vendorIdTxtBox.Value = value.id;
                MainWindow.Instance.vendorTitleTxtBox.Text = value.vendorTitle;
                MainWindow.Instance.vendorDescTxtBox.Text = value.vendorDescription;
                MainWindow.Instance.vendor_commentbox.Text = value.Comment;
                MainWindow.Instance.vendorDisableSortingBox.IsChecked = value.disableSorting;
                // App.Logger.LogInfo($"Set vendor {value.id}");
            }
        }

        public void Open()
        {
            Universal_ListView ulv = new Universal_ListView(MainWindow.CurrentProject.data.vendors.OrderBy(d => d.id).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Vendor, false)).ToList(), Universal_ItemList.ReturnType.Vendor);
            if (ulv.ShowDialog() == true)
            {
                Save();
                Current = ulv.SelectedValue as NPCVendor;
                App.Logger.LogInfo($"Opened vendor {MainWindow.Instance.vendorIdTxtBox.Value}");
            }
            MainWindow.CurrentProject.data.vendors = ulv.Values.Cast<NPCVendor>().ToList();
        }
        public void Reset()
        {
            MainWindow.Instance.vendorListBuyItems.Children.Clear();
            MainWindow.Instance.vendorListSellItems.Children.Clear();
            MainWindow.Instance.vendorDescTxtBox.Text = "";
            MainWindow.Instance.vendorTitleTxtBox.Text = "";
            MainWindow.Instance.vendorDisableSortingBox.IsChecked = false;
            App.Logger.LogInfo($"Vendor {MainWindow.Instance.vendorIdTxtBox.Value} cleared!");
        }
        public void Save()
        {
            NPCVendor cur = Current;
            if (cur.id == 0)
            {
                App.NotificationManager.Notify(LocalizationManager.Current.Notification["Vendor_ID_Zero"]);
                return;
            }
            if (MainWindow.CurrentProject.data.vendors.Where(d => d.id == cur.id).Count() > 0)
            {
                MainWindow.CurrentProject.data.vendors.Remove(MainWindow.CurrentProject.data.vendors.Where(d => d.id == cur.id).ElementAt(0));
            }
            MainWindow.CurrentProject.data.vendors.Add(cur);
            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Vendor_Saved"]);
            MainWindow.CurrentProject.isSaved = false;
            App.Logger.LogInfo($"Vendor {cur.id} saved!");
        }

        public void AddItemBuy(VendorItem item)
        {
            Universal_ItemList uil = new Universal_ItemList(item, Universal_ItemList.ReturnType.VendorItem, false, true)
            {
                Width = 240
            };
            uil.deleteButton.Click += (object sender, RoutedEventArgs e) =>
            {
                RemoveItemBuy(Util.FindParent<Universal_ItemList>(sender as Button));
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
                }
            };
            MainWindow.Instance.vendorListBuyItems.Children.Add(uil);
        }
        public void AddItemSell(VendorItem item)
        {
            Universal_ItemList uil = new Universal_ItemList(item, Universal_ItemList.ReturnType.VendorItem, false, true)
            {
                Width = 240
            };
            uil.deleteButton.Click += (object sender, RoutedEventArgs e) =>
            {
                RemoveItemSell(Util.FindParent<Universal_ItemList>(sender as Button));
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
                }
            };
            MainWindow.Instance.vendorListSellItems.Children.Add(uil);
        }
        public void RemoveItemSell(UIElement item)
        {
            MainWindow.Instance.vendorListSellItems.Children.Remove(item);
        }
        public void RemoveItemBuy(UIElement item)
        {
            MainWindow.Instance.vendorListBuyItems.Children.Remove(item);
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
        public void SendPresence()
        {
            var current = MainWindow.VendorEditor.Current;
            RichPresence presence = new RichPresence();
            presence.Timestamps = new Timestamps();
            presence.Timestamps.StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            presence.Assets = new Assets();
            presence.Assets.SmallImageKey = "icon_money_outlined";
            presence.Assets.SmallImageText = $"Vendors: {MainWindow.CurrentProject.data.vendors.Count}";
            presence.Details = $"Vendor Name: {(current == null ? "Untitled" : current.vendorTitle)}";
            presence.State = $"Buy: {(current == null ? 0 : current.BuyItems.Count)} / Sell: {(current == null ? 0 : current.SellItems.Count)}";
            MainWindow.DiscordManager.SendPresence(presence);
        }
    }
}
