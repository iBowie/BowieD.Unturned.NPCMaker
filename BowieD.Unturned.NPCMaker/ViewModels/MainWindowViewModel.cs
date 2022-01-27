using BowieD.Unturned.NPCMaker.Commands;
using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Common.Utility;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.FindReplace;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using DiscordRPC;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            RoutedCommand saveHotkey = new RoutedCommand();
            saveHotkey.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            MainWindow.CommandBindings.Add(new CommandBinding(saveHotkey,
                new ExecutedRoutedEventHandler((object sender, ExecutedRoutedEventArgs e) =>
                {
                    SaveProjectCommand.Execute(null);
                })));
            RoutedCommand loadHotkey = new RoutedCommand();
            loadHotkey.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            MainWindow.CommandBindings.Add(new CommandBinding(loadHotkey,
                new ExecutedRoutedEventHandler((object sender, ExecutedRoutedEventArgs e) =>
                {
                    LoadProjectCommand.Execute(null);
                })));
            RoutedCommand exportHotkey = new RoutedCommand();
            exportHotkey.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
            MainWindow.CommandBindings.Add(new CommandBinding(exportHotkey,
                new ExecutedRoutedEventHandler((object sender, ExecutedRoutedEventArgs e) =>
                {
                    ExportProjectCommand.Execute(null);
                })));
            RoutedCommand newFileHotkey = new RoutedCommand();
            newFileHotkey.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            MainWindow.CommandBindings.Add(new CommandBinding(newFileHotkey,
                new ExecutedRoutedEventHandler((object sender, ExecutedRoutedEventArgs e) =>
                {
                    NewProjectCommand.Execute(null);
                })));
            RoutedCommand logWindow = new RoutedCommand();
            logWindow.InputGestures.Add(new KeyGesture(Key.F1, ModifierKeys.Control));
            MainWindow.CommandBindings.Add(new CommandBinding(logWindow,
                new ExecutedRoutedEventHandler((object sender, ExecutedRoutedEventArgs e) =>
                {
                    if (ConsoleLogger.IsOpened)
                    {
                        ConsoleLogger.HideConsoleWindow();
                    }
                    else
                    {
                        ConsoleLogger.ShowConsoleWindow();
                    }
                })));
            RoutedCommand pasteCommand = new RoutedCommand();
            pasteCommand.InputGestures.Add(new KeyGesture(Key.V, ModifierKeys.Control));
            MainWindow.CommandBindings.Add(new CommandBinding(pasteCommand,
                new ExecutedRoutedEventHandler((object sender, ExecutedRoutedEventArgs e) =>
                {
                    switch (MainWindow.mainTabControl.SelectedIndex)
                    {
                        case 0 when CharacterTabViewModel.Character != null: // character (condition)
                            {
                                if (ClipboardManager.TryGetObject(ClipboardManager.ConditionFormat, out var obj) && obj is Condition cond)
                                {
                                    CharacterTabViewModel.Character.visibilityConditions.Add(cond);
                                }
                            }
                            break;
                        case 1 when DialogueTabViewModel.Dialogue != null: // dialogue (nothing at this moment)
                            {

                            }
                            break;
                        case 2 when VendorTabViewModel.Vendor != null: // vendor (vendor item)
                            {
                                if (ClipboardManager.TryGetObject(ClipboardManager.VendorItemFormat, out var obj) && obj is VendorItem item)
                                {
                                    if (item.isBuy)
                                        VendorTabViewModel.AddItemBuy(item);
                                    else
                                        VendorTabViewModel.AddItemSell(item);
                                }
                            }
                            break;
                        case 4 when QuestTabViewModel.Quest != null: // quest (condition, reward)
                            {
                                if (ClipboardManager.TryGetObject(ClipboardManager.ConditionFormat, out var obj) && obj is Condition cond)
                                {
                                    QuestTabViewModel.AddCondition(new Controls.Universal_ItemList(cond, true));
                                }
                                else if (ClipboardManager.TryGetObject(ClipboardManager.RewardFormat, out obj) && obj is Reward rew)
                                {
                                    QuestTabViewModel.AddReward(new Controls.Universal_ItemList(rew, true));
                                }
                            }
                            break;
                    }
                })));

            MainWindow.txtID.BindFindReplace(FindReplaceFormats.CHARACTER_ID);
            MainWindow.txtStartDialogueID.BindFindReplace(FindReplaceFormats.DIALOGUE_ID);
            MainWindow.dialogueInputIdControl.BindFindReplace(FindReplaceFormats.DIALOGUE_ID);
            MainWindow.vendorIdTxtBox.BindFindReplace(FindReplaceFormats.VENDOR_ID);
            MainWindow.questIdBox.BindFindReplace(FindReplaceFormats.QUEST_ID);

            CharacterTabViewModel = new CharacterTabViewModel();
            DialogueTabViewModel = new DialogueTabViewModel();
            VendorTabViewModel = new VendorTabViewModel();
            DialogueVendorTabViewModel = new VirtualDialogueVendorTabViewModel();
            QuestTabViewModel = new QuestTabViewModel();
            CurrencyTabViewModel = new CurrencyTabViewModel();
            MainWindow.mainTabControl.SelectionChanged += TabControl_SelectionChanged;

            MainWindow.CurrentProject.OnDataLoaded += async () =>
            {
                ResetAll();

                ProjectData proj = MainWindow.CurrentProject;
                NPCProject data = proj.data;

                if (data.lastCharacter > -1 && data.lastCharacter < data.characters.Count)
                {
                    CharacterTabViewModel.Character = data.characters[data.lastCharacter];
                }

                if (data.lastDialogue > -1 && data.lastDialogue < data.dialogues.Count)
                {
                    DialogueTabViewModel.Dialogue = data.dialogues[data.lastDialogue];
                }

                if (data.lastVendor > -1 && data.lastVendor < data.vendors.Count)
                {
                    VendorTabViewModel.Vendor = data.vendors[data.lastVendor];
                }

                if (data.lastDialogueVendor > -1 && data.lastDialogueVendor < data.dialogueVendors.Count)
                {
                    DialogueVendorTabViewModel.DialogueVendor = data.dialogueVendors[data.lastDialogueVendor];
                }

                if (data.lastQuest > -1 && data.lastQuest < data.quests.Count)
                {
                    QuestTabViewModel.Quest = data.quests[data.lastQuest];
                }

                if (data.lastCurrency > -1 && data.lastCurrency < data.currencies.Count)
                {
                    CurrencyTabViewModel.Currency = data.currencies[data.lastCurrency];
                }

                GameAssetManager.Purge(EGameAssetOrigin.Hooked);

                if (data.settings.assetDirs != null && data.settings.assetDirs.Count > 0)
                {
                    MainWindow.blockActionsOverlay.Dispatcher.Invoke(() =>
                    {
                        MainWindow.blockActionsOverlay.Visibility = Visibility.Visible;
                    });

                    foreach (var ad in data.settings.assetDirs)
                    {
                        MainWindow.textBlockActions.Dispatcher.Invoke(() =>
                        {
                            MainWindow.textBlockActions.Text = LocalizationManager.Current.Interface.Translate("StartUp_ImportGameAssets_Window_Step_Hooked", ad);
                        });

                        await GameAssetManager.Import(ad, EGameAssetOrigin.Hooked, (cur, total) =>
                        {
                            MainWindow.progrBar.Dispatcher.Invoke(() =>
                            {
                                MainWindow.progrBar.Value = cur;
                                MainWindow.progrBar.Maximum = total;
                            });
                        });
                    }

                    MainWindow.blockActionsOverlay.Dispatcher.Invoke(() =>
                    {
                        MainWindow.blockActionsOverlay.Visibility = Visibility.Collapsed;
                    });
                }

                UpdateAllTabs();
            };
        }
        public MainWindow MainWindow { get; set; }
        public CharacterTabViewModel CharacterTabViewModel { get; set; }
        public DialogueTabViewModel DialogueTabViewModel { get; set; }
        public VendorTabViewModel VendorTabViewModel { get; set; }
        public VirtualDialogueVendorTabViewModel DialogueVendorTabViewModel { get; set; }
        public QuestTabViewModel QuestTabViewModel { get; set; }
        public CurrencyTabViewModel CurrencyTabViewModel { get; set; }
        public MistakeTabViewModel MistakeTabViewModel { get; set; }
        public void ResetAll()
        {
            CharacterTabViewModel.Reset();
            DialogueTabViewModel.Reset();
            VendorTabViewModel.Reset();
            DialogueVendorTabViewModel.Reset();
            QuestTabViewModel.Reset();
            CurrencyTabViewModel.Reset();
        }
        public void SaveAll()
        {
            CharacterTabViewModel.Save();
            DialogueTabViewModel.Save();
            VendorTabViewModel.Save();
            DialogueVendorTabViewModel.Save();
            QuestTabViewModel.Save();
            CurrencyTabViewModel.Save();

            ProjectData proj = MainWindow.CurrentProject;
            NPCProject data = proj.data;

            data.lastCharacter = data.characters.IndexOf(CharacterTabViewModel.Character);
            data.lastDialogue = data.dialogues.IndexOf(DialogueTabViewModel.Dialogue);
            data.lastQuest = data.quests.IndexOf(QuestTabViewModel.Quest);
            data.lastVendor = data.vendors.IndexOf(VendorTabViewModel.Vendor);
            data.lastDialogueVendor = data.dialogueVendors.IndexOf(DialogueVendorTabViewModel.DialogueVendor);
            data.lastCurrency = data.currencies.IndexOf(CurrencyTabViewModel.Currency);
        }
        public void UpdateAllTabs()
        {
            CharacterTabViewModel.UpdateTabs();
            DialogueTabViewModel.UpdateTabs();
            VendorTabViewModel.UpdateTabs();
            DialogueVendorTabViewModel.UpdateTabs();
            QuestTabViewModel.UpdateTabs();
            CurrencyTabViewModel.UpdateTabs();
        }
        internal void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e?.AddedItems.Count == 0 || sender == null)
            {
                return;
            }

            int selectedIndex = (sender as TabControl).SelectedIndex;
            TabItem tab = e?.AddedItems[0] as TabItem;
            if (AppConfig.Instance.animateControls && tab?.Content is Grid g)
            {
                DoubleAnimation anim = new DoubleAnimation(0, 1, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                g.BeginAnimation(MainWindow.OpacityProperty, anim);
            }
            if (selectedIndex == (sender as TabControl).Items.Count - 1)
            {
                Mistakes.MistakesManager.FindMistakes();
            }
            if (MainWindow.DiscordManager != null)
            {
                try
                {
                    switch (selectedIndex)
                    {
                        case 0:
                            {
                                MainWindow.DiscordManager.SendPresence(new RichPresence
                                {
                                    Timestamps = new Timestamps
                                    {
                                        StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                                    },
                                    Assets = new Assets
                                    {
                                        SmallImageKey = "icon_info_outlined",
                                        SmallImageText = $"Characters: {MainWindow.CurrentProject.data.characters.Count}".Shortify(125)
                                    },
                                    Details = $"Current NPC: {CharacterTabViewModel.EditorName}".Shortify(125),
                                    State = $"Display Name: {CharacterTabViewModel.DisplayName}".Shortify(125)
                                });
                            }
                            break;
                        case 1:
                            {
                                MainWindow.DiscordManager.SendPresence(new RichPresence
                                {
                                    Timestamps = new Timestamps
                                    {
                                        StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                                    },
                                    Assets = new Assets
                                    {
                                        SmallImageKey = "icon_chat_outlined",
                                        SmallImageText = $"Dialogues: {MainWindow.CurrentProject.data.dialogues.Count}".Shortify(125)
                                    },
                                    Details = $"Messages: {DialogueTabViewModel.Dialogue.Messages.Count}".Shortify(125),
                                    State = $"Responses: {DialogueTabViewModel.Dialogue.Responses.Count}".Shortify(125)
                                });
                            }
                            break;
                        case 2:
                            {
                                MainWindow.DiscordManager.SendPresence(new RichPresence
                                {
                                    Timestamps = new Timestamps
                                    {
                                        StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                                    },
                                    Assets = new Assets
                                    {
                                        SmallImageKey = "icon_money_outlined",
                                        SmallImageText = $"Vendors: {MainWindow.CurrentProject.data.vendors.Count}".Shortify(125)
                                    },
                                    Details = $"Vendor Name: {VendorTabViewModel.Title}".Shortify(125),
                                    State = $"Buy: {VendorTabViewModel.Vendor.items.Count(d => d.isBuy)} / Sell: {VendorTabViewModel.Vendor.items.Count(d => !d.isBuy)}".Shortify(125)
                                });
                            }
                            break;
                        case 3:
                            {
                                MainWindow.DiscordManager.SendPresence(new RichPresence
                                {
                                    Timestamps = new Timestamps
                                    {
                                        StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                                    },
                                    Assets = new Assets
                                    {
                                        SmallImageKey = "icon_money_outlined",
                                        SmallImageText = $"Dialogue Vendors: {MainWindow.CurrentProject.data.dialogueVendors.Count}".Shortify(125)
                                    },
                                    Details = $"Dialogue Vendor ID: {DialogueVendorTabViewModel.ID}".Shortify(125),
                                    State = $"Items: {DialogueVendorTabViewModel.DialogueVendor.Items.Count}".Shortify(125)
                                });
                            }
                            break;
                        case 4:
                            {
                                MainWindow.DiscordManager.SendPresence(new RichPresence
                                {
                                    Timestamps = new Timestamps
                                    {
                                        StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                                    },
                                    Assets = new Assets
                                    {
                                        SmallImageKey = "icon_exclamation_outlined",
                                        SmallImageText = $"Quests: {MainWindow.CurrentProject.data.quests.Count}".Shortify(125)
                                    },
                                    Details = $"Quest Name: {QuestTabViewModel.Title}".Shortify(125),
                                    State = $"Rewards: {QuestTabViewModel.Quest.rewards.Count} | Conds: {QuestTabViewModel.Quest.conditions.Count}".Shortify(125)
                                });
                            }
                            break;
                        case 5:
                            {
                                MainWindow.DiscordManager.SendPresence(new RichPresence
                                {
                                    Timestamps = new Timestamps
                                    {
                                        StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                                    },
                                    Assets = new Assets
                                    {
                                        SmallImageKey = "icon_money_outlined",
                                        SmallImageText = $"Currencies: {MainWindow.CurrentProject.data.currencies.Count}".Shortify(125)
                                    },
                                    Details = $"Currencies: {MainWindow.CurrentProject.data.currencies.Count}".Shortify(125),
                                    State = $"Editing currencies".Shortify(125)
                                });
                            }
                            break;
                        case 6:
                            {
                                MainWindow.DiscordManager.SendPresence(new RichPresence
                                {
                                    Timestamps = new Timestamps
                                    {
                                        StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                                    },
                                    Assets = new Assets()
                                    {
                                        SmallImageKey = "icon_warning_outlined",
                                        SmallImageText = $"Mistakes: {MainWindow.Instance.lstMistakes.Items.Count}".Shortify(125)
                                    },
                                    Details = $"Critical errors: {Mistakes.MistakesManager.Criticals_Count}".Shortify(125),
                                    State = $"Warnings: {Mistakes.MistakesManager.Warnings_Count}".Shortify(125)
                                });
                                break;
                            }
                        default:
                            {
                                MainWindow.DiscordManager.SendPresence(new RichPresence
                                {
                                    Timestamps = new Timestamps
                                    {
                                        StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                                    },
                                    Assets = new Assets()
                                    {
                                        SmallImageKey = "icon_question_outlined",
                                        SmallImageText = "Chilling in another dimension"
                                    },
                                    Details = $"If you can see this message".Shortify(125),
                                    State = $"It means that this user went across dimensions.".Shortify(125)
                                });
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.LogException("Could not update Rich Presence", ex: ex);
                }
            }
        }
        private ICommand
            newProjectCommand,
            loadProjectCommand,
            saveProjectCommand,
            saveAsProjectCommand,
            exportProjectCommand,
            exportProjectToUnturnedCommand,
            exportProjectToWorkshopCommand,
            exitCommand,
            optionsCommand,
            projectSettingsCommand,
            aboutCommand,
            importFileCommand,
            importDirectoryCommand;
        public ICommand ImportDirectoryCommand
        {
            get
            {
                if (importDirectoryCommand == null)
                {
                    importDirectoryCommand = new BaseCommand(() =>
                    {
                        CommonOpenFileDialog ofd = new CommonOpenFileDialog
                        {
                            IsFolderPicker = true,
                            Multiselect = false,
                            RestoreDirectory = true,
                            Title = LocalizationManager.Current.Interface.Translate("Main_Menu_File_Import_Directory_Title"),
                        };
                        CommonFileDialogResult result = ofd.ShowDialog();
                        if (result == CommonFileDialogResult.Ok)
                        {
                            ParseDirCommand pCommand = Command.GetCommand<ParseDirCommand>() as ParseDirCommand;
                            pCommand.Execute(new string[] { Path.GetDirectoryName(ofd.FileName) });
                            if (pCommand.LastResult)
                            {
                                App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Import_Directory_Done", pCommand.LastImported, pCommand.LastSkipped));
                                MainWindow.MainWindowViewModel.UpdateAllTabs();
                            }
                        }
                    });
                }
                return importDirectoryCommand;
            }
        }
        public ICommand ImportFileCommand
        {
            get
            {
                if (importFileCommand == null)
                {
                    importFileCommand = new BaseCommand(() =>
                    {
                        OpenFileDialog ofd = new OpenFileDialog()
                        {
                            Filter = $"Unturned Asset |Asset.dat",
                            Multiselect = true
                        };
                        if (ofd.ShowDialog() == true)
                        {
                            ParseCommand pCommand = Command.GetCommand<ParseCommand>() as ParseCommand;
                            foreach (string file in ofd.FileNames)
                            {
                                pCommand.Execute(new string[] { file });
                                if (pCommand.LastResult)
                                {
                                    App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Import_File_Done", file));
                                    MainWindow.MainWindowViewModel.UpdateAllTabs();
                                }
                                else
                                {
                                    App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Import_File_Fail", file));
                                }
                            }
                        }
                    });
                }
                return importFileCommand;
            }
        }
        public ICommand NewProjectCommand
        {
            get
            {
                if (newProjectCommand == null)
                {
                    newProjectCommand = new BaseCommand(() =>
                    {
                        if (MainWindow.CurrentProject.SavePrompt() == null)
                        {
                            return;
                        }

                        MainWindow.CurrentProject.data = new NPCProject();
                        MainWindow.CurrentProject.file = "";
                        ResetAll();
                        UpdateAllTabs();
                        MainWindow.CurrentProject.isSaved = true;
                        MainWindow.Started = DateTime.UtcNow;
                    });
                }
                return newProjectCommand;
            }
        }
        public ICommand SaveProjectCommand
        {
            get
            {
                if (saveProjectCommand == null)
                {
                    saveProjectCommand = new BaseCommand(() =>
                    {
                        SaveAll();

                        if (MainWindow.CurrentProject.Save())
                        {
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Project_Saved"]);

                            MainWindow.AddToRecentList(MainWindow.CurrentProject.file);
                        }
                    });
                }
                return saveProjectCommand;
            }
        }
        public ICommand SaveAsProjectCommand
        {
            get
            {
                if (saveAsProjectCommand == null)
                {
                    saveAsProjectCommand = new BaseCommand(() =>
                    {
                        SaveAll();
                        string oldPath = MainWindow.CurrentProject.file;
                        MainWindow.CurrentProject.file = "";
                        if (!MainWindow.CurrentProject.Save())
                        {
                            MainWindow.CurrentProject.file = oldPath;
                        }
                        else
                        {
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Project_Saved"]);

                            MainWindow.AddToRecentList(MainWindow.CurrentProject.file);
                        }
                    });
                }
                return saveAsProjectCommand;
            }
        }
        public ICommand LoadProjectCommand
        {
            get
            {
                if (loadProjectCommand == null)
                {
                    loadProjectCommand = new BaseCommand(() =>
                    {
                        string path;
                        OpenFileDialog ofd = new OpenFileDialog()
                        {
                            Filter =
                            $"{LocalizationManager.Current.General["Project_SaveFilter"]}|*.npcproj" + "|" +
                            $"{LocalizationManager.Current.General["Project_SaveFilter_Legacy"]}|*.npc",
                            Multiselect = false
                        };
                        bool? res = ofd.ShowDialog();
                        if (res == true)
                        {
                            path = ofd.FileName;
                        }
                        else
                        {
                            return;
                        }

                        string oldPath = MainWindow.CurrentProject.file;
                        MainWindow.CurrentProject.file = path;
                        if (MainWindow.CurrentProject.Load(null))
                        {
                            UpdateAllTabs();
                            App.NotificationManager.Clear();
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Project_Loaded"]);
                            MainWindow.AddToRecentList(MainWindow.CurrentProject.file);
                        }
                        else
                        {
                            MainWindow.CurrentProject.file = oldPath;
                        }
                    });
                }
                return loadProjectCommand;
            }
        }
        public ICommand ExportProjectCommand
        {
            get
            {
                if (exportProjectCommand == null)
                {
                    exportProjectCommand = new BaseCommand(() =>
                    {
                        Mistakes.MistakesManager.FindMistakes();
                        if (Mistakes.MistakesManager.Criticals_Count > 0)
                        {
                            SystemSounds.Hand.Play();
                            MainWindow.mainTabControl.SelectedIndex = MainWindow.mainTabControl.Items.Count - 1;
                            return;
                        }
                        if (Mistakes.MistakesManager.Warnings_Count > 0)
                        {
                            MessageBoxResult res = MessageBox.Show(LocalizationManager.Current.Interface["Export_Warnings_Text"], LocalizationManager.Current.Interface["Export_Warnings_Caption"], MessageBoxButton.YesNo);
                            if (!(res == MessageBoxResult.OK || res == MessageBoxResult.Yes))
                            {
                                return;
                            }
                        }
                        SaveAll();
                        if (MainWindow.CurrentProject.Save())
                        {
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Project_Saved"]);
                        }

                        Export.Exporter.ExportNPC(MainWindow.CurrentProject.data, Path.Combine(AppConfig.ExeDirectory, "results"));
                    });
                }
                return exportProjectCommand;
            }
        }
        public ICommand ExportProjectToUnturnedCommand
        {
            get
            {
                if (exportProjectToUnturnedCommand == null)
                {
                    exportProjectToUnturnedCommand = new AdvancedCommand(() =>
                    {
                        Mistakes.MistakesManager.FindMistakes();
                        if (Mistakes.MistakesManager.Criticals_Count > 0)
                        {
                            SystemSounds.Hand.Play();
                            MainWindow.mainTabControl.SelectedIndex = MainWindow.mainTabControl.Items.Count - 1;
                            return;
                        }
                        if (Mistakes.MistakesManager.Warnings_Count > 0)
                        {
                            MessageBoxResult res = MessageBox.Show(LocalizationManager.Current.Interface["Export_Warnings_Text"], LocalizationManager.Current.Interface["Export_Warnings_Caption"], MessageBoxButton.YesNo);
                            if (!(res == MessageBoxResult.OK || res == MessageBoxResult.Yes))
                            {
                                return;
                            }
                        }
                        SaveAll();
                        if (MainWindow.CurrentProject.Save())
                        {
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Project_Saved"]);
                        }

                        Export.Exporter.ExportNPC(MainWindow.CurrentProject.data, Path.Combine(AppConfig.Instance.unturnedDir, "Sandbox"));
                    }, (arg) =>
                    {
                        if (!string.IsNullOrEmpty(AppConfig.Instance.unturnedDir) && PathUtility.IsUnturnedPath(AppConfig.Instance.unturnedDir))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });
                }

                return exportProjectToUnturnedCommand;
            }
        }
        public ICommand ExportProjectToWorkshopCommand
        {
            get
            {
                if (exportProjectToWorkshopCommand == null)
                {
                    exportProjectToWorkshopCommand = new AdvancedCommand(async () =>
                    {
                        Mistakes.MistakesManager.FindMistakes();
                        if (Mistakes.MistakesManager.Criticals_Count > 0)
                        {
                            SystemSounds.Hand.Play();
                            MainWindow.mainTabControl.SelectedIndex = MainWindow.mainTabControl.Items.Count - 1;
                            return;
                        }
                        if (Mistakes.MistakesManager.Warnings_Count > 0)
                        {
                            MessageBoxResult res = MessageBox.Show(LocalizationManager.Current.Interface["Export_Warnings_Text"], LocalizationManager.Current.Interface["Export_Warnings_Caption"], MessageBoxButton.YesNo);
                            if (!(res == MessageBoxResult.OK || res == MessageBoxResult.Yes))
                            {
                                return;
                            }
                        }
                        SaveAll();
                        if (MainWindow.CurrentProject.Save())
                        {
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Project_Saved"]);
                        }

                        Export.Exporter.ExportNPC(MainWindow.CurrentProject.data, Path.Combine(AppConfig.ExeDirectory, "wshop_results"), true);

                        string resDir = Path.Combine(AppConfig.ExeDirectory, "wshop_results", $"{MainWindow.CurrentProject.data.guid}");

                        try
                        {
                            UGC_CreateUpdateView ugc_cuv = new UGC_CreateUpdateView()
                            { Owner = MainWindow };

                            if (ugc_cuv.ShowDialog() == true)
                            {
                                switch (ugc_cuv.Result)
                                {
                                    case UGC_CreateUpdateView.EResult.Create:
                                        {
                                            UGC_SelectorView ugc_sv = UGC_SelectorView.SV_Create(resDir);
                                            ugc_sv.Owner = MainWindow;
                                            if (ugc_sv.ShowDialog() == true)
                                            {
                                                var mod = ugc_sv.FinalizedUGC;

                                                MainWindow.ugcOverlayText.Text = LocalizationManager.Current.Interface["UGC_Steps_Uploading"];
                                                MainWindow.ugcOverlay.Visibility = Visibility.Visible;

                                                var t1 = await App.SteamManager.CreateUGC(mod);

                                                int eCode = t1.Item1;
                                                ulong fileID = t1.Item2;

                                                MainWindow.ugcOverlay.Visibility = Visibility.Collapsed;

                                                switch (eCode)
                                                {
                                                    case 0:
                                                        {
                                                            if (fileID > 0)
                                                            {
                                                                Button button = new Button
                                                                {
                                                                    Content = new TextBlock
                                                                    {
                                                                        Text = LocalizationManager.Current.Notification["Upload_Done_Goto"]
                                                                    }
                                                                };

                                                                button.Click += (sender, e) =>
                                                                {
                                                                    Process.Start($"https://steamcommunity.com/sharedfiles/filedetails/?id={fileID}");
                                                                };

                                                                App.NotificationManager.Notify(LocalizationManager.Current.Notification["Upload_Done"], buttons: button);
                                                            }
                                                            else
                                                            {
                                                                App.NotificationManager.Notify(LocalizationManager.Current.Notification["Upload_Done"]);
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        {
                                                            App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Upload_Error", eCode));
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case UGC_CreateUpdateView.EResult.Update:
                                        {
                                            MainWindow.ugcOverlayText.Text = LocalizationManager.Current.Interface["UGC_Steps_Query"];
                                            MainWindow.ugcOverlay.Visibility = Visibility.Visible;

                                            var ugcs = await App.SteamManager.QueryUGC();

                                            MainWindow.ugcOverlay.Visibility = Visibility.Collapsed;

                                            UGC_QueryListView ugc_qlv = new UGC_QueryListView(ugcs)
                                            { Owner = MainWindow };

                                            if (ugc_qlv.ShowDialog() == true)
                                            {
                                                UGC_SelectorView ugc_sv = UGC_SelectorView.SV_Update(ugc_qlv.Result, resDir);
                                                ugc_sv.Owner = MainWindow;
                                                if (ugc_sv.ShowDialog() == true)
                                                {
                                                    var mod = ugc_sv.FinalizedUGC;

                                                    MainWindow.ugcOverlayText.Text = LocalizationManager.Current.Interface["UGC_Steps_Uploading"];
                                                    MainWindow.ugcOverlay.Visibility = Visibility.Visible;

                                                    var t1 = await App.SteamManager.UpdateUGC(mod);

                                                    int eCode = t1.Item1;
                                                    ulong fileID = t1.Item2;

                                                    MainWindow.ugcOverlay.Visibility = Visibility.Collapsed;

                                                    switch (eCode)
                                                    {
                                                        case 0:
                                                            {
                                                                if (fileID > 0)
                                                                {
                                                                    Button button = new Button
                                                                    {
                                                                        Content = new TextBlock
                                                                        {
                                                                            Text = LocalizationManager.Current.Notification["Upload_Done_Goto"]
                                                                        }
                                                                    };

                                                                    button.Click += (sender, e) =>
                                                                    {
                                                                        Process.Start($"https://steamcommunity.com/sharedfiles/filedetails/?id={fileID}");
                                                                    };

                                                                    App.NotificationManager.Notify(LocalizationManager.Current.Notification["Upload_Done"], buttons: button);
                                                                }
                                                                else
                                                                {
                                                                    App.NotificationManager.Notify(LocalizationManager.Current.Notification["Upload_Done"]);
                                                                }
                                                            }
                                                            break;
                                                        default:
                                                            {
                                                                App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Upload_Error", eCode));
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            await App.Logger.LogException("Could not upload mod", ex: ex);
                        }
                    });
                }

                return exportProjectToWorkshopCommand;
            }
        }
        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new BaseCommand(() =>
                    {
                        MainWindow.PerformExit();
                    });
                }
                return exitCommand;
            }
        }
        public ICommand AboutCommand
        {
            get
            {
                if (aboutCommand == null)
                {
                    aboutCommand = new BaseCommand(() =>
                    {
                        Forms.Form_About form_About = new Forms.Form_About();
                        form_About.Owner = MainWindow;
                        form_About.ShowDialog();
                    });
                }
                return aboutCommand;
            }
        }
        public ICommand OptionsCommand
        {
            get
            {
                if (optionsCommand == null)
                {
                    optionsCommand = new BaseCommand(() =>
                    {
                        ConfigWindow configWindow = new Configuration.ConfigWindow();
                        configWindow.Owner = MainWindow;
                        configWindow.ShowDialog();
                    });
                }
                return optionsCommand;
            }
        }
        public ICommand ProjectSettingsCommand
        {
            get
            {
                if (projectSettingsCommand == null)
                {
                    projectSettingsCommand = new BaseCommand(() =>
                    {
                        ProjectSettingsView psv = new ProjectSettingsView(MainWindow.CurrentProject.data);
                        psv.Owner = MainWindow;
                        psv.ShowDialog();
                    });
                }
                return projectSettingsCommand;
            }
        }
        private ICommand _openFindReplaceCommand;
        public ICommand OpenFindReplaceCommand
        {
            get
            {
                if (_openFindReplaceCommand is null)
                {
                    _openFindReplaceCommand = new BaseCommand(() =>
                    {
                        FindReplaceDialog frd = new FindReplaceDialog();

                        frd.ShowDialog();
                    });
                }

                return _openFindReplaceCommand;
            }
        }
    }
}
