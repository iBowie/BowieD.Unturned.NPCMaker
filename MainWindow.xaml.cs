#define BETA
//#define RELEASE

using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Media;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media;
using System.Globalization;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Forms;
using System.Windows.Media.Animation;
using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.BetterControls;

using System.Net; // ONLY FOR UPDATE CHECK (FOR THOSE WHO DECOMPILE MY APP (STOP DOING THIS THO))
using BowieD.Unturned.NPCMaker.Examples;
using DiscordRPC;
using System.Text;
// STILL ONLY FOR UPDATES

namespace BowieD.Unturned.NPCMaker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            Config.Configuration.Load();
            Config.Configuration.Save();
            #region LOCALIZATION
            App.LanguageChanged += App_LanguageChanged;
            languageMenuItem.Items.Clear();
            foreach (var lang in App.Languages)
            {
                MenuItem menuLang = new MenuItem
                {
                    Header = lang.NativeName,
                    Tag = lang,
                    IsChecked = lang.Equals(App.Language)
                };
                menuLang.Click += ChangeLanguageClick;
                languageMenuItem.Items.Add(menuLang);
            }
            App.Language = Config.Configuration.Properties.language ?? new CultureInfo("en-US");
            #endregion
            #region OPEN_WITH
            string[] args = Environment.GetCommandLineArgs();
            if (args != null && args.Length >= 0)
            {
                try
                {
                    for (int k = 0; k < args.Length; k++)
                    {
                        if (FileCompatible(args[k]))
                        {
                            if (!Load(args[k], false))
                            {
                                if (Load(args[k], true, true))
                                {
                                    notificationsStackPanel.Children.Clear();
                                    DoNotification((string)TryFindResource("notify_Loaded"));
                                }
                            }
                            break;
                        }
                    }
                }
                catch { }
            }
            #endregion
            #region PROPERTY SETUP
            if (Config.Configuration.Properties.recent == null)
                Config.Configuration.Properties.recent = new string[0];
            Config.Configuration.Properties.recent = Config.Configuration.Properties.recent.Where(d => File.Exists(d)).ToArray();
            RecentList.ItemsSource = Config.Configuration.Properties.recent;
            #endregion
            #region SCALE SETUP
            foreach (UIElement ui in scaleMenuItem.Items)
            {
                var w = (ui as MenuItem);
                if (double.Parse(w.Tag.ToString().Replace('.', ',')) == Config.Configuration.Properties.scale)
                {
                    ScaleUpdate(w, null);
                    break;
                }
            }
            #endregion
            #region APPAREL SETUP
            faceImageIndex.MaxValue = faceAmount - 1;
            FaceImageIndex_Changed(faceImageIndex.Value);
            beardImageIndex.MaxValue = beardAmount - 1;
            BeardImageIndex_Changed(beardImageIndex.Value);
            hairImageIndex.MaxValue = haircutAmount - 1;
            HairImageIndex_Changed(hairImageIndex.Value);
            #region COLOR SAMPLES
            #region UNTURNED
            List<string> unturnedColors = new List<string>()
            {
                "F4E6D2",
                "D9CAB4",
                "BEA582",
                "9D886B",
                "94764B",
                "706049",
                "534736",
                "4B3D31",
                "332C25",
                "231F1C",
                "D7D7D7",
                "C1C1C1",
                "CDC08C",
                "AC6A39",
                "665037",
                "57452F",
                "352C22",
                "373737",
                "191919"
            };
            BrushConverter brushConverter = new BrushConverter();
            foreach (string uColor in unturnedColors)
            {
                Grid g = new Grid();
                Border b = new Border
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Width = 16,
                    Height = 16,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Background = brushConverter.ConvertFromString($"#{uColor}") as Brush
                };
                Label l = new Label
                {
                    Content = $"#{uColor}",
                    Margin = new Thickness(16, 0, 0, 0)
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
                    Margin = new Thickness(0, 0, 0, 0),
                    Tag = $"#{uColor}"
                };
                copyButton.Click += CopyButton_Click;
                g.Children.Add(b);
                g.Children.Add(l);
                g.Children.Add(copyButton);
                unturnedColorSampleList.Children.Add(g);
            }
            #endregion
            UserColorListChanged();
            #endregion
            #endregion
            #region HOLIDAYS
            int day = DateTime.Now.Day;
            int month = DateTime.Now.Month;
            #region PERSONAL HOLIDAYS
            if (day == 22 && month == 3)
                DoNotification("Happy Birthday, BowieD!");
            if (day == 30 && month == 9)
                DoNotification("International Translation Day! Congratz, BowieD!");
            if (day == 30 && month == 10)
                DoNotification("Happy Birthday, DimesAO!");
            #endregion
            #region OFFICIAL HOLIDAYS
            if ((day == 1 && month == 1) || (day == 31 && month == 12))
                DoNotification("Happy New Year!");
            if (day == 14 && month == 2)
                DoNotification("Valentine's Day!");
            if (day == 1 && month == 4)
                DoNotification("April Fools!");
            if (day == 8 && month == 3)
                DoNotification("Have a nice day, women!");
            if (day == 20 && month == 3)
                DoNotification("Earth Day! Don't forget to do something good to Earth today!");
            if (day == 3 && month == 4)
                DoNotification("Sun Day! Say hi to our beatiful star!");
            if (day == 4 && month == 4)
                DoNotification("May the force be with you...");
            if (day == 11 && month == 1)
                DoNotification("Thank You!");
            #endregion
            #region COMMUNITY HOLIDAYS
            if (day == 7 && month == 5)
                DoNotification("Happy Birthday, Зефирка!");
            #endregion
            #endregion
            #region AFTER UPDATE
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "updater.exe"))
                {
                    Whats_New box = new Whats_New(
                        (string)TryFindResource("app_News_Title"),
                        string.Format((string)TryFindResource("app_News_BodyTitle"), version),
                        (string)TryFindResource("app_News_Text"),
                        (string)TryFindResource("app_News_OK")
                        );
                    box.ShowDialog();
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "updater.exe");
                }
            }
            catch { }
            #endregion
            #region AUTOSAVE INIT
            autosaveNPCCheckbox.IsChecked = Autosave_NPC_Enabled;
            autosaveDialoguesCheckbox.IsChecked = Autosave_Dialogues_Enabled;
            autosaveVendorsCheckbox.IsChecked = Autosave_Vendor_Enabled;
            autosaveQuestsCheckbox.IsChecked = Autosave_Quest_Enabled;
            #endregion
            #region VERSION SPECIFIC CODE
#if BETA
            //MessageBox.Show((string)TryFindResource("app_Beta"));
            DoNotification((string)TryFindResource("app_Beta"));
#endif
#if !BETA
            betaOverlayText.Visibility = Visibility.Collapsed;
#endif
#if !DEBUG
            debugOverlayText.Visibility = Visibility.Collapsed;
#endif
            #endregion
            #region EXAMPLE DEBUG
#if !DEBUG
            saveAsExampleButton.Visibility = Visibility.Collapsed;
#endif
            #endregion
            #region read cache
            try
            {
                using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "unturnedCache.xml", FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<UnturnedFile>));
                    CachedUnturnedFiles = serializer.Deserialize(reader) as List<UnturnedFile>;
                }
            }
            catch { }
            #endregion
            Config.Configuration.Properties.firstLaunch = false;
            isSaved = true;
            discordRichPresenceMenu.IsChecked = Config.Configuration.Properties.enableDiscord;
            if (Config.Configuration.Properties.enableDiscord)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "DiscordRPC.dll") && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Newtonsoft.Json.dll"))
                {
                    DiscordWorker = new DiscordRPC.DiscordWorker(1000);
                    (DiscordWorker as DiscordRPC.DiscordWorker)?.Initialize();
                    TabControl_SelectionChanged(mainTabControl, null);
                }
            }
        }

        #region CONSTANTS
        public const int 
        faceAmount = 32,
        beardAmount = 16,
        haircutAmount = 23,
        version = 18;
        #endregion
        #region STATIC
        public static MainWindow Instance;
        public static NPCSave CurrentNPC { get; set; }
        public static object DiscordWorker { get; set; }
        #endregion
        #region LOCALIZATION EVENTS
        private void ChangeLanguageClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi && mi.Tag is CultureInfo lang)
            {
                App.Language = lang;
            }
        }
        private void App_LanguageChanged(object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;
            foreach (MenuItem i in languageMenuItem.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;
                i.IsChecked = ci != null && ci.Equals(currLang);
            }
        }
        #endregion
        #region CURRENT NPC
        public static string saveFile = "";
        public static bool isSaved = true;
        public static List<NPCDialogue> dialogues = new List<NPCDialogue>();
        public static List<NPCQuest> quests = new List<NPCQuest>();
        public static List<NPCVendor> vendors = new List<NPCVendor>();
        public static List<NPC.Condition> visibilityConditions = new List<NPC.Condition>();
        public static NPCSave StateAsNPC => Instance.ConvertCurrentStateToNPC();
        #endregion
        #region SAVE_LOAD
        public void Save(bool asExample = false)
        {
            #if !DEBUG
            if ((CurrentNPC?.IsReadOnly) ?? false)
            {
                DoNotification((string)TryFindResource("notify_ReadOnly"));
                return;
            }
            #endif
            if (saveFile == null || saveFile == "")
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = $"{(string)TryFindResource("save_Filter")} (*.npc)|*.npc",
                    FileName = $"{ConvertCurrentStateToNPC().editorName}.npc",
                    OverwritePrompt = true
                };
                var result = sfd.ShowDialog();
                if (result == true)
                    saveFile = sfd.FileName;
                else
                    return;
            }
            if (Config.Configuration.Properties.recent == null)
                Config.Configuration.Properties.recent = new string[0];
            if (!Config.Configuration.Properties.recent.Contains(saveFile))
            {
                List<string> r = Config.Configuration.Properties.recent.ToList();
                r.Insert(0, saveFile);
                Config.Configuration.Properties.recent = r.ToArray();
                Config.Configuration.Save();
            }
            try
            {
                if (asExample)
                {
                    NPCExample example = new NPCExample(ConvertCurrentStateToNPC());
                    if (saveFile != null && saveFile != "")
                    {
                        using (FileStream fs = new FileStream(saveFile, FileMode.Create))
                        using (XmlWriter writer = XmlWriter.Create(fs))
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(NPCExample));
                            ser.Serialize(writer, example);
                        }
                        DoNotification("EXAMPLE SAVED!");
                        isSaved = true;
                    }
                }
                else
                {
                    #if !DEBUG
                    if ((CurrentNPC?.IsReadOnly) ?? false)
                    {
                        DoNotification((string)TryFindResource("notify_ReadOnly"));
                        return;
                    }
                    #endif
                    NPCSave save = ConvertCurrentStateToNPC();
                    if (saveFile != null && saveFile != "")
                    {
                        using (FileStream fs = new FileStream(saveFile, FileMode.Create))
                        using (XmlWriter writer = XmlWriter.Create(fs))
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(NPCSave));
                            ser.Serialize(writer, save);
                        }
                        DoNotification((string)TryFindResource("notify_Saved"));
                        isSaved = true;
                    }
                }
            }
            catch (Exception ex) { DoNotification($"Saving failed! Exception: {ex.Message}"); }
        }
        public bool Load(string path, bool asExample = false, bool skipPrompt = false)
        {
            if (!skipPrompt)
                if (!(CurrentNPC?.IsReadOnly).GetValueOrDefault())
                    if (!isSaved)
                        if (!SavePrompt())
                            return false;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    if (asExample)
                    {
                        XmlSerializer deser = new XmlSerializer(typeof(NPCExample));
                        NPCExample save = deser.Deserialize(reader) as NPCExample;
                        CurrentNPC = save;
                    }
                    else
                    {
                        XmlSerializer deser = new XmlSerializer(typeof(NPCSave));
                        NPCSave save = deser.Deserialize(reader) as NPCSave;
                        CurrentNPC = save;
                        saveFile = path;
                    }
                    if (CurrentNPC.version < version)
                    {
                        DoNotification((string)TryFindResource("load_Old"));
                    }
                    if (CurrentNPC.version > version)
                    {
                        DoNotification((string)TryFindResource("load_New"));
                    }
                    ConvertNPCToState(CurrentNPC);
                }
                DoNotification((string)TryFindResource("notify_Loaded"));
                return true;
            }
            catch { DoNotification((string)TryFindResource("load_Incompatible")); return false; }
        }
        public bool SavePrompt()
        {
            if ((CurrentNPC?.IsReadOnly).GetValueOrDefault())
            {
                return true;
            }
            if (Autosave_NPC_Enabled)
            {
                Save();
                return true;
            }
            var result = MessageBox.Show((string)TryFindResource("app_Exit_UnsavedChanges_Text"), (string)TryFindResource("app_Exit_UnsavedChanges_Title"), MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                Save();
                return true;
            }
            else if (result == MessageBoxResult.No)
            {
                return true;
            }
            return false;
        }
        public bool FileCompatible(string path)
        {
            if (path == null || path.Length == 0 || !File.Exists(path))
                return false;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (XmlReader reader = XmlReader.Create(fs))
                    {
                        XmlSerializer s = new XmlSerializer(typeof(NPC.NPCSave));
                        XmlSerializer s2 = new XmlSerializer(typeof(NPCExample));
                        return s.CanDeserialize(reader) || s2.CanDeserialize(reader);
                    }
                }
            }
            catch { return false; }
        }

        private void AutosaveNPCCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Autosave_NPC_Enabled = !Autosave_NPC_Enabled;
        }
        private void AutosaveDialoguesCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Autosave_Dialogues_Enabled = !Autosave_Dialogues_Enabled;
        }
        private void AutosaveVendorsCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Autosave_Vendor_Enabled = !Autosave_Vendor_Enabled;
        }
        private void AutosaveQuestsCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Autosave_Quest_Enabled = !Autosave_Quest_Enabled;
        }
#endregion
        #region EVENTS
        private void SaveAsExampleButton_Click(object sender, RoutedEventArgs e)
        {
            Save(true);
        }
        private void Char_EditConditions_Button_Click(object sender, RoutedEventArgs e)
        {
            Universal_ListView ulv = new Universal_ListView(visibilityConditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, false)).ToList(), Universal_ItemList.ReturnType.Condition);
            ulv.ShowDialog();
            visibilityConditions = ulv.Values.Cast<NPC.Condition>().ToList();
        }
        private void Donate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((string)TryFindResource("menu_Donate_Out"));
        }
        private void RandomColor_Click(object sender, RoutedEventArgs e)
        {
            byte[] colors = new byte[3];
            System.Security.Cryptography.RandomNumberGenerator.Create().GetBytes(colors);
            colorSliderR.Value = colors[0];
            colorSliderG.Value = colors[1];
            colorSliderB.Value = colors[2];
        }
        private async void CheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            bool isUpdateAvailable = await IsUpdateAvailable();
            if (updCache && isUpdateAvailable)
            {
                //MessageBox.Show((string)TryFindResource("app_Update_Available"));
                DoNotification((string)TryFindResource("app_Update_Available"));
            }
            else if (updCache && !isUpdateAvailable)
            {
                //MessageBox.Show((string)TryFindResource("app_Update_Latest"));
                DoNotification((string)TryFindResource("app_Update_Latest"));
            }
            else if (!updCache)
            {
                //MessageBox.Show((string)TryFindResource("app_Update_Fail"));
                DoNotification((string)TryFindResource("app_Update_Fail"));
            }
        }
        private void UserColorListChanged()
        {
            userColorSampleList.Children.Clear();
            if (Config.Configuration.Properties.userColors == null)
                return;
            BrushConverter brushConverter = new BrushConverter();
            foreach (string uColor in Config.Configuration.Properties.userColors)
            {
                try
                {
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
                        Width = 16,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        ToolTip = (string)TryFindResource("apparel_User_Color_Remove")
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
                        Margin = new Thickness(0, 0, 18, 0),
                        Tag = $"#{uColor}"
                    };
                    copyButton.Click += CopyButton_Click;
                    button.Click += UserColorList_RemoveColor;
                    g.Children.Add(b);
                    g.Children.Add(l);
                    g.Children.Add(button);
                    g.Children.Add(copyButton);
                    userColorSampleList.Children.Add(g);
                }
                catch { }
            }
        }
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = (sender as Button);
                string toCopy = (string)b.Tag;
                Clipboard.SetText(toCopy);
            }
            catch { }
        }
        private void UserColorList_RemoveColor(object sender, RoutedEventArgs e)
        {
            Grid g = Util.FindParent<Grid>(sender as Button);
            Label l = Util.FindChildren<Label>(g);
            string color = l.Content.ToString();
            Config.Configuration.Properties.userColors = Config.Configuration.Properties.userColors.Where(d => d != color.Trim('#')).ToArray();
            Config.Configuration.Save();
            UserColorListChanged();
        }
        private void UserColorList_AddColor(object sender, RoutedEventArgs e)
        {
            if (Config.Configuration.Properties.userColors == null)
                Config.Configuration.Properties.userColors = new string[0];
            if (Config.Configuration.Properties.userColors.Length > 0 && Config.Configuration.Properties.userColors.Contains(colorHexOut.Text.Trim('#')))
                return;
            List<string> uColors = Config.Configuration.Properties.userColors.ToList();
            if (uColors == null)
                uColors = new List<string>();
            uColors.Add(colorHexOut.Text.Trim('#'));
            Config.Configuration.Properties.userColors = uColors.ToArray();
            Config.Configuration.Save();
            UserColorListChanged();
        }
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isSaved)
            {
                if (!SavePrompt())
                {
                    return;
                }
            }
            Config.Configuration.Save();
            if (CacheUpdated && CachedUnturnedFiles?.Count() > 0)
            {
                var res = MessageBox.Show((string)TryFindResource("app_Cache_Save"), "", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    try // save cache
                    {
                        using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "unturnedCache.xml", FileMode.Create))
                        using (XmlWriter writer = XmlWriter.Create(fs))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(List<UnturnedFile>));
                            serializer.Serialize(writer, CachedUnturnedFiles);
                        }
                    }
                    catch { }
                }
            }
            (DiscordWorker as DiscordRPC.DiscordWorker)?.Deinitialize();
            Environment.Exit(0);
        }
        private void RecentList_Click(object sender, RoutedEventArgs e)
        {
            MenuItem m = e.OriginalSource as MenuItem;
            Load(m.Header.ToString());
        }
        private void ExportClick(object sender, RoutedEventArgs e)
        {
            if (No_Exports > 0)
            {
                SystemSounds.Hand.Play();
                mainTabControl.SelectedIndex = mainTabControl.Items.Count - 1;
                return;
            }
            if (Warnings > 0)
            {
                var res = MessageBox.Show((string)FindResource("export_Warnings_Desc"), (string)FindResource("export_Warnings_Title"), MessageBoxButton.YesNo);
                if (!(res == MessageBoxResult.OK || res == MessageBoxResult.Yes))
                    return;
            }
            if (Autosave_Dialogues_Enabled && CurrentDialogue.id > 0)
                Dialogue_SaveButtonClick(null, null);
            if (Autosave_Quest_Enabled && CurrentQuest.id > 0)
                SaveQuest_Click(null, null);
            if (Autosave_Vendor_Enabled && CurrentVendor.id > 0)
                SaveVendor_Click(null, null);
            if (Autosave_NPC_Enabled && saveFile?.Length > 0)
                Save();
            Export_ExportWindow eew = new Export_ExportWindow(AppDomain.CurrentDomain.BaseDirectory + $@"results\{Inputted_EditorName}\");
            eew.DoActions(ConvertCurrentStateToNPC());
        }
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            Save();
        }
        private void DiscordRichPresenceMenu_Click(object sender, RoutedEventArgs e)
        {
            (sender as MenuItem).IsChecked = !(sender as MenuItem).IsChecked;
            Config.Configuration.Properties.enableDiscord = (sender as MenuItem).IsChecked;
            Config.Configuration.Save();
            DoNotification((string)TryFindResource("menu_Discord_Click"));
        }
        private void SaveAsClick(object sender, RoutedEventArgs e)
        {
            saveFile = "";
            Save();
        }
        private void LoadClick(object sender, RoutedEventArgs e)
        {
            string path = "";
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = $"{(string)TryFindResource("save_Filter")} (*.npc)|*.npc",
                Multiselect = false
            };
            var res = ofd.ShowDialog();
            if (res == true)
                path = ofd.FileName;
            else
                return;

            if (!Load(path, false))
            {
                if (Load(path, true, true))
                {
                    notificationsStackPanel.Children.Clear();
                    DoNotification((string)TryFindResource("notify_Loaded"));
                }
            }
        }
        private void FaceImageIndex_Changed(int value)
        {
            faceImageControl.Source = ("Resources/Unturned/Faces/" + value + ".png").GetImageSource();
            isSaved = false;
        }
        private void BeardImageIndex_Changed(int value)
        {
            beardImageControl.Source = ("Resources/Unturned/Beards/" + value + ".png").GetImageSource();
            isSaved = false;
        }
        private void HairImageIndex_Changed(int value)
        {
            hairImageControl.Source = ("Resources/Unturned/Hairs/" + value + ".png").GetImageSource();
            isSaved = false;
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
                g.BeginAnimation(OpacityProperty, anim);
            }
            if (selectedIndex == (sender as TabControl).Items.Count - 1)
            {
                FindMistakes();
            }
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {Inputted_EditorName ?? ""}",
                State = $"Current job:"
            };
            switch (selectedIndex)
            {
                case 0:
                    presence.State += " General Information";
                    break;
                case 1:
                    presence.State += " Apparel";
                    break;
                case 2:
                    presence.State += " Dialogues";
                    break;
                case 3:
                    presence.State += " Vendors";
                    break;
                case 4:
                    presence.State += " Quests";
                    break;
                case 5:
                    presence.State += " Looking for mistakes";
                    break;
                default:
                    presence.State += " Unknown";
                    break;
            }
            (MainWindow.DiscordWorker as DiscordRPC.DiscordWorker)?.SendPresence(presence);
        }
        private void ScaleUpdate(object sender, RoutedEventArgs e)
        {
            double d = double.Parse((sender as MenuItem).Tag.ToString().Replace('.', ','));
            mainGridScale.ScaleX = d;
            mainGridScale.ScaleY = d;
            Width = MinWidth * d;
            Height = MinHeight * d;
            foreach (UIElement ui in scaleMenuItem.Items)
            {
                (ui as MenuItem).IsChecked = false;
            }
            (sender as MenuItem).IsChecked = true;
            Config.Configuration.Properties.scale = d;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            Config.Configuration.Save();
            e.Cancel = true;
            ExitButtonClick(this, null);
            base.OnClosing(e);
        }
        private void ColorSliderChange(object sender, RoutedPropertyChangedEventArgs<double> value)
        {
            NPCColor c = new NPCColor((byte)colorSliderR.Value, (byte)colorSliderG.Value, (byte)colorSliderB.Value);
            string res = c.HEX;
            colorRectangle.Fill = new BrushConverter().ConvertFromString(res) as Brush;
            colorHexOut.Text = res;
        }
        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            Forms.Form_About a = new Forms.Form_About();
            a.ShowDialog();
        }
        private void FeedbackItemClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start((sender as MenuItem).Tag.ToString());
        }
        private void WhatsNew_Menu_Click(object sender, RoutedEventArgs e)
        {
            Whats_New box = new Whats_New(
                        (string)TryFindResource("app_News_Title"),
                        string.Format((string)TryFindResource("app_News_BodyTitle"), version),
                        (string)TryFindResource("app_News_Text"),
                        (string)TryFindResource("app_News_OK")
                        );
            box.ShowDialog();
        }
        #endregion
        #region STATE CONVERTERS
        public NPCSave ConvertCurrentStateToNPC()
        {
            NPCSave npc = new NPCSave
            {
                face = (byte)faceImageIndex.Value,
                beard = (byte)beardImageIndex.Value,
                haircut = (byte)hairImageIndex.Value,
                //saveFile = saveFile,
                id = Inputted_ID,
                editorName = Inputted_EditorName,
                displayName = Inputted_DisplayName,
                dialogues = dialogues,
                vendors = vendors,
                quests = quests,
                version = version,
                startDialogueId = Inputted_StartDialogueID,
                hat = Inputted_Equip_Hat,
                top = Inputted_Equip_Top,
                bottom = Inputted_Equip_Bottom,
                glasses = Inputted_Equip_Glasses,
                backpack = Inputted_Equip_Backpack,
                mask = Inputted_Equip_Mask,
                vest = Inputted_Equip_Vest,
                leftHanded = apparelLeftHandedCheckbox.IsChecked.Value,
                pose = (NPC_Pose)(apparelPoseBox.SelectedItem as ComboBoxItem).Tag,
                equipped = equipSlotBox.SelectedIndex > -1 ? (Equip_Type)(equipSlotBox.SelectedItem as ComboBoxItem).Tag : Equip_Type.None,
                hairColor = new NPCColor() { HEX = apparelHairColorBox.Text },
                skinColor = new NPCColor() { HEX = apparelSkinColorBox.Text },
                equipPrimary = Inputted_Equip_Primary,
                equipSecondary = Inputted_Equip_Secondary,
                equipTertiary = Inputted_Equip_Tertiary,
                visibilityConditions = visibilityConditions
            };
            return npc;
        }
        public void ConvertNPCToState(NPCSave save)
        {
            apparelSkinColorBox.Text = save.skinColor.HEX;
            apparelHairColorBox.Text = save.hairColor.HEX;
            Inputted_Equip_Backpack = save.backpack;
            Inputted_Equip_Mask = save.mask;
            Inputted_Equip_Vest = save.vest;
            Inputted_Equip_Top = save.top;
            Inputted_Equip_Bottom = save.bottom;
            Inputted_Equip_Glasses = save.glasses;
            Inputted_Equip_Hat = save.hat;
            Inputted_Equip_Primary = save.equipPrimary;
            Inputted_Equip_Secondary = save.equipSecondary;
            Inputted_Equip_Tertiary = save.equipTertiary;
            faceImageIndex.Value = save.face;
            beardImageIndex.Value = save.beard;
            hairImageIndex.Value = save.haircut;
            Inputted_ID = save.id;
            Inputted_EditorName = save.editorName;
            Inputted_DisplayName = save.displayName;
            dialogues = save.dialogues;
            vendors = save.vendors;
            quests = save.quests;
            //saveFile = save.saveFile;
            Inputted_StartDialogueID = save.startDialogueId;
            apparelLeftHandedCheckbox.IsChecked = save.leftHanded;
            visibilityConditions = save.visibilityConditions;
            foreach (var i in equipSlotBox.Items)
            {
                if ((Equip_Type)(i as ComboBoxItem).Tag == save.equipped)
                {
                    equipSlotBox.SelectedItem = i;
                    break;
                }
            }
            foreach (var i in apparelPoseBox.Items)
            {
                if ((NPC_Pose)(i as ComboBoxItem).Tag == save.pose)
                {
                    apparelPoseBox.SelectedItem = i;
                    break;
                }
            }
            CurrentNPC = save;
        }
#endregion
        #region PROPERTIES
        public ushort Inputted_ID
        {
            get
            {
                return (ushort)txtID.Value;
            }
            set
            {
                txtID.Value = value;
            }
        }
        public string Inputted_DisplayName
        {
            get => txtDisplayName.Text ?? "";
            set
            {
                txtDisplayName.Text = value;
            }
        }
        public string Inputted_EditorName
        {
            get => txtEditorName.Text ?? "";
            set
            {
                txtEditorName.Text = value;
            }
        }
        public ushort Inputted_StartDialogueID
        {
            get
            {
                return (ushort)txtStartDialogueID.Value;
            }
            set
            {
                txtStartDialogueID.Value = value;
            }
        }

        public ushort Inputted_Equip_Hat
        {
            get
            {
                return (ushort)hatIdBox.Value;
            }
            set
            {
                hatIdBox.Value = value;
            }
        }
        public ushort Inputted_Equip_Mask
        {
            get
            {
                return (ushort)maskIdBox.Value;
            }
            set
            {
                maskIdBox.Value = value;
            }
        }
        public ushort Inputted_Equip_Glasses
        {
            get
            {
                return (ushort)glassesIdBox.Value;
            }
            set
            {
                glassesIdBox.Value = value;
            }
        }
        public ushort Inputted_Equip_Backpack
        {
            get
            {
                return (ushort)backpackIdBox.Value;
            }
            set
            {
                backpackIdBox.Value = value;
            }
        }
        public ushort Inputted_Equip_Vest
        {
            get
            {
                return (ushort)vestIdBox.Value;
            }
            set
            {
                vestIdBox.Value = value;
            }
        }
        public ushort Inputted_Equip_Top
        {
            get
            {
                return (ushort)topIdBox.Value;
            }
            set
            {
                topIdBox.Value = value;
            }
        }
        public ushort Inputted_Equip_Bottom
        {
            get
            {
                return (ushort)bottomIdBox.Value;
            }
            set
            {
                bottomIdBox.Value = value;
            }
        }
        public ushort Inputted_Equip_Primary
        {
            get
            {
                return (ushort)primaryIdBox.Value;
            }
            set
            {
                primaryIdBox.Value = value;
            }
        }
        public ushort Inputted_Equip_Secondary
        {
            get
            {
                return (ushort)secondaryIdBox.Value;
            }
            set
            {
                secondaryIdBox.Value = value;
            }
        }
        public ushort Inputted_Equip_Tertiary
        {
            get
            {
                return (ushort)tertiaryIdBox.Value;
            }
            set
            {
                tertiaryIdBox.Value = value;
            }
        }

        // 0 - NPC
        // 1 - Dialogues
        // 2 - Vendor
        // 3 - Quest

        public bool Autosave_NPC_Enabled
        {
            get
            {
                if (Config.Configuration.Properties.autosaveParams?.Length != 4)
                    Config.Configuration.Properties.autosaveParams = new bool[4];
                return Config.Configuration.Properties.autosaveParams[0];
            }
            set
            {
                if (Config.Configuration.Properties.autosaveParams?.Length != 4)
                    Config.Configuration.Properties.autosaveParams = new bool[4];
                Config.Configuration.Properties.autosaveParams[0] = value;
                autosaveNPCCheckbox.IsChecked = value;
            }
        }
        public bool Autosave_Dialogues_Enabled
        {
            get
            {
                if (Config.Configuration.Properties.autosaveParams?.Length != 4)
                    Config.Configuration.Properties.autosaveParams = new bool[4];
                return Config.Configuration.Properties.autosaveParams[1];
            }
            set
            {
                if (Config.Configuration.Properties.autosaveParams?.Length != 4)
                    Config.Configuration.Properties.autosaveParams = new bool[4];
                Config.Configuration.Properties.autosaveParams[1] = value;
                autosaveDialoguesCheckbox.IsChecked = value;
            }
        }
        public bool Autosave_Vendor_Enabled
        {
            get
            {
                if (Config.Configuration.Properties.autosaveParams?.Length != 4)
                    Config.Configuration.Properties.autosaveParams = new bool[4];
                return Config.Configuration.Properties.autosaveParams[2];
            }
            set
            {
                if (Config.Configuration.Properties.autosaveParams?.Length != 4)
                    Config.Configuration.Properties.autosaveParams = new bool[4];
                Config.Configuration.Properties.autosaveParams[2] = value;
                autosaveVendorsCheckbox.IsChecked = value;
            }
        }
        public bool Autosave_Quest_Enabled
        {
            get
            {
                if (Config.Configuration.Properties.autosaveParams?.Length != 4)
                    Config.Configuration.Properties.autosaveParams = new bool[4];
                return Config.Configuration.Properties.autosaveParams[3];
            }
            set
            {
                if (Config.Configuration.Properties.autosaveParams?.Length != 4)
                    Config.Configuration.Properties.autosaveParams = new bool[4];
                Config.Configuration.Properties.autosaveParams[3] = value;
                autosaveQuestsCheckbox.IsChecked = value;
            }
        }
#endregion
        #region MISTAKES
        public void FindMistakes()
        {
            lstMistakes.Items.Clear();
            List<Mistake> foundMistakes = CheckableMistakes.Where(d => d.IsMistake).ToList();
            foreach (Mistake m in foundMistakes)
            {
                lstMistakes.Items.Add(m);
            }
        }
        public List<Mistake> CheckableMistakes
        {
            get
            {
                return new List<Mistake>()
                {
                    new Mistakes.General.TooShortDisplayName(),
                    new Mistakes.General.NonEnglishCharsEditorName(),
                    new Mistakes.General.AssignmentOfficial(),
                    new Mistakes.Dialogue.EmptyMessage(),
                    new Mistakes.Dialogue.QuestSetting()
                };
            }
        }
        public int Advices => CheckableMistakes.Where(d => d.Importance == IMPORTANCE.ADVICE && d.IsMistake).Count();
        public int Warnings => CheckableMistakes.Where(d => d.Importance == IMPORTANCE.HIGH && d.IsMistake).Count();
        public int No_Exports => CheckableMistakes.Where(d => d.Importance == IMPORTANCE.NO_EXPORT && d.IsMistake).Count();
        private async void DA_Button_Click(object sender, RoutedEventArgs e)
        {
            bool skipCache = false;
            if (CachedUnturnedFiles?.Count() > 0)
            {
                var res = MessageBox.Show((string)TryFindResource("mistakes_DA_UpdateCache"), "", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Cancel)
                {
                    blockActionsOverlay.Visibility = Visibility.Collapsed;
                    return;
                }
                if (res == MessageBoxResult.No)
                    skipCache = true;
            }
            if (!skipCache)
            {
                blockActionsOverlay.Visibility = Visibility.Visible;
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog
                {
                    ShowNewFolderButton = false
                };
                var res = fbd.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    blockActionsOverlay.Visibility = Visibility.Collapsed;
                    return;
                }
                if (!File.Exists(fbd.SelectedPath + @"\Unturned.exe"))
                {
                    blockActionsOverlay.Visibility = Visibility.Collapsed;
                    return;
                }
                await CacheFiles(fbd.SelectedPath);
                blockActionsOverlay.Visibility = Visibility.Collapsed;
            }
            FindMistakes();
            foreach (NPCDialogue dialogue in dialogues)
            {
                if (CachedUnturnedFiles.Any(d => d.Type == EAssetType.Dialogue && d.Id == dialogue.id))
                {
                    lstMistakes.Items.Add(new Mistakes.Generic(string.Format((string)TryFindResource("deep_dialogue"), dialogue.id), "", IMPORTANCE.HIGH, true)
                    {
                        TranslateName = false
                    });
                }
                await Task.Yield();
            }
            foreach (NPCVendor vendor in vendors)
            {
                if (CachedUnturnedFiles.Any(d => d.Type == EAssetType.Vendor && d.Id == vendor.id))
                {
                    lstMistakes.Items.Add(new Mistakes.Generic(string.Format((string)TryFindResource("deep_vendor"), vendor.id), "", IMPORTANCE.HIGH, true)
                    {
                        TranslateName = false
                    });
                }
                foreach (var it in vendor.items)
                {
                    if (it.type == ItemType.VEHICLE && !CachedUnturnedFiles.Any(d => d.Type == EAssetType.Vehicle && d.Id == it.id))
                    {
                        lstMistakes.Items.Add(new Mistakes.Generic(string.Format((string)TryFindResource("deep_vehicle"), it.id), "", IMPORTANCE.HIGH, true)
                        {
                            TranslateName = false
                        });
                        continue;
                    }
                    if (it.type == ItemType.ITEM && !CachedUnturnedFiles.Any(d => d.Type == EAssetType.Item && d.Id == it.id))
                    {
                        lstMistakes.Items.Add(new Mistakes.Generic(string.Format((string)TryFindResource("deep_item"), it.id), "", IMPORTANCE.HIGH, true)
                        {
                            TranslateName = false
                        });
                        continue;
                    }
                }
                await Task.Yield();
            }
            foreach (NPCQuest quest in quests)
            {
                if (CachedUnturnedFiles.Any(d => d.Type == EAssetType.Quest && d.Id == quest.id))
                {
                    lstMistakes.Items.Add(new Mistakes.Generic(string.Format((string)TryFindResource("deep_quest"), quest.id), "", IMPORTANCE.HIGH, true)
                    {
                        TranslateName = false
                    });
                }
                await Task.Yield();
            }
            if (Inputted_ID > 0)
            {
                ushort input = Inputted_ID;
                if (CachedUnturnedFiles.Any(d => d.Type == EAssetType.NPC && d.Id == input))
                {
                    lstMistakes.Items.Add(new Mistakes.Generic(string.Format((string)TryFindResource("deep_char"), input), "", IMPORTANCE.HIGH, true)
                    {
                        TranslateName = false
                    });
                }
            }
            blockActionsOverlay.Visibility = Visibility.Collapsed;
        }
#endregion
        #region DIALOGUE_EDITOR
        private void Dialogue_SaveButtonClick(object sender, RoutedEventArgs e)
        {
            var dil = CurrentDialogue;
            if (dil.id == 0)
            {
                DoNotification((string)TryFindResource("dialogue_ID_Zero"));
                return;
            }
            var o = dialogues.Where(d => d.id == dil.id);
            if (o.Count() > 0)
                dialogues.Remove(o.ElementAt(0));
            dialogues.Add(CurrentDialogue);
            DoNotification((string)TryFindResource("notify_Dialogue_Saved"));
            if (Autosave_NPC_Enabled && saveFile?.Length > 0)
                Save();
        }
        private void Dialogue_ClearCurrentButtonClick(object sender, RoutedEventArgs e)
        {
            List<UIElement> toRemove = new List<UIElement>();
            foreach (UIElement item in messagePagesGrid.Children)
            {
                if (item is Dialogue_Message)
                {
                    toRemove.Add(item);
                }
            }
            foreach (UIElement item in toRemove)
            {
                messagePagesGrid.Children.Remove(item);
            }
            toRemove = new List<UIElement>();
            foreach (UIElement item in dialoguePlayerRepliesGrid.Children)
            {
                if (item is Dialogue_Response)
                {
                    toRemove.Add(item);
                }
            }
            foreach (UIElement item in toRemove)
            {
                dialoguePlayerRepliesGrid.Children.Remove(item);
            }
        }
        private void Dialogue_OpenButtonClick(object sender, RoutedEventArgs e)
        {
            var ulv = new Universal_ListView(dialogues.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Dialogue, false)).ToList(), Universal_ItemList.ReturnType.Dialogue);
            if (ulv.ShowDialog() == true)
            {
                if (Autosave_Dialogues_Enabled && CurrentDialogue.id > 0)
                    Dialogue_SaveButtonClick(sender, e);
                CurrentDialogue = ulv.SelectedValue as NPCDialogue;
            }
            dialogues = ulv.Values.Cast<NPCDialogue>().ToList();
        }
        private void Dialogue_AddMessageClick(object sender, RoutedEventArgs e)
        {
            var o = new Dialogue_Message(new NPCMessage() { pages = new List<string>() });
            o.deletePageButton.Click += Dialogue_RemoveMessageClick;
            int ind = 0;
            for (int k = 0; k < messagePagesGrid.Children.Count; k++)
            {
                if (messagePagesGrid.Children[k] is Dialogue_Message)
                    ind = k + 1;
            }
            messagePagesGrid.Children.Insert(ind, o);
            if (Autosave_Dialogues_Enabled && CurrentDialogue.id > 0)
                Dialogue_SaveButtonClick(sender, e);
        }
        private void Dialogue_AddReplyClick(object sender, RoutedEventArgs e)
        {
            var o = new Dialogue_Response();
            o.deleteButton.Click += Dialogue_RemoveReplyClick;
            int ind = 0;
            for (int k = 0; k < dialoguePlayerRepliesGrid.Children.Count; k++)
            {
                if (dialoguePlayerRepliesGrid.Children[k] is Dialogue_Response)
                {
                    ind = k + 1;
                }
            }
            dialoguePlayerRepliesGrid.Children.Insert(ind, o);
            foreach (UIElement uie in dialoguePlayerRepliesGrid.Children)
            {
                if (uie is Dialogue_Response dr)
                {
                    dr.UpdateOrderButtons();
                }
            }
            if (Autosave_Dialogues_Enabled && CurrentDialogue.id > 0)
                Dialogue_SaveButtonClick(sender, e);
        }
        private void Dialogue_RemoveReplyClick(object sender, RoutedEventArgs e)
        {
            Dialogue_Response ans = Util.FindParent<Dialogue_Response>(sender as Button);
            dialoguePlayerRepliesGrid.Children.Remove(ans);
            foreach (UIElement uie in dialoguePlayerRepliesGrid.Children)
            {
                if (uie is Dialogue_Response dr)
                {
                    dr.UpdateOrderButtons();
                }
            }
            if (Autosave_Dialogues_Enabled && CurrentDialogue.id > 0)
                Dialogue_SaveButtonClick(sender, e);
        }
        private void Dialogue_RemoveMessageClick(object sender, RoutedEventArgs e)
        {
            Dialogue_Message pag = Util.FindParent<Dialogue_Message>(sender as Button);
            messagePagesGrid.Children.Remove(pag);
            if (Autosave_Dialogues_Enabled && CurrentDialogue.id > 0)
                Dialogue_SaveButtonClick(sender, e);
        }
        private void SetAsStart_Button_Click(object sender, RoutedEventArgs e)
        {
            Dialogue_SaveButtonClick(null, null);
            var dial = CurrentDialogue;
            if (dial.id > 0 && Inputted_StartDialogueID != dial.id)
            {
                Inputted_StartDialogueID = dial.id;
                try
                {
                    DoNotification(string.Format((string)TryFindResource("dialogue_Start_Notify"), dial.id));
                }
                catch { }
            }
        }
        public NPCDialogue CurrentDialogue
        {
            get
            {
                ushort dialogueID = (ushort)dialogueInputIdControl.Value;
                NPCDialogue ret = new NPCDialogue();
                List<Dialogue_Message> messages = new List<Dialogue_Message>();
                foreach (UIElement ui in messagePagesGrid.Children)
                {
                    if (ui is Dialogue_Message dm)
                    {
                        messages.Add(dm);
                    }
                }
                List<Dialogue_Response> responses = new List<Dialogue_Response>();
                foreach (UIElement ui in dialoguePlayerRepliesGrid.Children)
                {
                    if (ui is Dialogue_Response dr)
                    {
                        responses.Add(dr);
                    }
                }

                ret.messages = messages.Select(d => d.Message).ToList();
                ret.responses = responses.Select(d => d.Response).ToList();
                ret.id = dialogueID;
                ret.comment = dialogue_commentbox.Text;
                return ret;
            }
            set
            {
                Dialogue_ClearCurrentButtonClick(null, null);
                NPCDialogue d = value;
                dialogueInputIdControl.Value = d.id;
                foreach (NPCResponse response in d.responses)
                {
                    Dialogue_Response dialogue_Response = new Dialogue_Response(response);
                    dialogue_Response.deleteButton.Click += Dialogue_RemoveReplyClick;
                    int ind = 0;
                    for (int k = 0; k < dialoguePlayerRepliesGrid.Children.Count; k++)
                    {
                        if (dialoguePlayerRepliesGrid.Children[k] is Dialogue_Response)
                        {
                            ind = k + 1;
                        }
                    }
                    dialoguePlayerRepliesGrid.Children.Insert(ind, dialogue_Response);
                }
                foreach (UIElement uie in dialoguePlayerRepliesGrid.Children)
                {
                    if (uie is Dialogue_Response dr)
                    {
                        dr.UpdateOrderButtons();
                    }
                }
                foreach (NPCMessage message in d.messages)
                {
                    Dialogue_Message dialogue_Message = new Dialogue_Message(message);
                    dialogue_Message.deletePageButton.Click += Dialogue_RemoveMessageClick;
                    int ind = 0;
                    for (int k = 0; k < messagePagesGrid.Children.Count; k++)
                    {
                        if (messagePagesGrid.Children[k] is Dialogue_Message)
                            ind = k + 1;
                    }
                    messagePagesGrid.Children.Insert(ind, dialogue_Message);
                }
                dialogue_commentbox.Text = d.comment;
            }
        }
        #endregion
        #region VENDOR_EDITOR
        public NPCVendor CurrentVendor
        {
            get
            {
                List<VendorItem> items = new List<VendorItem>();
                foreach (UIElement ui in vendorListBuyItems.Children)
                {
                    items.Add((ui as Universal_ItemList).Value as VendorItem);
                }
                foreach (UIElement ui in vendorListSellItems.Children)
                {
                    items.Add((ui as Universal_ItemList).Value as VendorItem);
                }
                return new NPCVendor()
                {
                    items = items,
                    id = (ushort)vendorIdTxtBox.Value,
                    vendorDescription = vendorDescTxtBox.Text,
                    vendorTitle = vendorTitleTxtBox.Text,
                    comment = vendor_commentbox.Text
                };
            }
            set
            {
                ClearVendor_Click(null, null);
                foreach (VendorItem item in value.BuyItems)
                {
                    Universal_ItemList uil = new Universal_ItemList(item, Universal_ItemList.ReturnType.VendorItem, false);
                    uil.deleteButton.Click += DeleteVendorBuy_Click;
                    vendorListBuyItems.Children.Add(uil);
                }
                foreach (VendorItem item in value.SellItems)
                {
                    Universal_ItemList uil = new Universal_ItemList(item, Universal_ItemList.ReturnType.VendorItem, false);
                    uil.deleteButton.Click += DeleteVendorSell_Click;
                    vendorListSellItems.Children.Add(uil);
                }
                vendorIdTxtBox.Value = value.id;
                vendorTitleTxtBox.Text = value.vendorTitle;
                vendorDescTxtBox.Text = value.vendorDescription;
                vendor_commentbox.Text = value.comment;
            }
        }
        private void AddVendorItem_Click(object sender, RoutedEventArgs e)
        {
            BetterForms.Universal_VendorItemEditor uvie = new BetterForms.Universal_VendorItemEditor();
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {Inputted_EditorName}",
                State = $"Adding vendor item"
            };
            (MainWindow.DiscordWorker as DiscordRPC.DiscordWorker)?.SendPresence(presence);
            if (uvie.ShowDialog() == true)
            {
                VendorItem resultedVendorItem = uvie.Result;
                if (resultedVendorItem.isBuy)
                {
                    Vendor_Add_Buy(resultedVendorItem);
                }
                else
                {
                    Vendor_Add_Sell(resultedVendorItem);
                }
                if (Autosave_Vendor_Enabled && CurrentVendor.id > 0)
                    SaveVendor_Click(sender, e);
            }
            TabControl_SelectionChanged(mainTabControl, null);
        }
        private void SaveVendor_Click(object sender, RoutedEventArgs e)
        {
            NPCVendor cur = CurrentVendor;
            if (vendors.Where(d => d.id == cur.id).Count() > 0)
            {
                vendors.Remove(vendors.Where(d => d.id == cur.id).ElementAt(0));
            }
            vendors.Add(cur);
            DoNotification((string)TryFindResource("notify_Vendor_Saved"));
            if (Autosave_NPC_Enabled && saveFile?.Length > 0)
                Save();
        }
        private void ClearVendor_Click(object sender, RoutedEventArgs e)
        {
            vendorListBuyItems.Children.Clear();
            vendorListSellItems.Children.Clear();
        }
        private void OpenVendor_Click(object sender, RoutedEventArgs e)
        {
            BetterForms.Universal_ListView ulv = new BetterForms.Universal_ListView(vendors.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Vendor, false)).ToList(), Universal_ItemList.ReturnType.Vendor);
            if (ulv.ShowDialog() == true)
            {
                if (Autosave_Vendor_Enabled && CurrentVendor.id > 0)
                    SaveVendor_Click(sender, e);
                CurrentVendor = ulv.SelectedValue as NPCVendor;
            }
            vendors = ulv.Values.Cast<NPCVendor>().ToList();
        }
        private void DeleteVendorBuy_Click(object sender, RoutedEventArgs e)
        {
            Universal_ItemList uil = Util.FindParent<Universal_ItemList>(sender as Button);
            vendorListBuyItems.Children.Remove(uil);
            if (Autosave_Vendor_Enabled && CurrentVendor.id > 0)
                SaveVendor_Click(sender, e);
        }
        private void DeleteVendorSell_Click(object sender, RoutedEventArgs e)
        {
            Universal_ItemList uil = Util.FindParent<Universal_ItemList>(sender as Button);
            vendorListSellItems.Children.Remove(uil);
            if (Autosave_Vendor_Enabled && CurrentVendor.id > 0)
                SaveVendor_Click(sender, e);
        }
        public void Vendor_Add_Buy(VendorItem item)
        {
            Universal_ItemList uil = new Universal_ItemList(item, Universal_ItemList.ReturnType.VendorItem, false)
            {
                Width = 240
            };
            uil.deleteButton.Click += DeleteVendorBuy_Click;
            vendorListBuyItems.Children.Add(uil);
        }
        public void Vendor_Add_Sell(VendorItem item)
        {
            Universal_ItemList uil = new Universal_ItemList(item, Universal_ItemList.ReturnType.VendorItem, false)
            {
                Width = 240
            };
            uil.deleteButton.Click += DeleteVendorSell_Click;
            vendorListSellItems.Children.Add(uil);
        }
        public void Vendor_Delete_Buy(UIElement item)
        {
            vendorListBuyItems.Children.Remove(item);
        }
        public void Vendor_Delete_Buy(int id)
        {
            vendorListBuyItems.Children.RemoveAt(id);
        }
        public void Vendor_Delete_Sell(int id)
        {
            vendorListSellItems.Children.RemoveAt(id);
        }
        public void Vendor_Delete_Sell(UIElement item)
        {
            vendorListSellItems.Children.Remove(item);
        }
#endregion
        #region QUEST_EDITOR
        public NPCQuest CurrentQuest
        {
            get
            {
                NPCQuest ret = new NPCQuest() { conditions = new List<NPC.Condition>(), rewards = new List<Reward>() };

                foreach (UIElement ui in listQuestRewards.Children)
                {
                    ret.rewards.Add((ui as Universal_ItemList).Value as Reward);
                }
                foreach (UIElement ui in listQuestConditions.Children)
                {
                    ret.conditions.Add((ui as Universal_ItemList).Value as NPC.Condition);
                }
                ret.title = questTitleBox.Text;
                ret.description = questDescBox.Text;
                ret.id = (ushort)questIdBox.Value;
                ret.comment = quest_commentbox.Text;

                return ret;
            }
            set
            {
                ClearQuest_Click(null, null);
                foreach (Reward reward in value.rewards)
                {
                    Universal_ItemList uil = new Universal_ItemList(reward, Universal_ItemList.ReturnType.Reward, true);
                    uil.deleteButton.Click += RemoveQuestReward_Click;
                    listQuestRewards.Children.Add(uil);
                }
                foreach (NPC.Condition cond in value.conditions)
                {
                    Universal_ItemList uil = new Universal_ItemList(cond, Universal_ItemList.ReturnType.Condition, true);
                    uil.deleteButton.Click += RemoveQuestCondition_Click;
                    listQuestConditions.Children.Add(uil);
                }
                questIdBox.Value = value.id;
                questTitleBox.Text = value.title;
                questDescBox.Text = value.description;
                quest_commentbox.Text = value.comment;
            }
        }
        private void AddQuestCondition_Click(object sender, RoutedEventArgs e)
        {
            Universal_ConditionEditor uce = new Universal_ConditionEditor(null, true);
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {Inputted_EditorName}",
                State = "Creating condition for a quest"
            };
            (MainWindow.DiscordWorker as DiscordRPC.DiscordWorker)?.SendPresence(presence);
            if (uce.ShowDialog() == true)
            {
                NPC.Condition cond = uce.Result;
                Universal_ItemList uil = new Universal_ItemList(cond, Universal_ItemList.ReturnType.Condition, true);
                uil.deleteButton.Click += RemoveQuestCondition_Click;
                listQuestConditions.Children.Add(uil);
                if (Autosave_Quest_Enabled && CurrentQuest.id > 0)
                    SaveQuest_Click(sender, e);
            }
            TabControl_SelectionChanged(mainTabControl, null);
        }
        private void RemoveQuestCondition_Click(object sender, RoutedEventArgs e)
        {
            Universal_ItemList uil = Util.FindParent<Universal_ItemList>(sender as Button);
            listQuestConditions.Children.Remove(uil);
            if (Autosave_Quest_Enabled && CurrentQuest.id > 0)
                SaveQuest_Click(sender, e);
        }
        private void AddQuestReward_Click(object sender, RoutedEventArgs e)
        {
            Universal_RewardEditor ure = new Universal_RewardEditor(null, true);
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {Inputted_EditorName}",
                State = "Creating reward for a quest"
            };
            (MainWindow.DiscordWorker as DiscordRPC.DiscordWorker)?.SendPresence(presence);
            if (ure.ShowDialog() == true)
            {
                Reward rew = ure.Result;
                Universal_ItemList uil = new Universal_ItemList(rew, Universal_ItemList.ReturnType.Reward, true);
                uil.deleteButton.Click += RemoveQuestReward_Click;
                listQuestRewards.Children.Add(uil);
                if (Autosave_Quest_Enabled && CurrentQuest.id > 0)
                    SaveQuest_Click(sender, e);
            }
            TabControl_SelectionChanged(mainTabControl, null);
        }
        private void RemoveQuestReward_Click(object sender, RoutedEventArgs e)
        {
            Universal_ItemList uil = Util.FindParent<Universal_ItemList>(sender as Button);
            listQuestRewards.Children.Remove(uil);
            if (Autosave_Quest_Enabled && CurrentQuest.id > 0)
                SaveQuest_Click(sender, e);
        }
        private void SaveQuest_Click(object sender, RoutedEventArgs e)
        {
            if (quests.Where(d => d.id == questIdBox.Value).Count() > 0)
                quests.Remove(quests.Where(d => d.id == questIdBox.Value).ElementAt(0));
            quests.Add(CurrentQuest);
            DoNotification((string)TryFindResource("notify_Quest_Saved"));
            if (Autosave_NPC_Enabled && saveFile?.Length > 0)
                Save();
        }
        private void LoadQuest_Click(object sender, RoutedEventArgs e)
        {
            BetterForms.Universal_ListView ulv = new BetterForms.Universal_ListView(quests.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Quest, false)).ToList(), Universal_ItemList.ReturnType.Quest);
            if (ulv.ShowDialog() == true)
            {
                if (Autosave_Quest_Enabled && CurrentQuest.id > 0)
                    SaveQuest_Click(sender, e);
                CurrentQuest = ulv.SelectedValue as NPCQuest;
            }
            quests = ulv.Values.Cast<NPCQuest>().ToList();
        }
        private void ClearQuest_Click(object sender, RoutedEventArgs e)
        {
            listQuestConditions.Children.Clear();
            listQuestRewards.Children.Clear();
        }
#endregion
        #region DRAG AND DROP
        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            dropOverlay.Visibility = Visibility.Visible;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }
        private void Window_DragLeave(object sender, DragEventArgs e)
        {
            dropOverlay.Visibility = Visibility.Hidden;
        }
        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                string path = (e.Data.GetData(DataFormats.FileDrop) as string[])[0];
                if (!Load(path, false))
                {
                    if (Load(path, true, true))
                    {
                        notificationsStackPanel.Children.Clear();
                        DoNotification((string)TryFindResource("notify_Loaded"));
                    }
                }
            }
            dropOverlay.Visibility = Visibility.Hidden;
        }
#endregion
        #region UPDATE CHECK
        public async Task<bool> IsUpdateAvailable()
        {
            if (updCache)
                return updValue;
            else
            {
                checkForUpdatesButton.IsEnabled = false;
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        string vers = await wc.DownloadStringTaskAsync("https://bowiestuff.at.ua/npcmakerversion.txt");
                        updCache = true;
                        if (vers == version.ToString())
                        {
                            updValue = false;
                        }
                        else
                        {
                            forceUpdateButton.IsEnabled = true;
                            updValue = true;
                        }
                        return updValue;
                    }
                }
                catch { }
                finally { checkForUpdatesButton.IsEnabled = true; }
#region LEGACY
                //try
                //{
                //    using (WebClient wc = new WebClient())
                //    {
                //        string vers = wc.DownloadString("https://bowiestuff.at.ua/npcmakerversion.txt");
                //        if (vers == version.ToString())
                //        {
                //            updCache = true;
                //            updValue = false;
                //            return false;
                //        }
                //        forceUpdateButton.IsEnabled = true;
                //        updCache = true;
                //        updValue = true;
                //        return true;
                //    }
                //}
                //catch { }
#endregion
            }
            return false;
        }

        private bool updCache = false;
        private bool updValue = false;
#endregion
        #region UPDATE
        public void DownloadUpdater()
        {
            using (WebClient wc = new WebClient())
            using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "updater.exe", FileMode.Create))
            {
                byte[] dat = wc.DownloadData("https://bowiestuff.at.ua/npcmakerupdater.e");
                for (int k = 0; k < dat.Length; k++)
                {
                    fs.WriteByte(dat[k]);
                }
            }
        }
        private void ForceUpdate_Click(object sender, RoutedEventArgs e)
        {
            DownloadUpdater();
            RunUpdate();
        }

        public void RunUpdate()
        {
            string fileName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (!fileName.EndsWith(".exe"))
                fileName += ".exe";
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "updater.exe",  fileName);
            (DiscordWorker as DiscordRPC.DiscordWorker)?.Deinitialize();
            Environment.Exit(0);
        }
#endregion
        #region NOTIFICATIONS
        public void DoNotification(string text, double fontSize = 16, TextAlignment textAlignment = TextAlignment.Center)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                TextAlignment = textAlignment,
                FontSize = fontSize,
                TextWrapping = TextWrapping.Wrap
            };
            DoNotification(textBlock);
        }
        public void DoNotification(TextBlock textBlock)
        {
            Notification.NotificationBase notificationBase = new Notification.NotificationBase(notificationsStackPanel, textBlock);
            notificationsStackPanel.Children.Add(notificationBase);
        }
#endregion
                
        #region DEEP GAME ANALYSIS METHODS
        // check for conflict id's, check every item in NPC for validness ^.^
        // WIP

        public enum EAssetType
        {
            None,
            Item,
            Dialogue,
            Vendor,
            Animal,
            NPC,
            Quest,
            Vehicle
        }
        public enum EItemType
        {
            HAT,
            PANTS,
            SHIRT,
            MASK,
            BACKPACK,
            VEST,
            GLASSES,
            GUN,
            SIGHT,
            TACTICAL,
            GRIP,
            BARREL,
            MAGAZINE,
            FOOD,
            WATER,
            MEDICAL,
            MELEE,
            FUEL,
            TOOL,
            BARRICADE,
            STORAGE,
            BEACON,
            FARM,
            TRAP,
            STRUCTURE,
            SUPPLY,
            THROWABLE,
            GROWER,
            OPTIC,
            REFILL,
            FISHER,
            CLOUD,
            MAP,
            KEY,
            BOX,
            ARREST_START,
            ARREST_END,
            TANK,
            GENERATOR,
            DETONATOR,
            CHARGE,
            LIBRARY,
            FILTER,
            SENTRY,
            VEHICLE_REPAIR_TOOL,
            TIRE,
            COMPASS,
            OIL_PUMP
        }

        public static async Task<bool> CacheFiles(string directory, bool light = true)
        {
            List<UnturnedFile> cache = new List<UnturnedFile>();
            List<FileInfo> validFiles = new DirectoryInfo(directory).GetFiles("*.dat", SearchOption.AllDirectories).ToList();
            if (light)
            {
                long oldTotal = validFiles.Count();
                validFiles = validFiles.Where(d => d.Name != "English.dat" && d.Name != "Russian.dat").ToList();
                Instance.DoNotification($"Skipped {oldTotal - validFiles.Count()} files.");
            }
            long step = 1;
            Instance.progrBar.Maximum = validFiles.Count();
            foreach (FileInfo fi in validFiles)
            {
                step++;
                try
                {
                    bool typeFound = false;
                    EAssetType assetType = EAssetType.None;
                    bool idFound = false;
                    bool skip = false;
                    ushort id = 0;
                    UnturnedFile unturnedFile = new UnturnedFile() { FileName = fi.Name, Path = fi.FullName };
                    using (StreamReader sr = new StreamReader(fi.FullName))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (line?.ToLower().StartsWith("name ") == true || line?.ToLower().StartsWith("description ") == true)
                            {
                                skip = true;
                                break;
                            }
                            if (!typeFound && line?.ToLower().StartsWith("type ") == true && line.Split(' ')?.Length >= 2)
                            {
                                if (Enum.TryParse(line.Substring(line.IndexOf(' ') + 1), out EAssetType type))
                                {
                                    assetType = type;
                                    typeFound = true;
                                }
                                else if (Enum.TryParse(line.Substring(line.IndexOf(' ') + 1).ToUpper(), out EItemType itemType))
                                {
                                    assetType = EAssetType.Item;
                                    typeFound = true;
                                }
                            }
                            if (!idFound && line?.ToLower().StartsWith("id ") == true && line.Split(' ')?.Length == 2)
                            {
                                if (ushort.TryParse(line.Split(' ')[1], out ushort resultedId))
                                {
                                    id = resultedId;
                                    idFound = true;
                                }
                            }
                        }
                    }
                    if (idFound)
                        unturnedFile.Id = id;
                    if (typeFound)
                        unturnedFile.Type = assetType;
                    if (!skip && assetType != EAssetType.None)
                        cache.Add(unturnedFile);
                }
                catch { }
                await Task.Yield();
                if (step % 25 == 0)
                    await Task.Delay(1);
                Instance.progrBar.Value = step;
            }
            CachedUnturnedFiles = cache;
            CacheUpdated = true;
            return true;
        }
        public static bool CacheUpdated = false;
        public static List<UnturnedFile> CachedUnturnedFiles { get; set; }

        public class UnturnedFile
        {
            public string FileName { get; set; }
            [XmlIgnore]
            public string Path { get; set; }
            public ushort Id { get; set; }
            public EAssetType Type { get; set; }
        }

        internal static bool AssetExist(EAssetType assetType, ushort id)
        {
            var res = (from d
             in CachedUnturnedFiles
             where d.Id == id && d.Type == assetType
             select d);
            return (res.Count() > 0);
        }
        internal static UnturnedFile GetAsset(EAssetType assetType, ushort id)
        {
            var res = (from d
                       in CachedUnturnedFiles
                       where d.Id == id && d.Type == assetType
                       select d);
            return res.First();
        }

#endregion
    }
}
