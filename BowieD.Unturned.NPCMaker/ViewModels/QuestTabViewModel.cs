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
        public ushort ID { get => Quest.id; set => Quest.id = value; }
        public string Title { get => Quest.title; set => Quest.title = value; }
        public string Description { get => Quest.description; set => Quest.description = value; }
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
                        UpdateConditions();
                        UpdateRewards();
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

        public void UpdateConditions()
        {
            Quest.conditions.Clear();
            foreach (var uie in MainWindow.Instance.listQuestConditions.Children)
            {
                if (uie is Universal_ItemList dr)
                {
                    Quest.conditions.Add(dr.Value as Condition);
                }
            }
            MainWindow.Instance.listQuestConditions.UpdateOrderButtons();
        }
        public void UpdateRewards()
        {
            Quest.rewards.Clear();
            foreach (var uie in MainWindow.Instance.listQuestRewards.Children)
            {
                if (uie is Universal_ItemList dr)
                {
                    Quest.rewards.Add(dr.Value as Reward);
                }
            }
            MainWindow.Instance.listQuestRewards.UpdateOrderButtons();
        }

        void AddCondition(Universal_ItemList uil)
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
                Quest.conditions = newConditions;

                panel.UpdateOrderButtons();
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
            MainWindow.Instance.listQuestConditions.Children.Add(uil);
            MainWindow.Instance.listQuestConditions.UpdateOrderButtons();
        }
        void AddReward(Universal_ItemList uil)
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
                Quest.rewards = newRewards;

                panel.UpdateOrderButtons();
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
            MainWindow.Instance.listQuestRewards.Children.Add(uil);
            MainWindow.Instance.listQuestRewards.UpdateOrderButtons();
        }
    }
}
