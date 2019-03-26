using BowieD.Unturned.NPCMaker.BetterControls;
using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.NPC;
using DiscordRPC;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

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
            inst.txtEditorName.TextChanged += EditorName_Change;
            inst.txtDisplayName.TextChanged += DisplayName_Change;
            inst.txtID.ValueChanged += NPC_ID_Change;
            inst.txtStartDialogueID.ValueChanged += StartDialogue_ID_Change;
            inst.hatIdBox.ValueChanged += HatId_Change;
            inst.topIdBox.ValueChanged += TopId_Change;
            inst.bottomIdBox.ValueChanged += BottomId_Change;
            inst.maskIdBox.ValueChanged += MaskId_Change;
            inst.backpackIdBox.ValueChanged += BackId_Change;
            inst.vestIdBox.ValueChanged += VestId_Change;
            inst.glassesIdBox.ValueChanged += GlassesId_Change;
            inst.primaryIdBox.ValueChanged += PrimaryId_Change;
            inst.secondaryIdBox.ValueChanged += SecondaryId_Change;
            inst.tertiaryIdBox.ValueChanged += TertiaryId_Change;
            inst.apparelLeftHandedCheckbox.Click += LeftHanded_Change;
            inst.apparelPoseBox.SelectionChanged += Pose_Change;
            inst.equipSlotBox.SelectionChanged += Equipped_Change;
            inst.regenerateGuidsButton.Click += RegenerateGuids_Click;
            inst.optionsMenuItem.Click += Options_Click;
            inst.saveAsExampleButton.Click += SaveAsExampleButton_Click;
            inst.visibilityCondsButton.Click += Char_EditConditions_Button_Click;
            inst.randomColorButton.Click += RandomColor_Click;
            inst.checkForUpdatesButton.Click += CheckForUpdates_Click;
            inst.exitButton.Click += ExitButtonClick;
            inst.RecentList.Click += RecentList_Click;
            inst.exportButton.Click += ExportClick;
            inst.saveButton.Click += SaveClick;
            inst.saveAsButton.Click += SaveAsClick;
            inst.loadButton.Click += LoadClick;
            inst.faceImageIndex.ValueChanged += FaceImageIndex_Changed;
            inst.beardImageIndex.ValueChanged += BeardImageIndex_Changed;
            inst.hairImageIndex.ValueChanged += HairImageIndex_Changed;
            inst.mainTabControl.SelectionChanged += TabControl_SelectionChanged;
            inst.colorSliderR.ValueChanged += ColorSliderChange;
            inst.colorSliderG.ValueChanged += ColorSliderChange;
            inst.colorSliderB.ValueChanged += ColorSliderChange;
            inst.aboutMenuItem.Click += AboutMenu_Click;
            inst.vkComm.Click += FeedbackItemClick;
            inst.discordComm.Click += FeedbackItemClick;
            inst.steamComm.Click += FeedbackItemClick;
            inst.patchNotesMenuItem.Click += WhatsNew_Menu_Click;
            inst.apparelSkinColorBox.TextChanged += ApparelSkinColorBox_TextChanged;
            inst.apparelHairColorBox.TextChanged += ApparelHairColorBox_TextChanged;
            inst.apparelHairRandomize.Click += ApparelHaircut_Random;
            inst.apparelBeardRandomize.Click += ApparelBeard_Random;
            inst.apparelFaceRandomize.Click += ApparelFace_Random;
            inst.userColorSaveButton.Click += UserColorList_AddColor;
            inst.switchToAnotherScheme.Click += ColorScheme_Switch;
            inst.colorHexOut.PreviewTextInput += ColorHex_Input;
            DataObject.AddPastingHandler(inst.colorHexOut, ColorHex_Pasted);
        }

        private MainWindow inst;

        #region EVENTS
        internal void ColorHex_Input(object sender, TextCompositionEventArgs e)
        {
            string text = inst.colorHexOut.Text;
            int cursorPos = inst.colorHexOut.SelectionStart;
            text = text.Insert(inst.colorHexOut.SelectionStart, e.Text);
            if ((text.StartsWith("#") && text.Length < 7) || text.Length < 6)
                return;
            var parseAble = NPCColor.CanParseHex(text);
            e.Handled = !parseAble;
            inst.userColorSaveButton.IsEnabled = parseAble;
            if (parseAble)
            {
                var color = new NPCColor() { HEX = text };
                inst.colorHexOut.Text = color.HEX;
                if (MainWindow.IsRGB)
                {
                    inst.colorSliderR.Value = color.R;
                    inst.colorSliderG.Value = color.G;
                    inst.colorSliderB.Value = color.B;
                }
                else
                {
                    var colorHSV = color.HSV;
                    inst.colorSliderR.Value = colorHSV.Item1;
                    inst.colorSliderG.Value = colorHSV.Item2;
                    inst.colorSliderB.Value = colorHSV.Item3;
                }
                inst.colorHexOut.SelectionStart = cursorPos + 1;
            }
        }
        internal void ColorHex_Pasted(object sender, DataObjectPastingEventArgs e)
        {
            if (e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true))
            {
                var parseAble = NPCColor.CanParseHex(e.SourceDataObject.GetData(DataFormats.UnicodeText) as string);
                e.Handled = !parseAble;
                inst.userColorSaveButton.IsEnabled = parseAble;
                if (parseAble)
                {
                    var color = new NPCColor() { HEX = (e.SourceDataObject.GetData(DataFormats.UnicodeText) as string) };
                    inst.colorHexOut.Text = color.HEX;
                    if (MainWindow.IsRGB)
                    {
                        inst.colorSliderR.Value = color.R;
                        inst.colorSliderG.Value = color.G;
                        inst.colorSliderB.Value = color.B;
                    }
                    else
                    {
                        var colorHSV = color.HSV;
                        inst.colorSliderR.Value = colorHSV.Item1;
                        inst.colorSliderG.Value = colorHSV.Item2;
                        inst.colorSliderB.Value = colorHSV.Item3;
                    }
                }
            }
            else
            {
                e.Handled = true;
                inst.userColorSaveButton.IsEnabled = false;
            }
        }
        internal void MistakeList_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (inst.lstMistakes.SelectedItem != null && inst.lstMistakes.SelectedItem is Mistake mist)
            {
                mist.OnClick?.Invoke();
            }
        }
        internal void EditorName_Change(object sender, TextChangedEventArgs e)
        {
            MainWindow.CurrentNPC.editorName = (sender as TextBox).Text;
            MainWindow.isSaved = false;
            if (MainWindow.DiscordWorker != null)
            {
                RichPresence presence = new RichPresence
                {
                    Details = $"Editing {(MainWindow.CurrentNPC.editorName == null || MainWindow.CurrentNPC.editorName.Length < 1 ? "NPC without name" : MainWindow.CurrentNPC.editorName)}",
                    State = $"Display Name: {(MainWindow.CurrentNPC.displayName == null || MainWindow.CurrentNPC.displayName.Length < 1 ? "None" : MainWindow.CurrentNPC.displayName)}"
                };
                presence.Timestamps = new Timestamps
                {
                    StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                };
                presence.Assets = new Assets
                {
                    SmallImageKey = "icon_info_outlined",
                    SmallImageText = "Info"
                };
                (MainWindow.DiscordWorker as DiscordRPC.DiscordWorker)?.SendPresence(presence);
            }
        }
        internal void DisplayName_Change(object sender, TextChangedEventArgs e)
        {
            MainWindow.CurrentNPC.displayName = (sender as TextBox).Text;
            if (MainWindow.DiscordWorker != null)
            {
                RichPresence presence = new RichPresence
                {
                    Details = $"Editing {(MainWindow.CurrentNPC.editorName == null || MainWindow.CurrentNPC.editorName.Length < 1 ? "NPC without name" : MainWindow.CurrentNPC.editorName)}",
                    State = $"Display Name: {(MainWindow.CurrentNPC.displayName == null || MainWindow.CurrentNPC.displayName.Length < 1 ? "None" : MainWindow.CurrentNPC.displayName)}"
                };
                presence.Timestamps = new Timestamps
                {
                    StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                };
                presence.Assets = new Assets
                {
                    SmallImageKey = "icon_info_outlined",
                    SmallImageText = "Info"
                };
                (MainWindow.DiscordWorker as DiscordRPC.DiscordWorker)?.SendPresence(presence);
            }
            MainWindow.isSaved = false;
        }
        internal void NPC_ID_Change(object sender, /*long e*/ RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.id = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void StartDialogue_ID_Change(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.startDialogueId = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void HatId_Change(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.hat = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void TopId_Change(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.top = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void BottomId_Change(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.bottom = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void MaskId_Change(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.mask = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void BackId_Change(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.backpack = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void VestId_Change(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.vest = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void GlassesId_Change(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.glasses = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void PrimaryId_Change(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.equipPrimary = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void SecondaryId_Change(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.equipSecondary = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void TertiaryId_Change(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                MainWindow.CurrentNPC.equipTertiary = (ushort)e.NewValue.Value;
                MainWindow.isSaved = false;
            }
        }
        internal void LeftHanded_Change(object sender, RoutedEventArgs e)
        {
            MainWindow.CurrentNPC.leftHanded = (sender as CheckBox).IsChecked.Value;
            MainWindow.isSaved = false;
        }
        internal void Pose_Change(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
                return;
            var item = (sender as ComboBox).SelectedItem as ComboBoxItem;
            if (item == null)
                return;
            var val = (NPC_Pose)item.Tag;
            MainWindow.CurrentNPC.pose = val;
            MainWindow.isSaved = false;
        }
        internal void Equipped_Change(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
                return;
            var item = (sender as ComboBox).SelectedItem as ComboBoxItem;
            if (item == null)
                return;
            var val = (Equip_Type)item.Tag;
            MainWindow.CurrentNPC.equipped = val;
            MainWindow.isSaved = false;
        }
        internal void RegenerateGuids_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentNPC.dialogues != null)
            {
                foreach (NPCDialogue d in MainWindow.CurrentNPC.dialogues)
                {
                    if (d != null)
                        d.guid = Guid.NewGuid().ToString("N");
                }
            }
            if (MainWindow.CurrentNPC.vendors != null)
            {
                foreach (NPCVendor v in MainWindow.CurrentNPC.vendors)
                {
                    if (v != null)
                        v.guid = Guid.NewGuid().ToString("N");
                }
            }
            if (MainWindow.CurrentNPC.quests != null)
            {
                foreach (NPCQuest q in MainWindow.CurrentNPC.quests)
                {
                    if (q != null)
                        q.guid = Guid.NewGuid().ToString("N");
                }
            }
            inst.DoNotification(MainWindow.Localize("general_Regenerated"));
            MainWindow.isSaved = false;
        }
        internal void Options_Click(object sender, RoutedEventArgs e)
        {
            Config.ConfigWindow cw = new Config.ConfigWindow();
            cw.ShowDialog();
        }
        internal void SaveAsExampleButton_Click(object sender, RoutedEventArgs e)
        {
            inst.Save(true);
        }
        internal void Char_EditConditions_Button_Click(object sender, RoutedEventArgs e)
        {
            Universal_ListView ulv = new Universal_ListView(MainWindow.CurrentNPC.visibilityConditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, false)).ToList(), Universal_ItemList.ReturnType.Condition);
            ulv.ShowDialog();
            MainWindow.CurrentNPC.visibilityConditions = ulv.Values.Cast<NPC.Condition>().ToList();
            MainWindow.isSaved = false;
        }
        internal void RandomColor_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.IsRGB)
            {
                byte[] colors = new byte[3];
                System.Security.Cryptography.RandomNumberGenerator.Create().GetBytes(colors);
                inst.colorSliderR.Value = colors[0];
                inst.colorSliderG.Value = colors[1];
                inst.colorSliderB.Value = colors[2];
            }
            else
            {
                inst.colorSliderR.Value = new Random().Next(360);
                inst.colorSliderG.Value = new Random().NextDouble();
                inst.colorSliderB.Value = new Random().NextDouble();
            }
        }   
        internal async void CheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            bool? isUpdateAvailable = await inst.IsUpdateAvailable();
            if (isUpdateAvailable.HasValue && isUpdateAvailable == true)
            {
                inst.DoNotification(MainWindow.Localize("app_Update_Available"));
            }
            else if (isUpdateAvailable.HasValue && isUpdateAvailable == false && !(sender is MainWindow))
            {
                inst.DoNotification(MainWindow.Localize("app_Update_Latest"));
            }
            else if (!isUpdateAvailable.HasValue)
            {
                inst.DoNotification(MainWindow.Localize("app_Update_Fail"));
            }
        }
        internal void UserColorListChanged()
        {
            inst.userColorSampleList.Children.Clear();
            if (Config.Configuration.Properties.userColors == null)
                return;
            BrushConverter brushConverter = new BrushConverter();
            foreach (string uColor in Config.Configuration.Properties.userColors)
            {
                if (uColor.StartsWith("#"))
                {
                    if (!brushConverter.IsValid(uColor))
                        continue;
                }
                else
                {
                    if (!brushConverter.IsValid($"#{uColor}"))
                        continue;
                }
                Grid g = new Grid();
                Border b = new Border
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Width = 16,
                    Height = 16,
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Black,
                    Background = brushConverter.ConvertFromString($"#{uColor}") as Brush
                };
                Label l = new Label
                {
                    Content = $"#{uColor}",
                    Margin = new Thickness(16, 0, 0, 0)
                };
                Button button = new Button()
                {
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Resources/ICON_CANCEL.png")),
                        Width = 16,
                        Height = 16
                    },
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 0, 10, 0),
                    ToolTip = MainWindow.Localize("apparel_User_Color_Remove")
                };
                Button copyButton = new Button
                {
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Resources/ICON_COPY.png")),
                        Width = 16,
                        Height = 16
                    },
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 0, 42, 0),
                    Tag = $"#{uColor}"
                };
                copyButton.Click += new RoutedEventHandler((sender, e) =>
                {
                    try
                    {
                        Button b1 = (sender as Button);
                        string toCopy = (string)b1.Tag;
                        Clipboard.SetText(toCopy);
                    }
                    catch { }
                });
                button.Click += UserColorList_RemoveColor;
                g.Children.Add(b);
                g.Children.Add(l);
                g.Children.Add(button);
                g.Children.Add(copyButton);
                inst.userColorSampleList.Children.Add(g);
            }
        }
        internal void UserColorList_RemoveColor(object sender, RoutedEventArgs e)
        {
            Grid g = Util.FindParent<Grid>(sender as Button);
            Label l = Util.FindChildren<Label>(g);
            string color = l.Content.ToString();
            Config.Configuration.Properties.userColors = Config.Configuration.Properties.userColors.Where(d => d != color.Trim('#')).ToArray();
            Config.Configuration.Save();
            UserColorListChanged();
        }
        internal void UserColorList_AddColor(object sender, RoutedEventArgs e)
        {
            if (Config.Configuration.Properties.userColors == null)
                Config.Configuration.Properties.userColors = new string[0];
            if (Config.Configuration.Properties.userColors.Length > 0 && Config.Configuration.Properties.userColors.Contains(inst.colorHexOut.Text.Trim('#')))
                return;
            List<string> uColors = Config.Configuration.Properties.userColors.ToList();
            if (uColors == null)
                uColors = new List<string>();
            uColors.Add(inst.colorHexOut.Text.Trim('#'));
            Config.Configuration.Properties.userColors = uColors.ToArray();
            Config.Configuration.Save();
            UserColorListChanged();
        }
        internal void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.isSaved)
            {
                if (!inst.SavePrompt())
                {
                    return;
                }
            }
            Config.Configuration.Save();
            if (MainWindow.CacheUpdated && MainWindow.CachedUnturnedFiles?.Count() > 0)
            {
                var res = MessageBox.Show(MainWindow.Localize("app_Cache_Save"), "", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    try // save cache
                    {
                        using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "unturnedCache.xml", FileMode.Create))
                        using (XmlWriter writer = XmlWriter.Create(fs))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(List<MainWindow.UnturnedFile>));
                            serializer.Serialize(writer, MainWindow.CachedUnturnedFiles);
                        }
                    }
                    catch { }
                }
            }
            (MainWindow.DiscordWorker as DiscordRPC.DiscordWorker)?.Deinitialize();
            Environment.Exit(0);
        }
        internal void RecentList_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.Equals(inst.RecentList))
                return;
            MenuItem m = e.OriginalSource as MenuItem;
            inst.Load(m.Header.ToString());
        }
        internal void ExportClick(object sender, RoutedEventArgs e)
        {
            inst.FindMistakes();
            if (inst.No_Exports > 0)
            {
                SystemSounds.Hand.Play();
                inst.mainTabControl.SelectedIndex = inst.mainTabControl.Items.Count - 1;
                return;
            }
            if (inst.Warnings > 0)
            {
                var res = MessageBox.Show(MainWindow.Localize("export_Warnings_Desc"), MainWindow.Localize("export_Warnings_Title"), MessageBoxButton.YesNo);
                if (!(res == MessageBoxResult.OK || res == MessageBoxResult.Yes))
                    return;
            }
            inst.Save();
            Export_ExportWindow eew = new Export_ExportWindow(AppDomain.CurrentDomain.BaseDirectory + $@"results\{MainWindow.CurrentNPC.editorName}\");
            eew.DoActions(MainWindow.CurrentNPC);
        }
        internal void NewButtonClick(object sender, RoutedEventArgs e)
        {
            if (!inst.SavePrompt())
                return;
            inst.ConvertNPCToState(new NPCSave());
            MainWindow.isSaved = true;
            MainWindow.Started = DateTime.UtcNow;
        }
        internal void SaveClick(object sender, RoutedEventArgs e)
        {
            inst.Save();
        }
        internal void SaveAsClick(object sender, RoutedEventArgs e)
        {
            MainWindow.oldFile = MainWindow.saveFile;
            MainWindow.saveFile = "";
            inst.Save();
            MainWindow.oldFile = "";
        }
        internal void LoadClick(object sender, RoutedEventArgs e)
        {
            string path = "";
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = $"{MainWindow.Localize("save_Filter")} (*.npc)|*.npc",
                Multiselect = false
            };
            var res = ofd.ShowDialog();
            if (res == true)
                path = ofd.FileName;
            else
                return;

            if (!inst.Load(path, false))
            {
                if (inst.Load(path, true, true))
                {
                    inst.notificationsStackPanel.Children.Clear();
                    inst.DoNotification(MainWindow.Localize("notify_Loaded"));
                }
            }
        }
        internal void FaceImageIndex_Changed(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            inst.faceImageControl.Source = ("Resources/Unturned/Faces/" + e.NewValue + ".png").GetImageSource();
            MainWindow.CurrentNPC.face = (byte)e.NewValue;
            MainWindow.isSaved = false;
        }
        internal void BeardImageIndex_Changed(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            foreach (UIElement ui in inst.beardRenderGrid.Children)
            {
                if (ui is Canvas c)
                {
                    c.Visibility = Visibility.Collapsed;
                }
            }
            inst.beardRenderGrid.Children[(int)e.NewValue].Visibility = Visibility.Visible;
            MainWindow.CurrentNPC.beard = (byte)e.NewValue;
            MainWindow.isSaved = false;
        }
        internal void HairImageIndex_Changed(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            foreach (UIElement ui in inst.hairRenderGrid.Children)
            {
                if (ui is Canvas c)
                {
                    c.Visibility = Visibility.Collapsed;
                }
            }
            inst.hairRenderGrid.Children[(int)e.NewValue].Visibility = Visibility.Visible;
            MainWindow.CurrentNPC.haircut = (byte)e.NewValue;
            MainWindow.isSaved = false;
        }
        internal void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e?.AddedItems.Count == 0 || sender == null)
                return;
            int selectedIndex = (sender as TabControl).SelectedIndex;
            TabItem tab = e?.AddedItems[0] as TabItem;
            if (tab?.Content is Grid g)
            {
                DoubleAnimation anim = new DoubleAnimation(0, 1, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                g.BeginAnimation(MainWindow.OpacityProperty, anim);
            }
            if (selectedIndex == (sender as TabControl).Items.Count - 1)
            {
                inst.FindMistakes();
            }
            RichPresence presence = new RichPresence
            {
                Details = $"Editing {(MainWindow.CurrentNPC.editorName == null || MainWindow.CurrentNPC.editorName.Length < 1 ? "NPC without name" : MainWindow.CurrentNPC.editorName)}",
                State = $"Display Name: {(MainWindow.CurrentNPC.displayName == null || MainWindow.CurrentNPC.displayName.Length < 1 ? "None" : MainWindow.CurrentNPC.displayName)}"
            };
            presence.Timestamps = new Timestamps();
            presence.Timestamps.StartUnixMilliseconds = (ulong)(MainWindow.Started.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            presence.Assets = new Assets();
            switch (selectedIndex)
            {
                case 0:
                    presence.Assets.SmallImageKey = "icon_info_outlined";
                    presence.Assets.SmallImageText = "Info";
                    break;
                case 1:
                    presence.Assets.SmallImageKey = "icon_apparel_outlined";
                    presence.Assets.SmallImageText = "Apparel";
                    break;
                case 2:
                    presence.Assets.SmallImageKey = "icon_chat_outlined";
                    presence.Assets.SmallImageText = $"Dialogues: {MainWindow.CurrentNPC.dialogues.Count}";
                    break;
                case 3:
                    presence.Assets.SmallImageKey = "icon_money_outlined";
                    presence.Assets.SmallImageText = $"Vendors: {MainWindow.CurrentNPC.vendors.Count}";
                    break;
                case 4:
                    presence.Assets.SmallImageKey = "icon_exclamation_outlined";
                    presence.Assets.SmallImageText = $"Quests: {MainWindow.CurrentNPC.quests.Count}";
                    break;
                case 5:
                    presence.Assets.SmallImageKey = "icon_warning_outlined";
                    presence.Assets.SmallImageText = $"Mistakes: {MainWindow.Instance.lstMistakes.Items.Count}";
                    break;
                default:
                    presence.Assets.SmallImageKey = "icon_question_outlined";
                    presence.Assets.SmallImageText = "Something?..";
                    break;
            }
            (MainWindow.DiscordWorker as DiscordRPC.DiscordWorker)?.SendPresence(presence);
        }
        internal void ColorSliderChange(object sender, RoutedPropertyChangedEventArgs<double> value)
        {
            NPCColor c;
            if (MainWindow.IsRGB)
                c = new NPCColor((byte)inst.colorSliderR.Value, (byte)inst.colorSliderG.Value, (byte)inst.colorSliderB.Value);
            else
                c = NPCColor.FromHSV((int)inst.colorSliderR.Value, inst.colorSliderG.Value, inst.colorSliderB.Value);
            string res = c.HEX;
            inst.colorRectangle.Fill = new BrushConverter().ConvertFromString(res) as Brush;
            inst.colorHexOut.Text = res;
            if (Config.Configuration.Properties.experimentalFeatures)
            {
                if (!MainWindow.IsRGB)
                {
                    var HSV = c.HSV;
                    // build first bar (Hue)
                    Slider senderSlider = sender as Slider ?? new Slider();
                    if (senderSlider.Name != inst.colorSliderR.Name)
                    {
                        List<GradientStop> stopsHue = new List<GradientStop>();
                        for (int k = 0; k <= 360; k++)
                        {
                            stopsHue.Add(new GradientStop(NPCColor.FromHSV(k, HSV.Item2, HSV.Item3).Color, k / 360d));
                        }
                        inst.colorSliderR.Background = new LinearGradientBrush(new GradientStopCollection(stopsHue), 0);
                    }
                    // build second bar (Saturation)
                    if (senderSlider.Name != inst.colorSliderG.Name)
                    {
                        List<GradientStop> stopsSatur = new List<GradientStop>();
                        for (double k = 0; k <= 1; k += 0.01)
                        {
                            stopsSatur.Add(new GradientStop(NPCColor.FromHSV(HSV.Item1, k, HSV.Item3).Color, k));
                        }
                        inst.colorSliderG.Background = new LinearGradientBrush(new GradientStopCollection(stopsSatur), 0);
                    }
                    // build third bar (Value)
                    if (senderSlider.Name != inst.colorSliderB.Name)
                    {
                        List<GradientStop> stopsValue = new List<GradientStop>();
                        for (double k = 0; k <= 1; k += 0.01)
                        {
                            stopsValue.Add(new GradientStop(NPCColor.FromHSV(HSV.Item1, HSV.Item2, k).Color, k));
                        }
                        inst.colorSliderB.Background = new LinearGradientBrush(new GradientStopCollection(stopsValue), 0);
                    }
                }
                else
                {
                    Slider senderSlider = sender as Slider ?? new Slider();
                    if (senderSlider.Name != inst.colorSliderR.Name)
                    {
                        List<GradientStop> stopsRed = new List<GradientStop>();
                        for (byte red = 0; red < 255; red++)
                        {
                            var clr = new NPCColor(red, c.G, c.B);
                            stopsRed.Add(new GradientStop(clr.Color, red / 255d));
                        }
                        inst.colorSliderR.Background = new LinearGradientBrush(new GradientStopCollection(stopsRed), 0);
                    }
                    if (senderSlider.Name != inst.colorSliderG.Name)
                    {
                        List<GradientStop> stopsGreen = new List<GradientStop>();
                        for (byte green = 0; green < 255; green++)
                        {
                            var clr = new NPCColor(c.R, green, c.B);
                            stopsGreen.Add(new GradientStop(clr.Color, green / 255d));
                        }
                        inst.colorSliderG.Background = new LinearGradientBrush(new GradientStopCollection(stopsGreen), 0);
                    }
                    if (senderSlider.Name != inst.colorSliderB.Name)
                    {
                        List<GradientStop> stopsBlue = new List<GradientStop>();
                        for (byte blue = 0; blue < 255; blue++)
                        {
                            var clr = new NPCColor(c.R, c.G, blue);
                            stopsBlue.Add(new GradientStop(clr.Color, blue / 255d));
                        }
                        inst.colorSliderB.Background = new LinearGradientBrush(new GradientStopCollection(stopsBlue), 0);
                    }
                }
            }
        }
        internal void ColorScheme_Switch(object sender, RoutedEventArgs e)
        {
            MainWindow.IsRGB = !MainWindow.IsRGB;
            NPCColor c;
            if (MainWindow.IsRGB)
            {
                c = NPCColor.FromHSV((int)inst.colorSliderR.Value, inst.colorSliderG.Value, inst.colorSliderB.Value);
                inst.colorSliderR.Value = 0;
                inst.colorSliderG.Value = 0;
                inst.colorSliderB.Value = 0;
                inst.colorSliderR.Minimum = 0;
                inst.colorSliderG.Minimum = 0;
                inst.colorSliderB.Minimum = 0;
                inst.colorSliderR.Maximum = 255;
                inst.colorSliderG.Maximum = 255;
                inst.colorSliderB.Maximum = 255;
                inst.colorSliderG.SmallChange = 1;
                inst.colorSliderG.LargeChange = 5;
                inst.colorSliderB.SmallChange = 1;
                inst.colorSliderB.LargeChange = 5;
                inst.colorSliderR.AutoToolTipPrecision = 0;
                inst.colorSliderG.AutoToolTipPrecision = 0;
                inst.colorSliderB.AutoToolTipPrecision = 0;
                inst.colorRLabel.Content = MainWindow.Localize("tool_Color_Red");
                inst.colorGLabel.Content = MainWindow.Localize("tool_Color_Green");
                inst.colorBLabel.Content = MainWindow.Localize("tool_Color_Blue");
                inst.colorRLabel.ToolTip = MainWindow.Localize("tool_Color_Red_Tip");
                inst.colorGLabel.ToolTip = MainWindow.Localize("tool_Color_Green_Tip");
                inst.colorBLabel.ToolTip = MainWindow.Localize("tool_Color_Blue_Tip");
                inst.switchToAnotherScheme.Content = MainWindow.Localize("tool_Color_SwitchTo_HSV");
                inst.colorSliderR.Value = c.R;
                inst.colorSliderG.Value = c.G;
                inst.colorSliderB.Value = c.B;
            }
            else
            {
                c = new NPCColor((byte)inst.colorSliderR.Value, (byte)inst.colorSliderG.Value, (byte)inst.colorSliderB.Value);
                inst.colorSliderR.Value = 0;
                inst.colorSliderG.Value = 0;
                inst.colorSliderB.Value = 0;
                inst.colorSliderR.Minimum = 0;
                inst.colorSliderG.Minimum = 0;
                inst.colorSliderB.Minimum = 0;
                inst.colorSliderR.Maximum = 359.99;
                inst.colorSliderG.Maximum = 1;
                inst.colorSliderB.Maximum = 1;
                inst.colorSliderG.AutoToolTipPrecision = 2;
                inst.colorSliderB.AutoToolTipPrecision = 2;
                inst.colorSliderG.SmallChange = 0.01;
                inst.colorSliderG.LargeChange = 0.1;
                inst.colorSliderB.SmallChange = 0.01;
                inst.colorSliderB.LargeChange = 0.1;
                inst.colorRLabel.Content = MainWindow.Localize("tool_Color_Hue");
                inst.colorGLabel.Content = MainWindow.Localize("tool_Color_Saturation");
                inst.colorBLabel.Content = MainWindow.Localize("tool_Color_Value");
                inst.colorRLabel.ToolTip = MainWindow.Localize("tool_Color_Hue_Tip");
                inst.colorGLabel.ToolTip = MainWindow.Localize("tool_Color_Saturation_Tip");
                inst.colorBLabel.ToolTip = MainWindow.Localize("tool_Color_Value_Tip");
                inst.switchToAnotherScheme.Content = MainWindow.Localize("tool_Color_SwitchTo_RGB");
                var cHSV = c.HSV;
                inst.colorSliderR.Value = cHSV.Item1;
                inst.colorSliderG.Value = cHSV.Item2;
                inst.colorSliderB.Value = cHSV.Item3;
            }
            ColorSliderChange(null, null);
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
                    new Whats_New(
                                                    MainWindow.Localize("app_News_Title"),
                                                    string.Format(MainWindow.Localize("app_News_BodyTitle"), MainWindow.Version),
                                                    wc.DownloadString($"https://raw.githubusercontent.com/iBowie/publicfiles/master/npcmakerpatch.{Config.Configuration.Properties.Language.Replace('-', '_')}.txt"),
                                                    MainWindow.Localize("app_News_OK")
                                                    ).ShowDialog();
                }
            }
            catch { }
        }
        internal void ApparelSkinColorBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = inst.apparelSkinColorBox.Text;
            BrushConverter bc = new BrushConverter();
            if (bc.IsValid(text))
            {
                Brush color = bc.ConvertFromString(text) as Brush;
                inst.faceImageBorder.Background = color;
                MainWindow.CurrentNPC.skinColor = new NPCColor() { HEX = text };
            }
            else
            {
                inst.faceImageBorder.Background = Brushes.Transparent;
            }
        }
        internal void ApparelHairColorBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = inst.apparelHairColorBox.Text;
            BrushConverter bc = new BrushConverter();
            if (bc.IsValid(text))
            {
                Brush color = bc.ConvertFromString(text) as Brush;
                inst.beardRenderGrid.DataContext = color;
                inst.hairRenderGrid.DataContext = color;
                MainWindow.CurrentNPC.hairColor = new NPCColor() { HEX = text };
            }
            else
            {
                inst.beardRenderGrid.DataContext = Brushes.Black;
                inst.hairRenderGrid.DataContext = Brushes.Black;
            }
        }
        internal void ApparelHaircut_Random(object sender, RoutedEventArgs e)
        {
            inst.hairImageIndex.Value = new Random().Next(0, MainWindow.haircutAmount);
        }
        internal void ApparelBeard_Random(object sender, RoutedEventArgs e)
        {
            inst.beardImageIndex.Value = new Random().Next(0, MainWindow.beardAmount);
        }
        internal void ApparelFace_Random(object sender, RoutedEventArgs e)
        {
            inst.faceImageIndex.Value = new Random().Next(0, MainWindow.faceAmount);
        }
        #endregion
    }
}
