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

            ContextMenu cmenu = new ContextMenu();

            cmenu.Items.Add(ContextHelper.CreateAddFromTemplateButton(typeof(NPCResponse), (result) =>
            {
                if (result is NPCResponse npcr)
                    AddResponse(new Dialogue_Response(npcr));
            }));

            MainWindow.Instance.dialogueAddReplyButton.ContextMenu = cmenu;

            ContextMenu cmenu2 = new ContextMenu();

            cmenu2.Items.Add(ContextHelper.CreateAddFromTemplateButton(typeof(NPCMessage), (result) =>
            {
                if (result is NPCMessage npcm)
                    AddMessage(new Dialogue_Message(npcm));
            }));

            MainWindow.Instance.dialogueAddMessageButton.ContextMenu = cmenu2;

            ContextMenu cmenu3 = new ContextMenu();

            cmenu3.Items.Add(ContextHelper.CreateAddFromTemplateButton(typeof(NPCDialogue), (result) =>
            {
                if (result is NPCDialogue npcd)
                {
                    MainWindow.CurrentProject.data.dialogues.Add(npcd);
                    MetroTabItem tabItem = CreateTab(npcd);
                    MainWindow.Instance.dialogueTabSelect.Items.Add(tabItem);
                    MainWindow.Instance.dialogueTabSelect.SelectedIndex = MainWindow.Instance.dialogueTabSelect.Items.Count - 1;
                }
            }));

            MainWindow.Instance.dialogueTabButtonAdd.ContextMenu = cmenu3;

            var dialogueInputIdControlContext = new ContextMenu();

            dialogueInputIdControlContext.Items.Add(ContextHelper.CreateFindUnusedIDButton((id) =>
            {
                this.ID = id;
                MainWindow.Instance.dialogueInputIdControl.Value = id;
            }, GameIntegration.EGameAssetCategory.NPC));

            MainWindow.Instance.dialogueInputIdControl.ContextMenu = dialogueInputIdControlContext;
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

        public void Save()
        {
            if (!(_dialogue is null))
            {
                UpdateResponses();
                UpdateMessages();
            }
        }
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

            var cmenu = new ContextMenu();
            List<MenuItem> cmenuItems = new List<MenuItem>()
            {
                ContextHelper.CreateCopyButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    MetroTabItem target = context.PlacementTarget as MetroTabItem;
                    ClipboardManager.SetObject(Universal_ItemList.ReturnType.Dialogue, target.DataContext);
                }),
                ContextHelper.CreateDuplicateButton((object sender, RoutedEventArgs e) =>
                {
                    Save();

                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    MetroTabItem target = context.PlacementTarget as MetroTabItem;
                    var cloned = (target.DataContext as NPCDialogue).Clone();

                    MainWindow.CurrentProject.data.dialogues.Add(cloned);
                    MetroTabItem ti = CreateTab(cloned);
                    MainWindow.Instance.dialogueTabSelect.Items.Add(ti);
                }),
                ContextHelper.CreatePasteButton((object sender, RoutedEventArgs e) =>
                {
                    if (ClipboardManager.TryGetObject(ClipboardManager.DialogueFormat, out var obj) && !(obj is null) && obj is NPCDialogue cloned)
                    {
                        MainWindow.CurrentProject.data.dialogues.Add(cloned);
                        MetroTabItem ti = CreateTab(cloned);
                        MainWindow.Instance.dialogueTabSelect.Items.Add(ti);
                    }
                })
            };

            foreach (var cmenuItem in cmenuItems)
                cmenu.Items.Add(cmenuItem);

            tabItem.ContextMenu = cmenu;
            return tabItem;
        }

        public NPCDialogue Dialogue
        {
            get
            {
                Save();

                return _dialogue;
            }
            set
            {
                Save();

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
        public string GUID
        {
            get => Dialogue.GUID;
            set
            {
                Dialogue.GUID = value;
                OnPropertyChange("GUID");
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
        private ICommand randomGuidCommand;
        private ICommand setGuidCommand;

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
                    addReplyCommand = new AdvancedCommand(() =>
                    {
                        AddResponse(new Dialogue_Response(new NPCResponse()));
                    }, (p) =>
                    {
                        return _dialogue.Responses.CanAdd;
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
                    addMessageCommand = new AdvancedCommand(() =>
                    {
                        AddMessage(new Dialogue_Message(new NPCMessage()));
                    }, (p) =>
                    {
                        return _dialogue.Messages.CanAdd;
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

                                    AssetPicker_Window apw = new AssetPicker_Window(typeof(GameNPCAsset));
                                    apw.Owner = MainWindow.Instance;
                                    if (apw.ShowDialog() == true)
                                    {
                                        NPCCharacter character = (apw.SelectedAsset as GameNPCAsset).character;

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
                        if (mfiv.ShowDialog(new string[1] { LocalizationManager.Current.Dialogue["Guid"] }, "") == true)
                        {
                            string res = mfiv.Values[0];
                            if (Guid.TryParse(res, out var newGuid))
                            {
                                GUID = newGuid.ToString("N");
                            }
                            else
                            {
                                MessageBox.Show(LocalizationManager.Current.Dialogue["Guid_Invalid"]);
                            }
                        }
                    });
                }
                return setGuidCommand;
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
                _dialogue.Messages = newMessages.ToLimitedList(byte.MaxValue);

                panel.UpdateOrderButtons<Dialogue_Message>();
            };
            message.OnStoppedDrag += () =>
            {
                UpdateMessages();
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
                _dialogue.Responses = newResponses.ToLimitedList(byte.MaxValue);

                panel.UpdateOrderButtons<Dialogue_Response>();
            };
            response.OnStoppedDrag += () =>
            {
                UpdateResponses();
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
                    if (GameAssetManager.TryGetAsset<GameDialogueAsset>(r.openDialogueId, out var nextD))
                    {
                        bool? nextDres = CheckDialogue(nextD.dialogue, results);

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
                    else
                    {
                        results[dialogue.ID] = false;
                        return false;
                    }
                }

                if (r.openQuestId != 0 && !GameAssetManager.TryGetAsset<GameQuestAsset>(r.openQuestId, out var nextQ))
                {
                    results[dialogue.ID] = false;
                    return false;
                }

                if (r.openVendorId != 0 && !GameAssetManager.TryGetAsset<GameVendorAsset>(r.openVendorId, out var nextV))
                {
                    results[dialogue.ID] = false;
                    return false;
                }
            }

            foreach (var m in dialogue.Messages)
            {
                if (m.prev != 0 && !GameAssetManager.TryGetAsset<GameDialogueAsset>(m.prev, out var prevD))
                {
                    results[dialogue.ID] = false;
                    return false;
                }
            }

            results[dialogue.ID] = true;
            return true;
        }
    }
}
