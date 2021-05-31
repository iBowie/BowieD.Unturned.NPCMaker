using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class VirtualDialogueVendorTabViewModel : BaseViewModel, ITabEditor, INPCTab
    {
        private VirtualDialogueVendor _dialogueVendor;

        public VirtualDialogueVendorTabViewModel()
        {
            MainWindow.Instance.dialogueVendorTabSelect.SelectionChanged += (sender, e) =>
            {
                var tab = MainWindow.Instance.dialogueVendorTabSelect;
                if (tab.SelectedItem != null && tab.SelectedItem is TabItem tabItem && tabItem.DataContext != null)
                {
                    VirtualDialogueVendor selectedTabChar = tabItem.DataContext as VirtualDialogueVendor;
                    if (selectedTabChar != null)
                        DialogueVendor = selectedTabChar;
                }

                if (tab.SelectedItem is null)
                {
                    MainWindow.Instance.dialogueVendorTabGrid.IsEnabled = false;
                    MainWindow.Instance.dialogueVendorTabGrid.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.dialogueVendorTabGridNoSelection.Visibility = Visibility.Visible;
                }
                else
                {
                    MainWindow.Instance.dialogueVendorTabGrid.IsEnabled = true;
                    MainWindow.Instance.dialogueVendorTabGrid.Visibility = Visibility.Visible;
                    MainWindow.Instance.dialogueVendorTabGridNoSelection.Visibility = Visibility.Collapsed;
                }
            };

            MainWindow.Instance.dialogueVendorTabButtonAdd.Command = new BaseCommand(() =>
            {
                VirtualDialogueVendor dv = new VirtualDialogueVendor();
                MainWindow.CurrentProject.data.dialogueVendors.Add(dv);
                MetroTabItem tabItem = CreateTab(dv);
                MainWindow.Instance.dialogueVendorTabSelect.Items.Add(tabItem);
                MainWindow.Instance.dialogueVendorTabSelect.SelectedIndex = MainWindow.Instance.dialogueVendorTabSelect.Items.Count - 1;
            });

            DialogueVendor = new VirtualDialogueVendor();

            UpdateTabs();

            var dialogueInputIdControlContext = new ContextMenu();

            dialogueInputIdControlContext.Items.Add(ContextHelper.CreateFindUnusedIDButton((id) =>
            {
                this.ID = id;
                MainWindow.Instance.dialogueVendorIdTxtBox.Value = id;
            }, GameIntegration.EGameAssetCategory.NPC));

            MainWindow.Instance.dialogueVendorIdTxtBox.ContextMenu = dialogueInputIdControlContext;

            var goodbyeDialogueIDContext = new ContextMenu();

            goodbyeDialogueIDContext.Items.Add(ContextHelper.CreateSelectAssetButton(typeof(GameDialogueAsset), (asset) =>
            {
                this.GoodbyeDialogueID = asset.ID;
                MainWindow.Instance.dialogueVendorGoodbyeDialogueBox.Value = asset.ID;
            }, "Control_SelectAsset_Dialogue", MahApps.Metro.IconPacks.PackIconMaterialKind.Chat));

            MainWindow.Instance.dialogueVendorGoodbyeDialogueBox.ContextMenu = goodbyeDialogueIDContext;

            var boughtDialogueIDContext = new ContextMenu();

            boughtDialogueIDContext.Items.Add(ContextHelper.CreateSelectAssetButton(typeof(GameDialogueAsset), (asset) =>
            {
                this.GoodbyeDialogueID = asset.ID;
                MainWindow.Instance.dialogueVendorBuyingDialogueBox.Value = asset.ID;
            }, "Control_SelectAsset_Dialogue", MahApps.Metro.IconPacks.PackIconMaterialKind.Chat));

            MainWindow.Instance.dialogueVendorBuyingDialogueBox.ContextMenu = boughtDialogueIDContext;

            var soldDialogueIDContext = new ContextMenu();

            soldDialogueIDContext.Items.Add(ContextHelper.CreateSelectAssetButton(typeof(GameDialogueAsset), (asset) =>
            {
                this.GoodbyeDialogueID = asset.ID;
                MainWindow.Instance.dialogueVendorSellingDialogueBox.Value = asset.ID;
            }, "Control_SelectAsset_Dialogue", MahApps.Metro.IconPacks.PackIconMaterialKind.Chat));

            MainWindow.Instance.dialogueVendorSellingDialogueBox.ContextMenu = soldDialogueIDContext;

            ContextMenu cmenu4 = new ContextMenu();

            cmenu4.Items.Add(ContextHelper.CreateSelectAssetButton(typeof(GameCurrencyAsset), (asset) =>
            {
                Currency = asset.GUID.ToString("N");
                MainWindow.Instance.dialogueVendorCurrencyBox.Text = asset.GUID.ToString("N");
            }, "Control_SelectAsset_Currency", MahApps.Metro.IconPacks.PackIconMaterialKind.CurrencyUsd));
            cmenu4.Items.Add(ContextHelper.CreateGenericButton(new AdvancedCommand(() =>
            {
                Currency = string.Empty;
                MainWindow.Instance.dialogueVendorCurrencyBox.Text = string.Empty;
            }, (obj) =>
            {
                return !string.IsNullOrEmpty(Currency);
            }), "Control_Vendor_SwitchToExperience", MahApps.Metro.IconPacks.PackIconMaterialKind.Star));

            MainWindow.Instance.dialogueVendorCurrencyBox.ContextMenu = cmenu4;
        }

        public VirtualDialogueVendor DialogueVendor
        {
            get
            {
                Save();

                return _dialogueVendor;
            }
            set
            {
                Save();

                _dialogueVendor = value;

                MainWindow.Instance.dialogueVendorListBuyItems.Children.Clear();
                MainWindow.Instance.dialogueVendorListSellItems.Children.Clear();
                MainWindow.Instance.dialogueVendorPagesGrid.Children.Clear();

                foreach (var c in value.Items)
                {
                    if (c.isBuy)
                        AddItemBuy(new Universal_ItemList(c, true));
                    else
                        AddItemSell(new Universal_ItemList(c, true));
                }

                foreach (var c in value.Pages)
                {
                    AddPage(c);
                }

                OnPropertyChange("");
            }
        }

        public ushort ID
        {
            get => DialogueVendor.ID;
            set
            {
                DialogueVendor.ID = value;
                OnPropertyChange("ID");
            }
        }
        public string GUID
        {
            get => DialogueVendor.GUID;
            set
            {
                DialogueVendor.GUID = value;
                OnPropertyChange("GUID");
            }
        }
        public string Comment
        {
            get => DialogueVendor.Comment;
            set
            {
                DialogueVendor.Comment = value;
                OnPropertyChange("Comment");
            }
        }
        public string Currency
        {
            get => DialogueVendor.CurrencyGUID;
            set
            {
                DialogueVendor.CurrencyGUID = value;
                OnPropertyChange("Currency");
            }
        }
        public string SellingFormatText
        {
            get => DialogueVendor.SellingFormatText;
            set
            {
                DialogueVendor.SellingFormatText = value;
                OnPropertyChange("SellingFormatText");
            }
        }
        public string BuyingFormatText
        {
            get => DialogueVendor.BuyingFormatText;
            set
            {
                DialogueVendor.BuyingFormatText = value;
                OnPropertyChange("BuyingFormatText");
            }
        }
        public string GoodbyeText
        {
            get => DialogueVendor.GoodbyeText;
            set
            {
                DialogueVendor.GoodbyeText = value;
                OnPropertyChange("GoodbyeText");
            }
        }
        public ushort SoldDialogueID
        {
            get => DialogueVendor.SoldDialogueID;
            set
            {
                DialogueVendor.SoldDialogueID = value;
                OnPropertyChange("SoldDialogueID");
            }
        }
        public ushort BoughtDialogueID
        {
            get => DialogueVendor.BoughtDialogueID;
            set
            {
                DialogueVendor.BoughtDialogueID = value;
                OnPropertyChange("BoughtDialogueID");
            }
        }
        public ushort GoodbyeDialogueID
        {
            get => DialogueVendor.GoodbyeDialogueID;
            set
            {
                DialogueVendor.GoodbyeDialogueID = value;
                OnPropertyChange("GoodbyeDialogueID");
            }
        }

        public void Save()
        {
            if (!(_dialogueVendor is null))
            {
                UpdateItems();

                var tab = MainWindow.Instance.dialogueVendorTabSelect;
                if (tab.SelectedItem != null && tab.SelectedItem is TabItem tabItem && tabItem.DataContext != null)
                {
                    tabItem.DataContext = _dialogueVendor;
                }
            }
        }
        public void Reset() { }

        public void UpdateTabs()
        {
            var tab = MainWindow.Instance.dialogueVendorTabSelect;
            tab.Items.Clear();
            int selected = -1;
            for (int i = 0; i < MainWindow.CurrentProject.data.dialogueVendors.Count; i++)
            {
                var dialogueVendor = MainWindow.CurrentProject.data.dialogueVendors[i];
                if (dialogueVendor == _dialogueVendor)
                    selected = i;
                MetroTabItem tabItem = CreateTab(dialogueVendor);
                tab.Items.Add(tabItem);
            }
            if (selected != -1)
                tab.SelectedIndex = selected;

            if (tab.SelectedItem is null)
            {
                MainWindow.Instance.dialogueVendorTabGrid.IsEnabled = false;
                MainWindow.Instance.dialogueVendorTabGrid.Visibility = Visibility.Collapsed;
                MainWindow.Instance.dialogueVendorTabGridNoSelection.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.dialogueVendorTabGrid.IsEnabled = true;
                MainWindow.Instance.dialogueVendorTabGrid.Visibility = Visibility.Visible;
                MainWindow.Instance.dialogueVendorTabGridNoSelection.Visibility = Visibility.Collapsed;
            }
        }

        private MetroTabItem CreateTab(VirtualDialogueVendor vendor)
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
                    ClipboardManager.SetObject(Universal_ItemList.ReturnType.DialogueVendor, target.DataContext);
                }),
                ContextHelper.CreateDuplicateButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    MetroTabItem target = context.PlacementTarget as MetroTabItem;
                    var cloned = (target.DataContext as VirtualDialogueVendor).Clone();

                    MainWindow.CurrentProject.data.dialogueVendors.Add(cloned);
                    MetroTabItem ti = CreateTab(cloned);
                    MainWindow.Instance.dialogueVendorTabSelect.Items.Add(ti);
                }),
                ContextHelper.CreatePasteButton((object sender, RoutedEventArgs e) =>
                {
                    if (ClipboardManager.TryGetObject(ClipboardManager.DialogueVendorFormat, out var obj) && !(obj is null) && obj is VirtualDialogueVendor cloned)
                    {
                        MainWindow.CurrentProject.data.dialogueVendors.Add(cloned);
                        MetroTabItem ti = CreateTab(cloned);
                        MainWindow.Instance.dialogueVendorTabSelect.Items.Add(ti);
                    }
                })
            };

            foreach (var cmenuItem in cmenuItems)
                cmenu.Items.Add(cmenuItem);

            tabItem.ContextMenu = cmenu;
            return tabItem;
        }

        public void RemoveItemBuy(UIElement element)
        {
            MainWindow.Instance.dialogueVendorListBuyItems.Children.Remove(element);
            UpdateItems();
        }
        public void RemoveItemSell(UIElement element)
        {
            MainWindow.Instance.dialogueVendorListSellItems.Children.Remove(element);
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
                var panel = MainWindow.Instance.dialogueVendorListBuyItems;
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
                _dialogueVendor.Items = new LimitedList<VendorItem>(newItems, byte.MaxValue - 1);

                panel.UpdateOrderButtons();
            };
            item.OnStoppedDrag += () =>
            {
                UpdateItems();
            };
            item.moveUpButton.Click += (sender, e) =>
            {
                MainWindow.Instance.dialogueVendorListBuyItems.MoveUp((sender as UIElement).TryFindParent<Universal_ItemList>());
                UpdateItems();
            };
            item.moveDownButton.Click += (sender, e) =>
            {
                MainWindow.Instance.dialogueVendorListBuyItems.MoveDown((sender as UIElement).TryFindParent<Universal_ItemList>());
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

            MainWindow.Instance.dialogueVendorListBuyItems.Children.Add(item);
            MainWindow.Instance.dialogueVendorListBuyItems.UpdateOrderButtons();
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
                var panel = MainWindow.Instance.dialogueVendorListSellItems;
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
                _dialogueVendor.Items = new LimitedList<VendorItem>(newItems, byte.MaxValue - 1);

                panel.UpdateOrderButtons();
            };
            item.OnStoppedDrag += () =>
            {
                UpdateItems();
            };
            item.moveUpButton.Click += (sender, e) =>
            {
                MainWindow.Instance.dialogueVendorListSellItems.MoveUp((sender as UIElement).TryFindParent<Universal_ItemList>());
                UpdateItems();
            };
            item.moveDownButton.Click += (sender, e) =>
            {
                MainWindow.Instance.dialogueVendorListSellItems.MoveDown((sender as UIElement).TryFindParent<Universal_ItemList>());
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
            MainWindow.Instance.dialogueVendorListSellItems.Children.Add(item);
            MainWindow.Instance.dialogueVendorListSellItems.UpdateOrderButtons();
        }
        public void AddPage(string item)
        {
            Dialogue_Message_Page dmp = new Dialogue_Message_Page(item);
            AddPage(dmp);
        }
        void AddPage(Dialogue_Message_Page dmp)
        {
            dmp.deleteButton.CommandParameter = dmp;
            dmp.deleteButton.Command = new BaseCommand((sender) =>
            {
                var current = sender as Dialogue_Message_Page;
                var panel = MainWindow.Instance.dialogueVendorPagesGrid;
                foreach (var ui in panel.Children)
                {
                    var dr = ui as Dialogue_Message_Page;
                    if (dr.Equals(current))
                    {
                        panel.Children.Remove(dr);
                        break;
                    }
                }
                List<string> newItems = new List<string>();
                foreach (UIElement ui in panel.Children)
                {
                    if (ui is Dialogue_Message_Page dr)
                    {
                        newItems.Add(dr.Page);
                    }
                }
                _dialogueVendor.Pages = new LimitedList<string>(newItems, byte.MaxValue);

                panel.UpdateOrderButtons();
            });

            dmp.OnStoppedDrag += () =>
            {
                UpdateItems();
            };
            dmp.moveUpButton.CommandParameter = dmp;
            dmp.moveUpButton.Command = new BaseCommand((sender) =>
            {
                MainWindow.Instance.dialogueVendorPagesGrid.MoveUp(sender as Dialogue_Message_Page);
                UpdateItems();
            });
            dmp.moveDownButton.CommandParameter = dmp;
            dmp.moveDownButton.Command = new BaseCommand((sender) =>
            {
                MainWindow.Instance.dialogueVendorPagesGrid.MoveDown(sender as Dialogue_Message_Page);
                UpdateItems();
            });

            var cmenu = new ContextMenu();
            List<MenuItem> cmenuItems = new List<MenuItem>()
            {
                ContextHelper.CreateCopyButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    Dialogue_Message_Page target = context.PlacementTarget as Dialogue_Message_Page;
                    ClipboardManager.SetObject(Universal_ItemList.ReturnType.GenericString, target.Page);
                }),
                ContextHelper.CreateDuplicateButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    Dialogue_Message_Page target = context.PlacementTarget as Dialogue_Message_Page;
                    var cloned = target.Page.Clone();

                    AddPage(cloned.ToString());
                }),
                ContextHelper.CreatePasteButton((object sender, RoutedEventArgs e) =>
                {
                    if (ClipboardManager.TryGetObject(ClipboardManager.GenericStringFormat, out var obj) && !(obj is null) && obj is string cloned)
                    {
                        AddPage(cloned);
                    }
                })
            };

            foreach (var cmenuItem in cmenuItems)
                cmenu.Items.Add(cmenuItem);

            dmp.ContextMenu = cmenu;
            MainWindow.Instance.dialogueVendorPagesGrid.Children.Add(dmp);
            MainWindow.Instance.dialogueVendorPagesGrid.UpdateOrderButtons();
        }
        void UpdateItems()
        {
            _dialogueVendor.Items.Clear();
            _dialogueVendor.Pages.Clear();

            foreach (var uie in MainWindow.Instance.dialogueVendorPagesGrid.Children)
            {
                if (uie is Dialogue_Message_Page dmp)
                {
                    _dialogueVendor.Pages.Add(dmp.Page);
                }
            }
            foreach (var uie in MainWindow.Instance.dialogueVendorListBuyItems.Children)
            {
                if (uie is Universal_ItemList dr)
                {
                    _dialogueVendor.Items.Add(dr.Value as VendorItem);
                }
            }
            foreach (var uie in MainWindow.Instance.dialogueVendorListSellItems.Children)
            {
                if (uie is Universal_ItemList dr)
                {
                    _dialogueVendor.Items.Add(dr.Value as VendorItem);
                }
            }

            MainWindow.Instance.dialogueVendorPagesGrid.UpdateOrderButtons();
            MainWindow.Instance.dialogueVendorListBuyItems.UpdateOrderButtons();
            MainWindow.Instance.dialogueVendorListSellItems.UpdateOrderButtons();
        }

        private ICommand _addItemCommand, _addPageCommand, closeTabCommand, _convertToDialogueCommand;
        private ICommand setGuidCommand;
        private ICommand randomGuidCommand;

        public ICommand AddItemCommand
        {
            get
            {
                if (_addItemCommand is null)
                {
                    _addItemCommand = new AdvancedCommand(() =>
                    {
                        Universal_VendorItemEditor uvie = new Universal_VendorItemEditor(DialogueVendor, null);
                        if (uvie.ShowDialog() == true)
                        {
                            VendorItem resultedVendorItem = uvie.Result;
                            Universal_ItemList uil = new Universal_ItemList(resultedVendorItem, true);
                            if (resultedVendorItem.isBuy)
                                AddItemBuy(uil);
                            else
                                AddItemSell(uil);
                            Save();
                        }
                    }, (p) =>
                    {
                        return _dialogueVendor.Items.CanAdd;
                    });
                }

                return _addItemCommand;
            }
        }
        public ICommand AddPageCommand
        {
            get
            {
                if (_addPageCommand is null)
                {
                    _addPageCommand = new AdvancedCommand(() =>
                    {
                        AddPage(string.Empty);
                        Save();
                    }, (p) =>
                    {
                        return _dialogueVendor.Pages.CanAdd;
                    });
                }

                return _addPageCommand;
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
                        MainWindow.CurrentProject.data.dialogueVendors.Remove(tab.DataContext as VirtualDialogueVendor);
                        MainWindow.Instance.dialogueVendorTabSelect.Items.Remove(sender);
                    });
                }
                return closeTabCommand;
            }
        }
        public ICommand ConvertToDialogueCommand
        {
            get
            {
                if (_convertToDialogueCommand is null)
                {
                    _convertToDialogueCommand = new BaseCommand(() =>
                    {
                        var res = MessageBox.Show(LocalizationManager.Current.Interface["Main_Tab_DialogueVendor_ConvertToDialogue_Confirm"], LocalizationManager.Current.Interface["Main_Tab_DialogueVendor_ConvertToDialogue"], MessageBoxButton.YesNo);

                        switch (res)
                        {
                            case MessageBoxResult.Yes:
                                {
                                    var dv = DialogueVendor;

                                    var dial = dv.CreateDialogue();

                                    MainWindow.CurrentProject.data.dialogueVendors.Remove(dv);
                                    MainWindow.CurrentProject.data.dialogues.Add(dial);

                                    MainWindow.Instance.MainWindowViewModel.DialogueVendorTabViewModel.UpdateTabs();
                                    MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.UpdateTabs();
                                }
                                break;
                        }
                    });
                }

                return _convertToDialogueCommand;
            }
        }
        public ICommand RandomGuidCommand
        {
            get
            {
                if (randomGuidCommand == null)
                {
                    randomGuidCommand = new BaseCommand(() =>
                    {
                        GUID = Guid.NewGuid().ToString("N");
                    });
                }
                return randomGuidCommand;
            }
        }
        public ICommand SetGuidCommand
        {
            get
            {
                if (setGuidCommand == null)
                {
                    setGuidCommand = new BaseCommand(() =>
                    {
                        MultiFieldInputView_Dialog mfiv = new MultiFieldInputView_Dialog(new string[1] { GUID });
                        if (mfiv.ShowDialog(new string[1] { LocalizationManager.Current.DialogueVendor["Guid"] }, "") == true)
                        {
                            string res = mfiv.Values[0];
                            if (Guid.TryParse(res, out var newGuid))
                            {
                                GUID = newGuid.ToString("N");
                            }
                            else
                            {
                                MessageBox.Show(LocalizationManager.Current.DialogueVendor["Guid_Invalid"]);
                            }
                        }
                    });
                }
                return setGuidCommand;
            }
        }
    }
}
