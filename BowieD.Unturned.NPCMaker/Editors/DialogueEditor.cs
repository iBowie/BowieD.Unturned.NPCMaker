using BowieD.Unturned.NPCMaker.BetterControls;
using BowieD.Unturned.NPCMaker.BetterForms;
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
    public class DialogueEditor : IEditor<NPCDialogue>
    {
        public DialogueEditor()
        {
            MainWindow.Instance.dialogueSaveButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) => {
                Save();
                SendPresence();
            });
            MainWindow.Instance.dialogueOpenButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Open();
                SendPresence();
            });
            MainWindow.Instance.dialogueResetButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                Reset();
                SendPresence();
            });
            MainWindow.Instance.dialogueAddReplyButton.Click += AddReplyClick;
            MainWindow.Instance.dialogueAddMessageButton.Click += AddMessageClick;
            MainWindow.Instance.setAsStartButton.Click += SetAsStartClick;
        }

        public void Open()
        {
            var ulv = new Universal_ListView(MainWindow.CurrentProject.dialogues.OrderBy(d => d.id).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Dialogue, false)).ToList(), Universal_ItemList.ReturnType.Dialogue);
            if (ulv.ShowDialog() == true)
            {
                Save();
                Current = ulv.SelectedValue as NPCDialogue;
                Logger.Log($"Opened dialogue {MainWindow.Instance.dialogueInputIdControl.Value}");
            }
            MainWindow.CurrentProject.dialogues = ulv.Values.Cast<NPCDialogue>().ToList();
        }
        public void Reset()
        {
            List<UIElement> toRemove = new List<UIElement>();
            foreach (UIElement item in MainWindow.Instance.messagePagesGrid.Children)
            {
                if (item is Dialogue_Message)
                {
                    toRemove.Add(item);
                }
            }
            foreach (UIElement item in toRemove)
            {
                MainWindow.Instance.messagePagesGrid.Children.Remove(item);
            }
            toRemove = new List<UIElement>();
            foreach (UIElement item in MainWindow.Instance.dialoguePlayerRepliesGrid.Children)
            {
                if (item is Dialogue_Response)
                {
                    toRemove.Add(item);
                }
            }
            foreach (UIElement item in toRemove)
            {
                MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Remove(item);
            }
            Logger.Log($"Cleared dialogue {MainWindow.Instance.dialogueInputIdControl.Value}");
        }
        public void Save()
        {
            var dil = Current;
            if (dil.id == 0)
            {
                MainWindow.NotificationManager.Notify(LocUtil.LocalizeInterface("dialogue_ID_Zero"));
                return;
            }
            var o = MainWindow.CurrentProject.dialogues.Where(d => d.id == dil.id);
            if (o.Count() > 0)
                MainWindow.CurrentProject.dialogues.Remove(o.ElementAt(0));
            MainWindow.CurrentProject.dialogues.Add(dil);
            MainWindow.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Dialogue_Saved"));
            MainWindow.isSaved = false;
            Logger.Log($"Dialogue {dil.id} saved!");
        }
        public NPCDialogue Current
        {
            get
            {
                ushort dialogueID = (ushort)(MainWindow.Instance.dialogueInputIdControl.Value ?? 0);
                NPCDialogue ret = new NPCDialogue();
                List<Dialogue_Message> messages = new List<Dialogue_Message>();
                foreach (UIElement ui in MainWindow.Instance.messagePagesGrid.Children)
                {
                    if (ui is Dialogue_Message dm)
                    {
                        messages.Add(dm);
                    }
                }
                List<Dialogue_Response> responses = new List<Dialogue_Response>();
                foreach (UIElement ui in MainWindow.Instance.dialoguePlayerRepliesGrid.Children)
                {
                    if (ui is Dialogue_Response dr)
                    {
                        dr.RebuildResponse();
                        responses.Add(dr);
                    }
                }

                ret.messages = messages.Select(d => d.Message).ToList();
                ret.responses = responses.Select(d => d.Response).ToList();
                ret.id = dialogueID;
                ret.Comment = MainWindow.Instance.dialogue_commentbox.Text;
                return ret;
            }
            set
            {
                Reset();
                NPCDialogue d = value;
                MainWindow.Instance.dialogueInputIdControl.Value = d.id;
                foreach (NPCResponse response in d.responses)
                {
                    Dialogue_Response dialogue_Response = new Dialogue_Response(response);
                    dialogue_Response.deleteButton.Click += RemoveReplyClick;
                    int ind = 0;
                    for (int k = 0; k < MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Count; k++)
                    {
                        if (MainWindow.Instance.dialoguePlayerRepliesGrid.Children[k] is Dialogue_Response)
                        {
                            ind = k + 1;
                        }
                    }
                    MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Insert(ind, dialogue_Response);
                }
                foreach (UIElement uie in MainWindow.Instance.dialoguePlayerRepliesGrid.Children)
                {
                    if (uie is Dialogue_Response dr)
                    {
                        dr.UpdateOrderButtons();
                    }
                }
                foreach (NPCMessage message in d.messages)
                {
                    Dialogue_Message dialogue_Message = new Dialogue_Message(message);
                    dialogue_Message.deletePageButton.Click += RemoveMessageClick;
                    int ind = 0;
                    for (int k = 0; k < MainWindow.Instance.messagePagesGrid.Children.Count; k++)
                    {
                        if (MainWindow.Instance.messagePagesGrid.Children[k] is Dialogue_Message)
                            ind = k + 1;
                    }
                    MainWindow.Instance.messagePagesGrid.Children.Insert(ind, dialogue_Message);
                }
                MainWindow.Instance.dialogue_commentbox.Text = d.Comment;
            }
        }
        private void AddReplyClick(object sender, RoutedEventArgs e)
        {
            var o = new Dialogue_Response();
            o.deleteButton.Click += RemoveReplyClick;
            int ind = 0;
            for (int k = 0; k < MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Count; k++)
            {
                if (MainWindow.Instance.dialoguePlayerRepliesGrid.Children[k] is Dialogue_Response)
                {
                    ind = k + 1;
                }
            }
            MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Insert(ind, o);
            foreach (UIElement uie in MainWindow.Instance.dialoguePlayerRepliesGrid.Children)
            {
                if (uie is Dialogue_Response dr)
                {
                    dr.UpdateOrderButtons();
                }
            }
        }
        private void RemoveReplyClick(object sender, RoutedEventArgs e)
        {
            Dialogue_Response ans = Util.FindParent<Dialogue_Response>(sender as Button);
            MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Remove(ans);
            foreach (UIElement uie in MainWindow.Instance.dialoguePlayerRepliesGrid.Children)
            {
                if (uie is Dialogue_Response dr)
                {
                    dr.UpdateOrderButtons();
                }
            }
        }
        private void AddMessageClick(object sender, RoutedEventArgs e)
        {
            var o = new Dialogue_Message(new NPCMessage() { pages = new List<string>() });
            o.deletePageButton.Click += RemoveMessageClick;
            int ind = 0;
            for (int k = 0; k < MainWindow.Instance.messagePagesGrid.Children.Count; k++)
            {
                if (MainWindow.Instance.messagePagesGrid.Children[k] is Dialogue_Message)
                    ind = k + 1;
            }
            MainWindow.Instance.messagePagesGrid.Children.Insert(ind, o);
        }
        private void RemoveMessageClick(object sender, RoutedEventArgs e)
        {
            Dialogue_Message pag = Util.FindParent<Dialogue_Message>(sender as Button);
            MainWindow.Instance.messagePagesGrid.Children.Remove(pag);
        }
        private void SetAsStartClick(object sender, RoutedEventArgs e)
        {
            Save();
            var dial = Current;
            if (dial.id > 0 && MainWindow.Instance.txtStartDialogueID.Value != dial.id)
            {
                MainWindow.Instance.txtStartDialogueID.Value = dial.id;
                MainWindow.NotificationManager.Notify(LocUtil.LocalizeInterface("dialogue_Start_Notify", dial.id));
                Logger.Log($"Dialogue {dial.id} set as start!");
            }
        }

        public void SendPresence()
        {
            RichPresence presence = new RichPresence();
            presence.Timestamps = new Timestamps();
            presence.Timestamps.StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            presence.Assets = new Assets();
            presence.Assets.SmallImageKey = "icon_chat_outlined";
            presence.Assets.SmallImageText = $"Dialogues: {MainWindow.CurrentProject.dialogues.Count}";
            presence.Details = $"Messages: {MainWindow.DialogueEditor.Current.MessagesAmount}";
            presence.State = $"Responses: {MainWindow.DialogueEditor.Current.ResponsesAmount}";
            MainWindow.DiscordManager.SendPresence(presence);
        }
    }
}
