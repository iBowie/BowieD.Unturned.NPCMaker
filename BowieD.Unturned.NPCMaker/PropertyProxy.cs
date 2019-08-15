using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Mistakes;
using BowieD.Unturned.NPCMaker.NPC;
using DiscordRPC;
using Microsoft.Win32;
using System;
using System.Media;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace BowieD.Unturned.NPCMaker
{
    public class PropertyProxy
    {
        public PropertyProxy(MainWindow main)
        {
            inst = main;
        }
        public void RegisterEvents()
        {
            inst.newButton.Click += NewButtonClick;
            inst.lstMistakes.SelectionChanged += MistakeList_Selected;
            inst.regenerateGuidsButton.Click += RegenerateGuids_Click;
            inst.optionsMenuItem.Click += Options_Click;
            inst.exitButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                MainWindow.PerformExit();
            });
            inst.exportButton.Click += ExportClick;
            inst.saveButton.Click += SaveClick;
            inst.saveAsButton.Click += SaveAsClick;
            inst.loadButton.Click += LoadClick;
            inst.mainTabControl.SelectionChanged += TabControl_SelectionChanged;
            inst.aboutMenuItem.Click += AboutMenu_Click;
            inst.vkComm.Click += FeedbackItemClick;
            inst.discordComm.Click += FeedbackItemClick;
            inst.steamComm.Click += FeedbackItemClick;
            RoutedCommand saveHotkey = new RoutedCommand();
            saveHotkey.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            inst.CommandBindings.Add(new CommandBinding(saveHotkey,
                new ExecutedRoutedEventHandler((object sender, ExecutedRoutedEventArgs e) =>
                {
                    MainWindow.CurrentProject.Save();
                })));
            RoutedCommand loadHotkey = new RoutedCommand();
            loadHotkey.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            inst.CommandBindings.Add(new CommandBinding(loadHotkey,
                new ExecutedRoutedEventHandler((object sender, ExecutedRoutedEventArgs e) =>
                {
                    LoadClick(sender, null);
                })));
            RoutedCommand exportHotkey = new RoutedCommand();
            exportHotkey.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
            inst.CommandBindings.Add(new CommandBinding(exportHotkey,
                new ExecutedRoutedEventHandler((object sender, ExecutedRoutedEventArgs e) =>
                {
                    ExportClick(sender, null);
                })));
            RoutedCommand newFileHotkey = new RoutedCommand();
            newFileHotkey.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            inst.CommandBindings.Add(new CommandBinding(newFileHotkey,
                new ExecutedRoutedEventHandler((object sender, ExecutedRoutedEventArgs e) =>
                {
                    NewButtonClick(sender, null);
                })));
            RoutedCommand logWindow = new RoutedCommand();
            logWindow.InputGestures.Add(new KeyGesture(Key.F1, ModifierKeys.Control));
            inst.CommandBindings.Add(new CommandBinding(logWindow,
                new ExecutedRoutedEventHandler((object sender, ExecutedRoutedEventArgs e) =>
            {
                if (ConsoleLogger.IsOpened)
                    ConsoleLogger.HideConsoleWindow();
                else
                    ConsoleLogger.ShowConsoleWindow();
            })));
        }


        private readonly MainWindow inst;

        #region EVENTS
        internal void MistakeList_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (inst.lstMistakes.SelectedItem != null && inst.lstMistakes.SelectedItem is Mistake mist)
            {
                mist.OnClick?.Invoke();
            }
        }
        internal void RegenerateGuids_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentProject.data.characters != null)
            {
                foreach (NPCCharacter c in MainWindow.CurrentProject.data.characters)
                {
                    if (c != null)
                        c.guid = Guid.NewGuid().ToString("N");
                }
            }
            if (MainWindow.CurrentProject.data.dialogues != null)
            {
                foreach (NPCDialogue d in MainWindow.CurrentProject.data.dialogues)
                {
                    if (d != null)
                        d.guid = Guid.NewGuid().ToString("N");
                }
            }
            if (MainWindow.CurrentProject.data.vendors != null)
            {
                foreach (NPCVendor v in MainWindow.CurrentProject.data.vendors)
                {
                    if (v != null)
                        v.guid = Guid.NewGuid().ToString("N");
                }
            }
            if (MainWindow.CurrentProject.data.quests != null)
            {
                foreach (NPCQuest q in MainWindow.CurrentProject.data.quests)
                {
                    if (q != null)
                        q.guid = Guid.NewGuid().ToString("N");
                }
            }
            App.NotificationManager.Notify(LocUtil.LocalizeInterface("character_Regenerated"));
            MainWindow.CurrentProject.isSaved = false;
        }
        internal void Options_Click(object sender, RoutedEventArgs e)
        {
            Configuration.ConfigWindow cw = new Configuration.ConfigWindow();
            cw.ShowDialog();
        }
        internal void ExportClick(object sender, RoutedEventArgs e)
        {
            Mistakes.MistakesManager.FindMistakes();
            if (Mistakes.MistakesManager.Criticals_Count > 0)
            {
                SystemSounds.Hand.Play();
                inst.mainTabControl.SelectedIndex = inst.mainTabControl.Items.Count - 1;
                return;
            }
            if (Mistakes.MistakesManager.Warnings_Count > 0)
            {
                var res = MessageBox.Show(LocUtil.LocalizeInterface("export_Warnings_Desc"), LocUtil.LocalizeInterface("export_Warnings_Title"), MessageBoxButton.YesNo);
                if (!(res == MessageBoxResult.OK || res == MessageBoxResult.Yes))
                    return;
            }
            MainWindow.SaveAllEditors();
            if (MainWindow.CurrentProject.Save())
                App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Saved"));
            Export.Exporter.ExportNPC(MainWindow.CurrentProject.data);
        }
        internal void NewButtonClick(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentProject.SavePrompt() == null)
                return;
            MainWindow.CurrentProject.data = new NPCProject();
            MainWindow.CurrentProject.file = "";
            MainWindow.ResetEditors();
            MainWindow.CurrentProject.isSaved = true;
            MainWindow.Started = DateTime.UtcNow;
        }
        internal void SaveClick(object sender, RoutedEventArgs e)
        {
            MainWindow.SaveAllEditors();
            if (MainWindow.CurrentProject.Save())
            {
                App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Saved"));
            }
        }
        internal void SaveAsClick(object sender, RoutedEventArgs e)
        {
            MainWindow.SaveAllEditors();
            string oldPath = MainWindow.CurrentProject.file;
            MainWindow.CurrentProject.file = "";
            if (!MainWindow.CurrentProject.Save())
            {
                MainWindow.CurrentProject.file = oldPath;
            }
            else
            {
                App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Saved"));
            }
        }
        internal void LoadClick(object sender, RoutedEventArgs e)
        {
            string path;
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = $"{LocUtil.LocalizeInterface("save_Filter")} (*.npc,*.npcproj)|*.npc;*.npcproj",
                Multiselect = false
            };
            var res = ofd.ShowDialog();
            if (res == true)
                path = ofd.FileName;
            else
                return;
            string oldPath = MainWindow.CurrentProject.file;
            MainWindow.CurrentProject.file = path;
            if (MainWindow.CurrentProject.Load(null))
            {
                App.NotificationManager.Clear();
                App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Loaded"));
                MainWindow.AddToRecentList(MainWindow.CurrentProject.file);
                MainWindow.ResetEditors();
            }
            else
            {
                MainWindow.CurrentProject.file = oldPath;
            }
        }
        internal void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e?.AddedItems.Count == 0 || sender == null)
                return;
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
                switch (selectedIndex)
                {
                    case 0:
                        MainWindow.CharacterEditor.SendPresence();
                        break;
                    case 1:
                        MainWindow.DialogueEditor.SendPresence();
                        break;
                    case 2:
                        MainWindow.VendorEditor.SendPresence();
                        break;
                    case 3:
                        MainWindow.QuestEditor.SendPresence();
                        break;
                    case 4:
                        {
                            RichPresence presence = new RichPresence
                            {
                                Timestamps = new Timestamps
                                {
                                    StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                                },
                                Assets = new Assets()
                            };
                            presence.Assets.SmallImageKey = "icon_warning_outlined";
                            presence.Assets.SmallImageText = $"Mistakes: {MainWindow.Instance.lstMistakes.Items.Count}";
                            presence.Details = $"Critical errors: {Mistakes.MistakesManager.Criticals_Count}";
                            presence.State = $"Warnings: {Mistakes.MistakesManager.Warnings_Count}";
                            MainWindow.DiscordManager.SendPresence(presence);
                            break;
                        }
                    default:
                        {

                            RichPresence presence = new RichPresence
                            {
                                Timestamps = new Timestamps
                                {
                                    StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                                },
                                Assets = new Assets()
                            };
                            presence.Assets.SmallImageKey = "icon_question_outlined";
                            presence.Assets.SmallImageText = "Something?..";
                            presence.Details = $"If this is shown";
                            presence.State = $"Then something gone wrong.";
                            MainWindow.DiscordManager.SendPresence(presence);
                            break;
                        }
                }
            }
        }
        internal void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            Forms.Form_About a = new Forms.Form_About();
            a.ShowDialog();
        }
        internal void FeedbackItemClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start((sender as MenuItem).Tag.ToString());
        }
        internal void WhatsNew_Menu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (WebClient wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    new Whats_New().ShowDialog();
                }
            }
            catch (Exception ex) { App.Logger.LogException("Could not open update notes window.", ex); }
        }
        #endregion
    }
}
