using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Currency;
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
    public sealed class CurrencyTabViewModel : BaseViewModel, ITabEditor, INPCTab
    {
        private CurrencyAsset _currency;
        public CurrencyTabViewModel()
        {
            MainWindow.Instance.currencyTabSelect.SelectionChanged += CurrencyTabSelect_SelectionChanged;
            MainWindow.Instance.currencyTabButtonAdd.Click += CurrencyTabButtonAdd_Click;
            CurrencyAsset empty = new CurrencyAsset();
            Currency = empty;
            UpdateTabs();
        }

        public CurrencyAsset Currency
        {
            get
            {
                Save();

                return _currency;
            }
            set
            {
                Save();

                _currency = value;

                MainWindow.Instance.currencyListEntries.Children.Clear();

                foreach (var e in value.Entries)
                {
                    AddEntry(e);
                }

                OnPropertyChange("");
            }
        }
        private ICommand addEntryCommand;
        private ICommand closeTabCommand;
        private ICommand sortGUIDA, sortGUIDD;
        private ICommand setGuidCommand;
        private ICommand randomGuidCommand;

        public ICommand SortGUIDAscending
        {
            get
            {
                if (sortGUIDA == null)
                {
                    sortGUIDA = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.currencies = MainWindow.CurrentProject.data.currencies.OrderBy(d => d.GUID).ToList();
                        UpdateTabs();
                    });
                }
                return sortGUIDA;
            }
        }
        public ICommand SortGUIDDescending
        {
            get
            {
                if (sortGUIDD == null)
                {
                    sortGUIDD = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.currencies = MainWindow.CurrentProject.data.currencies.OrderByDescending(d => d.GUID).ToList();
                        UpdateTabs();
                    });
                }
                return sortGUIDD;
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
                        MainWindow.CurrentProject.data.currencies.Remove(tab.DataContext as CurrencyAsset);
                        MainWindow.Instance.currencyTabSelect.Items.Remove(sender);
                    });
                }
                return closeTabCommand;
            }
        }
        public ICommand AddEntryCommand
        {
            get
            {
                if (addEntryCommand == null)
                {
                    addEntryCommand = new BaseCommand(() =>
                    {
                        CurrencyEntryEditor uvie = new CurrencyEntryEditor(new CurrencyEntry());
                        uvie.Owner = MainWindow.Instance;
                        if (uvie.ShowDialog() == true)
                        {
                            CurrencyEntry resultedCurrencyEntry = uvie.Entry;
                            CurrencyEntryControl cec = new CurrencyEntryControl(resultedCurrencyEntry);
                            AddEntry(cec);
                        }
                    });
                }
                return addEntryCommand;
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
                        if (mfiv.ShowDialog(new string[1] { LocalizationManager.Current.Currency["GUID"] }, "") == true)
                        {
                            string res = mfiv.Values[0];
                            if (Guid.TryParse(res, out var newGuid))
                            {
                                GUID = newGuid.ToString("N");
                            }
                            else
                            {
                                MessageBox.Show(LocalizationManager.Current.Currency["Guid_Invalid"]);
                            }
                        }
                    });
                }
                return setGuidCommand;
            }
        }
        private ICommand copyGuidCommand;
        public ICommand CopyGuidCommand
        {
            get
            {
                if (copyGuidCommand == null)
                {
                    copyGuidCommand = new BaseCommand(() =>
                    {
                        ClipboardManager.SetObject(Universal_ItemList.ReturnType.GenericString, GUID);
                    });
                }

                return copyGuidCommand;
            }
        }
        public string GUID
        {
            get => Currency.GUID; set
            {
                Currency.GUID = value;

                OnPropertyChange(nameof(GUID));
            }
        }
        public string ValueFormat
        {
            get => Currency.ValueFormat; set
            {
                Currency.ValueFormat = value;

                foreach (UIElement elem in MainWindow.Instance.currencyListEntries.Children)
                {
                    if (elem is CurrencyEntryControl cec)
                    {
                        cec.UpdateFormat(value);
                    }
                }

                OnPropertyChange(nameof(ValueFormat));
            }
        }

        public void AddEntry(CurrencyEntry entry)
        {
            CurrencyEntryControl cec = new CurrencyEntryControl(entry);
            AddEntry(cec);
        }
        void AddEntry(CurrencyEntryControl cec)
        {
            cec.editButton.Click += (sender, e) =>
            {
                var current = (sender as UIElement).TryFindParent<CurrencyEntryControl>();

                var old = current.Entry.Clone();

                CurrencyEntryEditor cee = new CurrencyEntryEditor(current.Entry);
                cee.Owner = MainWindow.Instance;
                if (cee.ShowDialog() != true)
                {
                    current.Entry = old;
                }
                else
                {
                    current.Entry = cee.Entry;
                }
            };
            cec.deleteButton.Click += (sender, e) =>
            {
                var current = (sender as UIElement).TryFindParent<CurrencyEntryControl>();
                var panel = MainWindow.Instance.currencyListEntries;
                foreach (var ui in panel.Children)
                {
                    var dr = ui as CurrencyEntryControl;
                    if (dr.Equals(current))
                    {
                        panel.Children.Remove(dr);
                        break;
                    }
                }
                List<CurrencyEntry> newItems = new List<CurrencyEntry>();
                foreach (UIElement ui in panel.Children)
                {
                    if (ui is CurrencyEntryControl dr)
                    {
                        newItems.Add(dr.Entry);
                    }
                }
                _currency.Entries = newItems;
            };

            cec.UpdateFormat(_currency.ValueFormat);

            MainWindow.Instance.currencyListEntries.Children.Add(cec);
        }
        private void CurrencyTabButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            CurrencyAsset item = new CurrencyAsset();
            MainWindow.CurrentProject.data.currencies.Add(item);
            MetroTabItem tabItem = CreateTab(item);
            MainWindow.Instance.currencyTabSelect.Items.Add(tabItem);
            MainWindow.Instance.currencyTabSelect.SelectedIndex = MainWindow.Instance.currencyTabSelect.Items.Count - 1;
        }
        private void CurrencyTabSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = MainWindow.Instance.currencyTabSelect;
            if (tab.SelectedItem != null && tab.SelectedItem is TabItem tabItem && tabItem.DataContext != null)
            {
                CurrencyAsset selectedTabChar = tabItem.DataContext as CurrencyAsset;
                if (selectedTabChar != null)
                    Currency = selectedTabChar;
            }

            if (tab.SelectedItem is null)
            {
                MainWindow.Instance.currencyTabGrid.IsEnabled = false;
                MainWindow.Instance.currencyTabGrid.Visibility = Visibility.Collapsed;
                MainWindow.Instance.currencyTabGridNoSelection.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.currencyTabGrid.IsEnabled = true;
                MainWindow.Instance.currencyTabGrid.Visibility = Visibility.Visible;
                MainWindow.Instance.currencyTabGridNoSelection.Visibility = Visibility.Collapsed;
            }
        }
        private MetroTabItem CreateTab(CurrencyAsset asset)
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
            tabItem.DataContext = asset;

            var cmenu = new ContextMenu();
            List<MenuItem> cmenuItems = new List<MenuItem>()
            {
                ContextHelper.CreateCopyButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    MetroTabItem target = context.PlacementTarget as MetroTabItem;
                    ClipboardManager.SetObject(Universal_ItemList.ReturnType.Currency, target.DataContext);
                }),
                ContextHelper.CreateDuplicateButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    MetroTabItem target = context.PlacementTarget as MetroTabItem;
                    var cloned = (target.DataContext as CurrencyAsset).Clone();

                    MainWindow.CurrentProject.data.currencies.Add(cloned);
                    MetroTabItem ti = CreateTab(cloned);
                    MainWindow.Instance.currencyTabSelect.Items.Add(ti);
                }),
                ContextHelper.CreatePasteButton((object sender, RoutedEventArgs e) =>
                {
                    if (ClipboardManager.TryGetObject(ClipboardManager.CurrencyFormat, out var obj) && !(obj is null) && obj is CurrencyAsset cloned)
                    {
                        MainWindow.CurrentProject.data.currencies.Add(cloned);
                        MetroTabItem ti = CreateTab(cloned);
                        MainWindow.Instance.currencyTabSelect.Items.Add(ti);
                    }
                })
            };

            foreach (var cmenuItem in cmenuItems)
                cmenu.Items.Add(cmenuItem);

            tabItem.ContextMenu = cmenu;
            return tabItem;
        }

        public void Save()
        {
            if (!(_currency is null))
            {
                UpdateItems();

                var tab = MainWindow.Instance.currencyTabSelect;
                if (tab.SelectedItem != null && tab.SelectedItem is TabItem tabItem && tabItem.DataContext != null)
                {
                    tabItem.DataContext = _currency;
                }
            }
        }
        public void Reset() { }
        public void UpdateTabs()
        {
            var tab = MainWindow.Instance.currencyTabSelect;
            tab.Items.Clear();
            int selected = -1;
            for (int i = 0; i < MainWindow.CurrentProject.data.currencies.Count; i++)
            {
                var currency = MainWindow.CurrentProject.data.currencies[i];
                if (currency == _currency)
                    selected = i;
                MetroTabItem tabItem = CreateTab(currency);
                tab.Items.Add(tabItem);
            }
            if (selected != -1)
                tab.SelectedIndex = selected;

            if (tab.SelectedItem is null)
            {
                MainWindow.Instance.currencyTabGrid.IsEnabled = false;
                MainWindow.Instance.currencyTabGrid.Visibility = Visibility.Collapsed;
                MainWindow.Instance.currencyTabGridNoSelection.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.currencyTabGrid.IsEnabled = true;
                MainWindow.Instance.currencyTabGrid.Visibility = Visibility.Visible;
                MainWindow.Instance.currencyTabGridNoSelection.Visibility = Visibility.Collapsed;
            }
        }
        void UpdateItems()
        {
            _currency.Entries.Clear();
            foreach (var uie in MainWindow.Instance.currencyListEntries.Children)
            {
                if (uie is CurrencyEntryControl dr)
                {
                    _currency.Entries.Add(dr.Entry);
                }
            }
        }
    }
}
