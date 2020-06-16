using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class DialogueTabViewModel : BaseViewModel
    {
        private NPCDialogue _dialogue;
        public DialogueTabViewModel()
        {
            Dialogue = new NPCDialogue();
        }
        public NPCDialogue Dialogue
        {
            get => _dialogue;
            set
            {
                _dialogue = value;

                MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Clear();
                foreach (var r in value.responses)
                    AddResponse(new Dialogue_Response(r));

                MainWindow.Instance.messagePagesGrid.Children.Clear();
                foreach (var m in value.messages)
                    AddMessage(new Dialogue_Message(m));

                OnPropertyChange("");
            }
        }
        public ushort ID
        {
            get => Dialogue.id;
            set
            {
                Dialogue.id = value;
                OnPropertyChange("ID");
            }
        }
        public string Comment
        {
            get => Dialogue.Comment;
            set
            {
                Dialogue.Comment = value;
                OnPropertyChange("Comment");
            }
        }

        private ICommand saveCommand, openCommand, resetCommand, addReplyCommand, addMessageCommand, setAsStartCommand, previewCommand;
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
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Dialogue_ID_Zero"]);
                            return;
                        }
                        UpdateResponses();
                        UpdateMessages();
                        if (!MainWindow.CurrentProject.data.dialogues.Contains(Dialogue))
                        {
                            MainWindow.CurrentProject.data.dialogues.Add(Dialogue);
                        }

                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["Dialogue_Saved"]);
                        MainWindow.CurrentProject.isSaved = false;
                        App.Logger.Log($"Dialogue {ID} saved!");
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
                        Universal_ListView ulv = new Universal_ListView(MainWindow.CurrentProject.data.dialogues.OrderBy(d => d.id).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Dialogue, false)).ToList(), Universal_ItemList.ReturnType.Dialogue);
                        ulv.Owner = MainWindow.Instance;
                        if (ulv.ShowDialog() == true)
                        {
                            SaveCommand.Execute(null);
                            Dialogue = ulv.SelectedValue as NPCDialogue;
                            App.Logger.Log($"Opened dialogue {ID}");
                        }
                        MainWindow.CurrentProject.data.dialogues = ulv.Values.Cast<NPCDialogue>().ToList();
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
                        Dialogue = new NPCDialogue();
                        App.Logger.Log($"Cleared dialogue {id}");
                    });
                }
                return resetCommand;
            }
        }
        public ICommand AddReplyCommand
        {
            get
            {
                if (addReplyCommand == null)
                {
                    addReplyCommand = new BaseCommand(() =>
                    {
                        AddResponse(new Dialogue_Response(new NPCResponse()));
                    });
                }
                return addReplyCommand;
            }
        }
        public ICommand AddMessageCommand
        {
            get
            {
                if (addMessageCommand == null)
                {
                    addMessageCommand = new BaseCommand(() =>
                    {
                        AddMessage(new Dialogue_Message(new NPCMessage()));
                    });
                }
                return addMessageCommand;
            }
        }
        public ICommand SetAsStartCommand
        {
            get
            {
                if (setAsStartCommand == null)
                {
                    setAsStartCommand = new BaseCommand(() =>
                    {
                        SaveCommand.Execute(null);
                        if (ID > 0 && MainWindow.Instance.MainWindowViewModel.CharacterTabViewModel.DialogueID != ID)
                        {
                            MainWindow.Instance.MainWindowViewModel.CharacterTabViewModel.DialogueID = ID;
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Dialogue_SetAsStart", ID));
                            App.Logger.Log($"Dialogue {ID} set as start!");
                        }
                    });
                }
                return setAsStartCommand;
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
                        Dictionary<ushort, bool?> results = new Dictionary<ushort, bool?>();
                        bool? checkResult = CheckDialogue(Dialogue, results);
                        switch (checkResult)
                        {
                            case true:
                            case null:
                                {
                                    if (checkResult == null)
                                        MessageBox.Show(LocalizationManager.Current.Interface["Main_Tab_Dialogue_Preview_Loop"]);
                                    
                                    Universal_Select select = new Universal_Select(Universal_ItemList.ReturnType.Character);
                                    if (select.ShowDialog() == true)
                                    {
                                        NPCCharacter character = select.SelectedValue as NPCCharacter;

                                        DialogueView_Window dvw = new DialogueView_Window(character, Dialogue, new Simulation());
                                        dvw.Owner = MainWindow.Instance;
                                        dvw.Display();
                                        dvw.ShowDialog();
                                    }

                                    break;
                                }
                            case false:
                                MessageBox.Show(LocalizationManager.Current.Interface["Main_Tab_Dialogue_Preview_Invalid"]);
                                break;
                        }
                    });
                }
                return previewCommand;
            }
        }

        void AddMessage(Dialogue_Message message)
        {
            message.deletePageButton.Click += (sender, e) =>
            {
                var current = (sender as UIElement).TryFindParent<Dialogue_Message>();
                var panel = MainWindow.Instance.messagePagesGrid;
                foreach (var ui in panel.Children)
                {
                    var dr = ui as Dialogue_Message;
                    if (dr.Equals(current))
                    {
                        panel.Children.Remove(dr);
                        break;
                    }
                }
                List<NPCMessage> newMessages = new List<NPCMessage>();

                foreach (UIElement ui in panel.Children)
                {
                    if (ui is Dialogue_Message dr)
                    {
                        newMessages.Add(dr.Message);
                    }
                }
                Dialogue.messages = newMessages;

                panel.UpdateOrderButtons<Dialogue_Message>();
            };
            message.moveUpButton.Click += (sender, e) =>
            {
                MainWindow.Instance.messagePagesGrid.MoveUp((sender as UIElement).TryFindParent<Dialogue_Message>());
                UpdateMessages();
            };
            message.moveDownButton.Click += (sender, e) =>
            {
                MainWindow.Instance.messagePagesGrid.MoveDown((sender as UIElement).TryFindParent<Dialogue_Message>());
                UpdateMessages();
            };
            MainWindow.Instance.messagePagesGrid.Children.Add(message);
            MainWindow.Instance.messagePagesGrid.UpdateOrderButtons<Dialogue_Message>();
        }
        void AddResponse(Dialogue_Response response)
        {
            response.deleteButton.Click += (sender, e) =>
            {
                var current = (sender as UIElement).TryFindParent<Dialogue_Response>();
                var panel = MainWindow.Instance.dialoguePlayerRepliesGrid;
                foreach (var ui in panel.Children)
                {
                    var dr = ui as Dialogue_Response;
                    if (dr.Equals(current))
                    {
                        panel.Children.Remove(dr);
                        break;
                    }
                }
                List<NPCResponse> newResponses = new List<NPCResponse>();

                foreach (UIElement ui in panel.Children)
                {
                    if (ui is Dialogue_Response dr)
                    {
                        newResponses.Add(dr.Response);
                    }
                }
                Dialogue.responses = newResponses;

                panel.UpdateOrderButtons<Dialogue_Response>();
            };
            response.orderButtonUp.Click += (sender, e) =>
            {
                MainWindow.Instance.dialoguePlayerRepliesGrid.MoveUp((sender as UIElement).TryFindParent<Dialogue_Response>());
                UpdateResponses();
            };
            response.orderButtonDown.Click += (sender, e) =>
            {
                MainWindow.Instance.dialoguePlayerRepliesGrid.MoveDown((sender as UIElement).TryFindParent<Dialogue_Response>());
                UpdateResponses();
            };
            MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Add(response);
            MainWindow.Instance.dialoguePlayerRepliesGrid.UpdateOrderButtons<Dialogue_Response>();
        }

        public void UpdateResponses()
        {
            Dialogue.responses.Clear();
            foreach (var uie in MainWindow.Instance.dialoguePlayerRepliesGrid.Children)
            {
                if (uie is Dialogue_Response dr)
                {
                    Dialogue.responses.Add(dr.Response);
                }
            }
            MainWindow.Instance.dialoguePlayerRepliesGrid.UpdateOrderButtons<Dialogue_Response>();
        }
        public void UpdateMessages()
        {
            Dialogue.messages.Clear();
            foreach (var uie in MainWindow.Instance.messagePagesGrid.Children)
            {
                if (uie is Dialogue_Message dm)
                {
                    Dialogue.messages.Add(dm.Message);
                }
            }
            MainWindow.Instance.dialoguePlayerRepliesGrid.UpdateOrderButtons<Dialogue_Message>();
        }

        /// <summary>
        /// <list type="bullet">
        /// <item>true - dialogue is safe</item>
        /// <item>false - dialogue is not safe</item>
        /// <item>null - dialogue can't be checked</item>
        /// </list>
        /// </summary>
        public static bool? CheckDialogue(NPCDialogue dialogue, Dictionary<ushort, bool?> results)
        {
            if (dialogue is null)
                return false;

            if (results.TryGetValue(dialogue.id, out var res))
                return res;

            results[dialogue.id] = null;

            foreach (var r in dialogue.responses)
            {
                if (r.openDialogueId != 0)
                {
                    var nextD = MainWindow.CurrentProject.data.dialogues.SingleOrDefault(d => d.id == r.openDialogueId);

                    if (nextD is null)
                    {
                        results[dialogue.id] = false;
                        return false;
                    }

                    bool? nextDres = CheckDialogue(nextD, results);
                    
                    if (nextDres == false)
                    {
                        results[dialogue.id] = false;
                        return false;
                    }
                    else if (nextDres == null)
                    {
                        results[dialogue.id] = null;
                        return null;
                    }
                }

                if (r.openQuestId != 0)
                {
                    var nextQ = MainWindow.CurrentProject.data.quests.SingleOrDefault(d => d.id == r.openQuestId);

                    if (nextQ is null)
                    {
                        results[dialogue.id] = false;
                        return false;
                    }
                }

                if (r.openVendorId != 0)
                {
                    var nextV = MainWindow.CurrentProject.data.vendors.SingleOrDefault(d => d.id == r.openVendorId);

                    if (nextV is null)
                    {
                        results[dialogue.id] = false;
                        return false;
                    }
                }
            }

            foreach (var m in dialogue.messages)
            {
                if (m.prev != 0)
                {
                    var prevD = MainWindow.CurrentProject.data.dialogues.SingleOrDefault(d => d.id == m.prev);

                    if (prevD is null)
                    {
                        results[dialogue.id] = false;
                        return false;
                    }
                }
            }

            results[dialogue.id] = true;
            return true;
        }
    }
}
