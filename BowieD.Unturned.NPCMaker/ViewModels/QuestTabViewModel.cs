using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class QuestTabViewModel : BaseViewModel
    {
        private NPCQuest _quest;
        public QuestTabViewModel()
        {
            Quest = new NPCQuest();
        }
        public NPCQuest Quest
        {
            get => _quest;
            set
            {
                _quest = value;
                OnPropertyChange("");
            }
        }
        public string Comment { get => Quest.Comment; set => Quest.Comment = value; }
        public ushort ID { get => Quest.id; set => Quest.id = value; }
        public string Title { get => Quest.title; set => Quest.title = value; }
        public string Description { get => Quest.description; set => Quest.description = value; }
        public List<Condition> Conditions
        {
            get
            {
                List<Condition> res = new List<Condition>();
                foreach (UIElement ui in MainWindow.Instance.listQuestConditions.Children)
                {
                    if (ui is Universal_ItemList uil)
                    {
                        res.Add(uil.Value as Condition);
                    }
                }
                return res;
            }
            set
            {
                Quest.conditions = value;
                UpdateConditions();
            }
        }
        public List<Reward> Rewards
        {
            get
            {
                List<Reward> res = new List<Reward>();
                foreach (UIElement ui in MainWindow.Instance.listQuestRewards.Children)
                {
                    if (ui is Universal_ItemList uil)
                    {
                        res.Add(uil.Value as Reward);
                    }
                }
                return res;
            }
            set
            {
                Quest.rewards = value;
                UpdateRewards();
            }
        }
        private void UpdateConditions()
        {
            MainWindow.Instance.listQuestConditions.Children.Clear();
            foreach (Condition k in Quest.conditions)
            {
                Universal_ItemList uil = new Universal_ItemList(k, Universal_ItemList.ReturnType.Condition, true);
                uil.deleteButton.Click += (object sender, RoutedEventArgs e) =>
                {
                    MainWindow.Instance.listQuestConditions.Children.Remove(Util.FindParent<Universal_ItemList>(sender as Button));
                    Quest.conditions = Conditions;
                    UpdateConditions();
                };
                uil.moveUpButton.Click += (object sender, RoutedEventArgs e) =>
                {
                    StackPanel panel = MainWindow.Instance.listQuestConditions;
                    int index = panel.IndexOf(uil);
                    if (index >= 1)
                    {
                        Universal_ItemList next = panel.Children[index - 1] as Universal_ItemList;
                        panel.Children.RemoveAt(index - 1);
                        panel.Children.Insert(index, next);
                        Quest.conditions = Conditions;
                        UpdateConditions();
                    }
                };
                uil.moveDownButton.Click += (object sender, RoutedEventArgs e) =>
                {
                    StackPanel panel = MainWindow.Instance.listQuestConditions;
                    int index = panel.IndexOf(uil);
                    if (index < panel.Children.Count - 1)
                    {
                        panel.Children.RemoveAt(index);
                        panel.Children.Insert(index + 1, uil);
                        Quest.conditions = Conditions;
                        UpdateConditions();
                    }
                };
                MainWindow.Instance.listQuestConditions.Children.Add(uil);
            }
        }
        private void UpdateRewards()
        {
            MainWindow.Instance.listQuestRewards.Children.Clear();
            foreach (Reward reward in Quest.rewards)
            {
                Universal_ItemList uil = new Universal_ItemList(reward, Universal_ItemList.ReturnType.Reward, true);
                uil.deleteButton.Click += (object sender, RoutedEventArgs e) =>
                {
                    MainWindow.Instance.listQuestRewards.Children.Remove(Util.FindParent<Universal_ItemList>(sender as Button));
                    Quest.rewards = Rewards;
                    UpdateRewards();
                };
                uil.moveUpButton.Click += (object sender, RoutedEventArgs e) =>
                {
                    StackPanel panel = MainWindow.Instance.listQuestRewards;
                    int index = panel.IndexOf(uil);
                    if (index >= 1)
                    {
                        Universal_ItemList next = panel.Children[index - 1] as Universal_ItemList;
                        panel.Children.RemoveAt(index - 1);
                        panel.Children.Insert(index, next);
                        Quest.rewards = Rewards;
                        UpdateRewards();
                    }
                };
                uil.moveDownButton.Click += (object sender, RoutedEventArgs e) =>
                {
                    StackPanel panel = MainWindow.Instance.listQuestRewards;
                    int index = panel.IndexOf(uil);
                    if (index < panel.Children.Count - 1)
                    {
                        panel.Children.RemoveAt(index);
                        panel.Children.Insert(index + 1, uil);
                        Quest.rewards = Rewards;
                        UpdateRewards();
                    }
                };
                MainWindow.Instance.listQuestRewards.Children.Add(uil);
            }
        }
        private ICommand
            saveCommand,
            openCommand,
            resetCommand,
            addConditionCommand,
            addRewardCommand,
            previewCommand;
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
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Quest_ID_Zero"]);
                            return;
                        }
                        if (!MainWindow.CurrentProject.data.quests.Contains(Quest))
                        {
                            MainWindow.CurrentProject.data.quests.Add(Quest);
                        }

                        MainWindow.CurrentProject.isSaved = false;
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["Quest_Saved"]);
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
                        Universal_ListView ulv = new Universal_ListView(MainWindow.CurrentProject.data.quests.OrderBy(d => d.id).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Quest, false)).ToList(), Universal_ItemList.ReturnType.Quest);
                        ulv.Owner = MainWindow.Instance;
                        if (ulv.ShowDialog() == true)
                        {
                            SaveCommand.Execute(null);
                            Quest = ulv.SelectedValue as NPCQuest;
                            UpdateConditions();
                            UpdateRewards();
                        }
                        MainWindow.CurrentProject.data.quests = ulv.Values.Cast<NPCQuest>().ToList();
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
                        Quest = new NPCQuest();
                        UpdateConditions();
                        UpdateRewards();
                        App.Logger.Log($"Quest {id} cleared!");
                    });
                }
                return resetCommand;
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
                            AddCondition(cond);
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
                            AddReward(rew);
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
                        SaveCommand.Execute(null);

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
        private void AddReward(Reward reward)
        {
            Universal_ItemList uil = new Universal_ItemList(reward, Universal_ItemList.ReturnType.Reward, true);
            uil.deleteButton.Click += (object sender, RoutedEventArgs e) =>
            {
                MainWindow.Instance.listQuestRewards.Children.Remove(Util.FindParent<Universal_ItemList>(sender as Button));
                Quest.rewards = Rewards;
                UpdateRewards();
            };
            MainWindow.Instance.listQuestRewards.Children.Add(uil);
            Quest.rewards = Rewards;
            UpdateRewards();
        }
        private void AddCondition(Condition condition)
        {
            Universal_ItemList uil = new Universal_ItemList(condition, Universal_ItemList.ReturnType.Condition, true);
            uil.deleteButton.Click += (object sender, RoutedEventArgs e) =>
            {
                MainWindow.Instance.listQuestConditions.Children.Remove(Util.FindParent<Universal_ItemList>(sender as Button));
                Quest.conditions = Conditions;
                UpdateConditions();
            };
            MainWindow.Instance.listQuestConditions.Children.Add(uil);
            Quest.conditions = Conditions;
            UpdateConditions();
        }
    }
}
