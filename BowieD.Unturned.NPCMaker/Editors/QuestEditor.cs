using BowieD.Unturned.NPCMaker.BetterControls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using DiscordRPC;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.Editors
{
    public class QuestEditor : IEditor<NPCQuest>
    {
        public QuestEditor()
        {
            MainWindow.Instance.questAddConditionButton.Click += (object sender, RoutedEventArgs e) =>
            {
                Universal_ConditionEditor uce = new Universal_ConditionEditor(null, true);
                if (uce.ShowDialog() == true)
                {
                    Condition cond = uce.Result;
                    AddCondition(cond);
                }
            };
            MainWindow.Instance.questAddRewardButton.Click += (object sender, RoutedEventArgs e) =>
            {
                Universal_RewardEditor ure = new Universal_RewardEditor(null, true);
                if (ure.ShowDialog() == true)
                {
                    Reward rew = ure.Result;
                    AddReward(rew);
                }
            };
            MainWindow.Instance.questSaveButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) => {
                Save();
                SendPresence();
            });
            MainWindow.Instance.questOpenButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Open();
                SendPresence();
            });
            MainWindow.Instance.questResetButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Reset();
                SendPresence();
            });
        }

        public NPCQuest Current
        {
            get
            {
                NPCQuest ret = new NPCQuest();
                foreach (UIElement ui in MainWindow.Instance.listQuestRewards.Children)
                {
                    ret.rewards.Add((ui as Universal_ItemList).Value as Reward);
                }
                foreach (UIElement ui in MainWindow.Instance.listQuestConditions.Children)
                {
                    ret.conditions.Add((ui as Universal_ItemList).Value as Condition);
                }
                ret.title = MainWindow.Instance.questTitleBox.Text;
                ret.description = MainWindow.Instance.questDescBox.Text;
                ret.id = (ushort)(MainWindow.Instance.questIdBox.Value ?? 0);
                ret.Comment = MainWindow.Instance.quest_commentbox.Text;
                return ret;
            }
            set
            {
                Reset();
                foreach (Reward reward in value.rewards)
                {
                    Universal_ItemList uil = new Universal_ItemList(reward, Universal_ItemList.ReturnType.Reward, true);
                    uil.deleteButton.Click += (object sender, RoutedEventArgs e) =>
                    {
                        MainWindow.Instance.listQuestRewards.Children.Remove(Util.FindParent<Universal_ItemList>(sender as Button));
                    };
                    MainWindow.Instance.listQuestRewards.Children.Add(uil);
                }
                foreach (Condition cond in value.conditions)
                {
                    Universal_ItemList uil = new Universal_ItemList(cond, Universal_ItemList.ReturnType.Condition, true);
                    uil.deleteButton.Click += (object sender, RoutedEventArgs e) => 
                    {
                        MainWindow.Instance.listQuestConditions.Children.Remove(Util.FindParent<Universal_ItemList>(sender as Button));
                    };
                    MainWindow.Instance.listQuestConditions.Children.Add(uil);
                }
                MainWindow.Instance.questIdBox.Value = value.id;
                MainWindow.Instance.questTitleBox.Text = value.title;
                MainWindow.Instance.questDescBox.Text = value.description;
                MainWindow.Instance.quest_commentbox.Text = value.Comment;
            }
        }
        public void Open()
        {
            Universal_ListView ulv = new Universal_ListView(MainWindow.CurrentProject.data.quests.OrderBy(d => d.id).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Quest, false)).ToList(), Universal_ItemList.ReturnType.Quest);
            if (ulv.ShowDialog() == true)
            {
                Save();
                Current = ulv.SelectedValue as NPCQuest;
            }
            MainWindow.CurrentProject.data.quests = ulv.Values.Cast<NPCQuest>().ToList();
        }
        public void Reset()
        {
            MainWindow.Instance.listQuestConditions.Children.Clear();
            MainWindow.Instance.listQuestRewards.Children.Clear();
            MainWindow.Instance.questTitleBox.Text = "";
            MainWindow.Instance.questDescBox.Text = "";
            MainWindow.Instance.questIdBox.Value = 0;
        }
        public void Save()
        {
            NPCQuest cur = Current;
            if (cur.id == 0)
                return;
            if (MainWindow.CurrentProject.data.quests.Where(d => d.id == MainWindow.Instance.questIdBox.Value).Count() > 0)
                MainWindow.CurrentProject.data.quests.Remove(MainWindow.CurrentProject.data.quests.Where(d => d.id == MainWindow.Instance.questIdBox.Value).ElementAt(0));
            MainWindow.CurrentProject.data.quests.Add(cur);
            MainWindow.CurrentProject.isSaved = false;
            App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Quest_Saved"));
        }

        public void SendPresence()
        {
            var current = MainWindow.QuestEditor.Current;
            RichPresence presence = new RichPresence();
            presence.Timestamps = new Timestamps();
            presence.Timestamps.StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            presence.Assets = new Assets();
            presence.Assets.SmallImageKey = "icon_exclamation_outlined";
            presence.Assets.SmallImageText = $"Quests: {MainWindow.CurrentProject.data.quests.Count}";
            presence.Details = $"Quest Name: {(current == null ? "Untitled" : current.title)}";
            presence.State = $"Rewards: {(current == null ? 0 : current.rewards.Count)} | Conds: {(current == null ? 0 : current.conditions.Count)}";
            MainWindow.DiscordManager.SendPresence(presence);
        }

        public void AddReward(Reward reward)
        {
            Universal_ItemList uil = new Universal_ItemList(reward, Universal_ItemList.ReturnType.Reward, true);
            uil.deleteButton.Click += (object sender, RoutedEventArgs e) =>
            {
                MainWindow.Instance.listQuestRewards.Children.Remove(Util.FindParent<Universal_ItemList>(sender as Button));
            };
            MainWindow.Instance.listQuestRewards.Children.Add(uil);
        }
        public void AddCondition(Condition condition)
        {
            Universal_ItemList uil = new Universal_ItemList(condition, Universal_ItemList.ReturnType.Condition, true);
            uil.deleteButton.Click += (object sender, RoutedEventArgs e) =>
            {
                MainWindow.Instance.listQuestConditions.Children.Remove(Util.FindParent<Universal_ItemList>(sender as Button));
            };
            MainWindow.Instance.listQuestConditions.Children.Add(uil);
        }
    }
}
