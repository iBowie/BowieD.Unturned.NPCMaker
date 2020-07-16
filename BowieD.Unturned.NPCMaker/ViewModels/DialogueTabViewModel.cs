using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class DialogueTabViewModel : BaseViewModel, ITabEditor, INPCTab
    {
        private NPCDialogue _dialogue;
        public DialogueTabViewModel()
        {
            MainWindow.Instance.dialogueTabSelect.SelectionChanged += DialogueTabSelect_SelectionChanged;
            MainWindow.Instance.dialogueTabButtonAdd.Click += DialogueTabButtonAdd_Click;
            NPCDialogue empty = new NPCDialogue();
            Dialogue = empty;
            UpdateTabs();
        }

        private void DialogueTabButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NPCDialogue item = new NPCDialogue();
            MainWindow.CurrentProject.data.dialogues.Add(item);
            MetroTabItem tabItem = CreateTab(item);
            MainWindow.Instance.dialogueTabSelect.Items.Add(tabItem);
            MainWindow.Instance.dialogueTabSelect.SelectedIndex = MainWindow.Instance.dialogueTabSelect.Items.Count - 1;
        }

        private void DialogueTabSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = MainWindow.Instance.dialogueTabSelect;
            if (tab.SelectedItem != null && tab.SelectedItem is TabItem tabItem && tabItem.DataContext != null)
            {
                NPCDialogue selectedTabChar = tabItem.DataContext as NPCDialogue;
                if (selectedTabChar != null)
                    Dialogue = selectedTabChar;
            }

            if (tab.SelectedItem is null)
            {
                MainWindow.Instance.dialogueTabGrid.IsEnabled = false;
                MainWindow.Instance.dialogueTabGrid.Visibility = Visibility.Collapsed;
                MainWindow.Instance.dialogueTabGridNoSelection.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.dialogueTabGrid.IsEnabled = true;
                MainWindow.Instance.dialogueTabGrid.Visibility = Visibility.Visible;
                MainWindow.Instance.dialogueTabGridNoSelection.Visibility = Visibility.Collapsed;
            }
        }

        public void Save() { }
        public void Reset() { }

        public void UpdateTabs()
        {
            var tab = MainWindow.Instance.dialogueTabSelect;
            tab.Items.Clear();
            int selected = -1;
            for (int i = 0; i < MainWindow.CurrentProject.data.dialogues.Count; i++)
            {
                var dialogue = MainWindow.CurrentProject.data.dialogues[i];
                if (dialogue == _dialogue)
                    selected = i;
                MetroTabItem tabItem = CreateTab(dialogue);
                tab.Items.Add(tabItem);
            }
            if (selected != -1)
                tab.SelectedIndex = selected;

            if (tab.SelectedItem is null)
            {
                MainWindow.Instance.dialogueTabGrid.IsEnabled = false;
                MainWindow.Instance.dialogueTabGrid.Visibility = Visibility.Collapsed;
                MainWindow.Instance.dialogueTabGridNoSelection.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.dialogueTabGrid.IsEnabled = true;
                MainWindow.Instance.dialogueTabGrid.Visibility = Visibility.Visible;
                MainWindow.Instance.dialogueTabGridNoSelection.Visibility = Visibility.Collapsed;
            }
        }

        private MetroTabItem CreateTab(NPCDialogue dialogue)
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
            tabItem.DataContext = dialogue;
            return tabItem;
        }

        public NPCDialogue Dialogue
        {
            get
            {
                if (!(_dialogue is null))
                {
                    UpdateResponses();
                    UpdateMessages();
                }
                return _dialogue;
            }
            set
            {
                if (!(_dialogue is null))
                {
                    UpdateResponses();
                    UpdateMessages();
                }

                _dialogue = value;

                MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Clear();
                foreach (var r in value.Responses)
                    AddResponse(new Dialogue_Response(r));

                MainWindow.Instance.messagePagesGrid.Children.Clear();
                foreach (var m in value.Messages)
                    AddMessage(new Dialogue_Message(m));

                OnPropertyChange("");
            }
        }
        public ushort ID
        {
            get => Dialogue.ID;
            set
            {
                Dialogue.ID = value;
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

        private ICommand addReplyCommand, addMessageCommand, setAsStartCommand, previewCommand;

        private ICommand closeTabCommand;
        private ICommand sortIDA, sortIDD;
        public ICommand SortIDAscending
        {
            get
            {
                if (sortIDA == null)
                {
                    sortIDA = new BaseCommand(() =>
                    {
                        MainWindow.CurrentProject.data.dialogues = MainWindow.CurrentProject.data.dialogues.OrderBy(d => d.ID).ToList();
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
                        MainWindow.CurrentProject.data.dialogues = MainWindow.CurrentProject.data.dialogues.OrderByDescending(d => d.ID).ToList();
                        UpdateTabs();
                    });
                }
                return sortIDD;
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
                        MainWindow.CurrentProject.data.dialogues.Remove(tab.DataContext as NPCDialogue);
                        MainWindow.Instance.dialogueTabSelect.Items.Remove(sender);
                    });
                }
                return closeTabCommand;
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
                _dialogue.Messages = newMessages;

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
                _dialogue.Responses = newResponses;

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
            _dialogue.Responses.Clear();
            foreach (var uie in MainWindow.Instance.dialoguePlayerRepliesGrid.Children)
            {
                if (uie is Dialogue_Response dr)
                {
                    _dialogue.Responses.Add(dr.Response);
                }
            }
            MainWindow.Instance.dialoguePlayerRepliesGrid.UpdateOrderButtons<Dialogue_Response>();
        }
        public void UpdateMessages()
        {
            _dialogue.Messages.Clear();
            foreach (var uie in MainWindow.Instance.messagePagesGrid.Children)
            {
                if (uie is Dialogue_Message dm)
                {
                    _dialogue.Messages.Add(dm.Message);
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

            if (results.TryGetValue(dialogue.ID, out var res))
                return res;

            results[dialogue.ID] = null;

            foreach (var r in dialogue.Responses)
            {
                if (r.openDialogueId != 0)
                {
                    var nextD = MainWindow.CurrentProject.data.dialogues.SingleOrDefault(d => d.ID == r.openDialogueId);

                    if (nextD is null)
                    {
                        results[dialogue.ID] = false;
                        return false;
                    }

                    bool? nextDres = CheckDialogue(nextD, results);

                    if (nextDres == false)
                    {
                        results[dialogue.ID] = false;
                        return false;
                    }
                    else if (nextDres == null)
                    {
                        results[dialogue.ID] = null;
                        return null;
                    }
                }

                if (r.openQuestId != 0)
                {
                    var nextQ = MainWindow.CurrentProject.data.quests.SingleOrDefault(d => d.ID == r.openQuestId);

                    if (nextQ is null)
                    {
                        results[dialogue.ID] = false;
                        return false;
                    }
                }

                if (r.openVendorId != 0)
                {
                    var nextV = MainWindow.CurrentProject.data.vendors.SingleOrDefault(d => d.ID == r.openVendorId);

                    if (nextV is null)
                    {
                        results[dialogue.ID] = false;
                        return false;
                    }
                }
            }

            foreach (var m in dialogue.Messages)
            {
                if (m.prev != 0)
                {
                    var prevD = MainWindow.CurrentProject.data.dialogues.SingleOrDefault(d => d.ID == m.prev);

                    if (prevD is null)
                    {
                        results[dialogue.ID] = false;
                        return false;
                    }
                }
            }

            results[dialogue.ID] = true;
            return true;
        }
    }
}
