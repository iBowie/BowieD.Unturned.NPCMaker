using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class QuestTabViewModel : BaseViewModel, ITabEditor, INPCTab
    {
        private NPCQuest _quest;
        public QuestTabViewModel()
        {
            MainWindow.Instance.questTabSelect.SelectionChanged += QuestTabSelect_SelectionChanged;
            MainWindow.Instance.questTabButtonAdd.Click += QuestTabButtonAdd_Click;
            NPCQuest empty = new NPCQuest();
            Quest = empty;
            UpdateTabs();

            ContextMenu cmenu = new ContextMenu();

            cmenu.Items.Add(ContextHelper.CreateAddFromTemplateButton(typeof(Condition), (result) =>
            {
                if (result is Condition item)
                {
                    AddCondition(new Universal_ItemList(item, true));
                }
            }));

            MainWindow.Instance.questAddConditionButton.ContextMenu = cmenu;

            ContextMenu cmenu2 = new ContextMenu();

            cmenu2.Items.Add(ContextHelper.CreateAddFromTemplateButton(typeof(Reward), (result) =>
            {
                if (result is Reward item)
                {
                    AddReward(new Universal_ItemList(item, true));
                }
            }));

            MainWindow.Instance.questAddRewardButton.ContextMenu = cmenu2;

            ContextMenu cmenu3 = new ContextMenu();

            cmenu3.Items.Add(ContextHelper.CreateAddFromTemplateButton(typeof(NPCQuest), (result) =>
            {
                if (result is NPCQuest item)
                {
                    MainWindow.CurrentProject.data.quests.Add(item);
                    MetroTabItem tabItem = CreateTab(item);
                    MainWindow.Instance.questTabSelect.Items.Add(tabItem);
                    MainWindow.Instance.questTabSelect.SelectedIndex = MainWindow.Instance.questTabSelect.Items.Count - 1;
                }
            }));

            MainWindow.Instance.questTabButtonAdd.ContextMenu = cmenu3;

            MainWindow.Instance.questTitleBox.ContextMenu = ContextHelper.CreateContextMenu(ContextHelper.EContextOption.Group_Rich | ContextHelper.EContextOption.Group_TextEdit);
            MainWindow.Instance.questDescBox.ContextMenu = ContextHelper.CreateContextMenu(ContextHelper.EContextOption.Group_Rich | ContextHelper.EContextOption.Group_TextEdit);

            var questIdBoxContext = new ContextMenu();

            questIdBoxContext.Items.Add(ContextHelper.CreateFindUnusedIDButton((id) =>
            {
                this.ID = id;
                MainWindow.Instance.questIdBox.Value = id;
            }, GameIntegration.EGameAssetCategory.NPC));

            MainWindow.Instance.questIdBox.ContextMenu = questIdBoxContext;
        }
        private void QuestTabButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NPCQuest item = new NPCQuest();
            MainWindow.CurrentProject.data.quests.Add(item);
            MetroTabItem tabItem = CreateTab(item);
            MainWindow.Instance.questTabSelect.Items.Add(tabItem);
            MainWindow.Instance.questTabSelect.SelectedIndex = MainWindow.Instance.questTabSelect.Items.Count - 1;
        }
        private void QuestTabSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = MainWindow.Instance.questTabSelect;
            if (tab.SelectedItem != null && tab.SelectedItem is TabItem tabItem && tabItem.DataContext != null)
            {
                NPCQuest selectedTabChar = tabItem.DataContext as NPCQuest;
                if (selectedTabChar != null)
                    Quest = selectedTabChar;
            }

            if (tab.SelectedItem is null)
            {
                MainWindow.Instance.questTabGrid.IsEnabled = false;
                MainWindow.Instance.questTabGrid.Visibility = Visibility.Collapsed;
                MainWindow.Instance.questTabGridNoSelection.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.questTabGrid.IsEnabled = true;
                MainWindow.Instance.questTabGrid.Visibility = Visibility.Visible;
                MainWindow.Instance.questTabGridNoSelection.Visibility = Visibility.Collapsed;
            }
        }
        public void Save()
        {
            if (!(_quest is null))
            {
                UpdateConditions();
                UpdateRewards();
            }
        }
        public void Reset() { }
        public void UpdateTabs()
        {
            var tab = MainWindow.Instance.questTabSelect;
            tab.Items.Clear();
            int selected = -1;
            for (int i = 0; i < MainWindow.CurrentProject.data.quests.Count; i++)
            {
                var quest = MainWindow.CurrentProject.data.quests[i];
                if (quest == _quest)
                    selected = i;
                MetroTabItem tabItem = CreateTab(quest);
                tab.Items.Add(tabItem);
            }
            if (selected != -1)
                tab.SelectedIndex = selected;

            if (tab.SelectedItem is null)
            {
                MainWindow.Instance.questTabGrid.IsEnabled = false;
                MainWindow.Instance.questTabGrid.Visibility = Visibility.Collapsed;
                MainWindow.Instance.questTabGridNoSelection.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.questTabGrid.IsEnabled = true;
                MainWindow.Instance.questTabGrid.Visibility = Visibility.Visible;
                MainWindow.Instance.questTabGridNoSelection.Visibility = Visibility.Collapsed;
            }
        }

        private MetroTabItem CreateTab(NPCQuest quest)
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
            tabItem.DataContext = quest;

            var cmenu = new ContextMenu();
            List<MenuItem> cmenuItems = new List<MenuItem>()
            {
                ContextHelper.CreateCopyButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    MetroTabItem target = context.PlacementTarget as MetroTabItem;
                    ClipboardManager.SetObject(Universal_ItemList.ReturnType.Quest, target.DataContext);
                }),
                ContextHelper.CreateDuplicateButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    MetroTabItem target = context.PlacementTarget as MetroTabItem;
                    var cloned = (target.DataContext as NPCQuest).Clone();

                    MainWindow.CurrentProject.data.quests.Add(cloned);
                    MetroTabItem ti = CreateTab(cloned);
                    MainWindow.Instance.questTabSelect.Items.Add(ti);
                }),
                ContextHelper.CreatePasteButton((object sender, RoutedEventArgs e) =>
                {
                    if (ClipboardManager.TryGetObject(ClipboardManager.QuestFormat, out var obj) && !(obj is null) && obj is NPCQuest cloned)
                    {
                        MainWindow.CurrentProject.data.quests.Add(cloned);
                        MetroTabItem ti = CreateTab(cloned);
                        MainWindow.Instance.questTabSelect.Items.Add(ti);
                    }
                })
            };

            foreach (var cmenuItem in cmenuItems)
                cmenu.Items.Add(cmenuItem);

            tabItem.ContextMenu = cmenu;
            return tabItem;
        }

        public NPCQuest Quest
        {
            get
            {
                Save();

                return _quest;
            }

            set
            {
                Save();

                _quest = value;

                MainWindow.Instance.listQuestConditions.Children.Clear();
                foreach (var c in value.conditions)
                    AddCondition(new Universal_ItemList(c, true));

                MainWindow.Instance.listQuestRewards.Children.Clear();
                foreach (var r in value.rewards)
                    AddReward(new Universal_ItemList(r, true));

                OnPropertyChange("");
            }
        }
        public string Comment { get => Quest.Comment; set => Quest.Comment = value; }
        public ushort ID { get => Quest.ID; set => Quest.ID = value; }
        public string Title { get => Quest.Title; set => Quest.Title = value; }
        public string Description { get => Quest.description; set => Quest.description = value; }
        private ICommand
            addConditionCommand,
            addRewardCommand,
            previewCommand;
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
                        MainWindow.CurrentProject.data.quests = MainWindow.CurrentProject.data.quests.OrderBy(d => d.ID).ToList();
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
                        MainWindow.CurrentProject.data.quests = MainWindow.CurrentProject.data.quests.OrderByDescending(d => d.ID).ToList();
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
                        MainWindow.CurrentProject.data.quests = MainWindow.CurrentProject.data.quests.OrderBy(d => d.Title).ToList();
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
                        MainWindow.CurrentProject.data.quests = MainWindow.CurrentProject.data.quests.OrderByDescending(d => d.Title).ToList();
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
                        MainWindow.CurrentProject.data.quests.Remove(tab.DataContext as NPCQuest);
                        MainWindow.Instance.questTabSelect.Items.Remove(sender);
                    });
                }
                return closeTabCommand;
            }
        }
        public ICommand AddConditionCommand
        {
            get
            {
                if (addConditionCommand == null)
                {
                    addConditionCommand = new BaseCommand(() =>
                    {
                        Universal_ConditionEditor uce = new Universal_ConditionEditor(null);
                        if (uce.ShowDialog() == true)
                        {
                            Condition cond = uce.Result;
                            AddCondition(new Universal_ItemList(cond, true));
                        }
                    });
                }
                return addConditionCommand;
            }
        }
        public ICommand AddRewardCommand
        {
            get
            {
                if (addRewardCommand == null)
                {
                    addRewardCommand = new BaseCommand(() =>
                    {
                        Universal_RewardEditor ure = new Universal_RewardEditor(null);
                        if (ure.ShowDialog() == true)
                        {
                            Reward rew = ure.Result;
                            AddReward(new Universal_ItemList(rew, true));
                        }
                    });
                }
                return addRewardCommand;
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

                        QuestView_Window dvw = new QuestView_Window(null, simulation, Quest, QuestView_Window.EMode.PREVIEW);
                        dvw.Owner = MainWindow.Instance;
                        dvw.ShowDialog();
                    });
                }
                return previewCommand;
            }
        }

        public void UpdateConditions()
        {
            _quest.conditions.Clear();
            foreach (var uie in MainWindow.Instance.listQuestConditions.Children)
            {
                if (uie is Universal_ItemList dr)
                {
                    _quest.conditions.Add(dr.Value as Condition);
                }
            }
            MainWindow.Instance.listQuestConditions.UpdateOrderButtons();
        }
        public void UpdateRewards()
        {
            _quest.rewards.Clear();
            foreach (var uie in MainWindow.Instance.listQuestRewards.Children)
            {
                if (uie is Universal_ItemList dr)
                {
                    _quest.rewards.Add(dr.Value as Reward);
                }
            }
            MainWindow.Instance.listQuestRewards.UpdateOrderButtons();
        }

        internal void AddCondition(Universal_ItemList uil)
        {
            if (uil.Type != Universal_ItemList.ReturnType.Condition)
                throw new ArgumentException($"Expected Condition, got {uil.Type}");

            uil.deleteButton.Click += (sender, e) =>
            {
                var current = (sender as UIElement).TryFindParent<Universal_ItemList>();
                var panel = MainWindow.Instance.listQuestConditions;
                foreach (var ui in panel.Children)
                {
                    var dr = ui as Universal_ItemList;
                    if (dr.Equals(current))
                    {
                        panel.Children.Remove(dr);
                        break;
                    }
                }
                List<Condition> newConditions = new List<Condition>();
                foreach (UIElement ui in panel.Children)
                {
                    if (ui is Universal_ItemList dr)
                    {
                        newConditions.Add(dr.Value as Condition);
                    }
                }
                _quest.conditions = newConditions;

                panel.UpdateOrderButtons();
            };
            uil.OnStoppedDrag += () =>
            {
                UpdateConditions();
            };
            uil.moveUpButton.Click += (sender, e) =>
            {
                MainWindow.Instance.listQuestConditions.MoveUp((sender as UIElement).TryFindParent<Universal_ItemList>());
                UpdateConditions();
            };
            uil.moveDownButton.Click += (sender, e) =>
            {
                MainWindow.Instance.listQuestConditions.MoveDown((sender as UIElement).TryFindParent<Universal_ItemList>());
                UpdateConditions();
            };

            uil.copyButton.Visibility = Visibility.Collapsed;

            var cmenu = new ContextMenu();
            List<MenuItem> cmenuItems = new List<MenuItem>()
            {
                ContextHelper.CreateCopyButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    Universal_ItemList target = context.PlacementTarget as Universal_ItemList;
                    ClipboardManager.SetObject(Universal_ItemList.ReturnType.Condition, target.Value);
                }),
                ContextHelper.CreateDuplicateButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    Universal_ItemList target = context.PlacementTarget as Universal_ItemList;
                    var cloned = (target.Value as Condition).Clone();

                    AddCondition(new Universal_ItemList(cloned, true));
                }),
                ContextHelper.CreatePasteButton((object sender, RoutedEventArgs e) =>
                {
                    if (ClipboardManager.TryGetObject(ClipboardManager.ConditionFormat, out var obj) && !(obj is null) && obj is Condition cloned)
                    {
                        AddCondition(new Universal_ItemList(cloned, true));
                    }
                })
            };

            foreach (var cmenuItem in cmenuItems)
                cmenu.Items.Add(cmenuItem);

            uil.ContextMenu = cmenu;

            MainWindow.Instance.listQuestConditions.Children.Add(uil);
            MainWindow.Instance.listQuestConditions.UpdateOrderButtons();
        }
        internal void AddReward(Universal_ItemList uil)
        {
            if (uil.Type != Universal_ItemList.ReturnType.Reward)
                throw new ArgumentException($"Expected Reward, got {uil.Type}");

            uil.deleteButton.Click += (sender, e) =>
            {
                var current = (sender as UIElement).TryFindParent<Universal_ItemList>();
                var panel = MainWindow.Instance.listQuestRewards;
                foreach (var ui in panel.Children)
                {
                    var dr = ui as Universal_ItemList;
                    if (dr.Equals(current))
                    {
                        panel.Children.Remove(dr);
                        break;
                    }
                }
                List<Reward> newRewards = new List<Reward>();
                foreach (UIElement ui in panel.Children)
                {
                    if (ui is Universal_ItemList dr)
                    {
                        newRewards.Add(dr.Value as Reward);
                    }
                }
                _quest.rewards = newRewards;

                panel.UpdateOrderButtons();
            };
            uil.OnStoppedDrag += () =>
            {
                UpdateRewards();
            };
            uil.moveUpButton.Click += (sender, e) =>
            {
                MainWindow.Instance.listQuestRewards.MoveUp((sender as UIElement).TryFindParent<Universal_ItemList>());
                UpdateRewards();
            };
            uil.moveDownButton.Click += (sender, e) =>
            {
                MainWindow.Instance.listQuestRewards.MoveDown((sender as UIElement).TryFindParent<Universal_ItemList>());
                UpdateRewards();
            };

            uil.copyButton.Visibility = Visibility.Collapsed;

            var cmenu = new ContextMenu();
            List<MenuItem> cmenuItems = new List<MenuItem>()
            {
                ContextHelper.CreateCopyButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    Universal_ItemList target = context.PlacementTarget as Universal_ItemList;
                    ClipboardManager.SetObject(Universal_ItemList.ReturnType.Reward, target.Value);
                }),
                ContextHelper.CreateDuplicateButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    Universal_ItemList target = context.PlacementTarget as Universal_ItemList;
                    var cloned = (target.Value as Reward).Clone();

                    AddReward(new Universal_ItemList(cloned, true));
                }),
                ContextHelper.CreatePasteButton((object sender, RoutedEventArgs e) =>
                {
                    if (ClipboardManager.TryGetObject(ClipboardManager.RewardFormat, out var obj) && !(obj is null) && obj is Reward cloned)
                    {
                        AddReward(new Universal_ItemList(cloned, true));
                    }
                })
            };

            foreach (var cmenuItem in cmenuItems)
                cmenu.Items.Add(cmenuItem);

            uil.ContextMenu = cmenu;

            MainWindow.Instance.listQuestRewards.Children.Add(uil);
            MainWindow.Instance.listQuestRewards.UpdateOrderButtons();
        }
    }
}
