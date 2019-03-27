#define RELEASE

using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Windows;
using System.Net;
using System.Windows.Media;
using System.Globalization;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.BetterControls;
using BowieD.Unturned.NPCMaker.Examples;
using DiscordRPC;
using System.Windows.Threading;
using BowieD.Unturned.NPCMaker.Logging;
using System.Reflection;

namespace BowieD.Unturned.NPCMaker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Logger.Log($"Launch stage. Version: {Version}.");
            Instance = this;
            Proxy = new PropertyProxy(this);
            Proxy.RegisterEvents();
            #region SCALE
            mainGridScale.ScaleX = Config.Configuration.Properties.scale;
            mainGridScale.ScaleY = Config.Configuration.Properties.scale;
            Width = MinWidth * Config.Configuration.Properties.scale;
            Height = MinHeight * Config.Configuration.Properties.scale;
            Logger.Log($"Scale set up to {Config.Configuration.Properties.scale}");
            #endregion
            #region THEME SETUP
            (Config.Configuration.Properties.currentTheme ?? Config.Configuration.DefaultTheme).Setup();
            Logger.Log($"Theme set to {(Config.Configuration.Properties.currentTheme ?? Config.Configuration.DefaultTheme).Name}");
            #endregion
            #region LOCALIZATION
            App.Language = Config.Configuration.Properties.language ?? new CultureInfo("en-US");
            Logger.Log($"Language set to {Config.Configuration.Properties.language.Name}");
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
                            if (!Load(args[k], false, true))
                            {
                                if (Load(args[k], true, true))
                                {
                                    notificationsStackPanel.Children.Clear();
                                    DoNotification(Localize("notify_Loaded"));
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
            #region APPAREL SETUP
            faceImageIndex.Maximum = faceAmount - 1;
            beardImageIndex.Maximum = beardAmount - 1;
            hairImageIndex.Maximum = haircutAmount - 1;
            Proxy.FaceImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, CurrentNPC.face));
            Proxy.HairImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, CurrentNPC.haircut));
            Proxy.BeardImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, CurrentNPC.beard));
            beardRenderGrid.DataContext = CurrentNPC.hairColor.Brush;
            hairRenderGrid.DataContext = CurrentNPC.hairColor.Brush;
            faceImageBorder.Background = apparelSkinColorBox.Text.Length == 0 ? Brushes.Transparent : CurrentNPC.skinColor.Brush;
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
                g.Children.Add(b);
                g.Children.Add(l);
                g.Children.Add(copyButton);
                unturnedColorSampleList.Children.Add(g);
            }
            #endregion
            Proxy.UserColorListChanged();
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
                    Proxy.WhatsNew_Menu_Click(null, null);
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "updater.exe");
                }
            }
            catch { }
            #endregion
            #region AUTOSAVE INIT
            if (Config.Configuration.Properties.autosaveOption > 0)
            {
                AutosaveTimer = new DispatcherTimer();
                switch (Config.Configuration.Properties.autosaveOption)
                {
                    case 1:
                        AutosaveTimer.Interval = new TimeSpan(0, 5, 0);
                        break;
                    case 2:
                        AutosaveTimer.Interval = new TimeSpan(0, 10, 0);
                        break;
                    case 3:
                        AutosaveTimer.Interval = new TimeSpan(0, 15, 0);
                        break;
                    case 4:
                        AutosaveTimer.Interval = new TimeSpan(0, 30, 0);
                        break;
                    case 5:
                        AutosaveTimer.Interval = new TimeSpan(1, 0, 0);
                        break;
                }
                AutosaveTimer.Tick += AutosaveTimer_Tick;
                AutosaveTimer.Start();
            }
            #endregion
            #region VERSION SPECIFIC CODE
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
            Proxy.CheckForUpdates_Click(Instance, null);
            Config.Configuration.Properties.firstLaunch = false;
            isSaved = true;
            #region DISCORD
            if (Config.Configuration.Properties.enableDiscord)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "DiscordRPC.dll") && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Newtonsoft.Json.dll"))
                {
                    DiscordWorker = new DiscordRPC.DiscordWorker(1000);
                    (DiscordWorker as DiscordRPC.DiscordWorker)?.Initialize();
                    Proxy.TabControl_SelectionChanged(mainTabControl, null);
                }
            }
            #endregion
            #region ENABLE EXPERIMENTAL
            if (Config.Configuration.Properties.experimentalFeatures)
            {

            }
            #endregion
            Proxy.ColorSliderChange(null, null);
            DoNotification(MainWindow.Localize("app_Free"));
        }
        public static string Localize(string key)
        {
            var res = Instance.TryFindResource(key);
            if (res == null || !(res is string resString) || resString.Length == 0)
                return key;
            return resString;
        }
        public static string Localize(string key, params object[] format)
        {
            string res = Localize(key);
            if (res == key)
                return key;
            return string.Format(res, format);
        }
        #region THEMES
        internal void Theme_SetupMetro()
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml") });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml") });
            IsMetro = true;
        }
        internal void Theme_Clear()
        {
            ResourceDictionary metroControls = (from d in Application.Current.Resources.MergedDictionaries
                                                where d.Source != null && d.Source.OriginalString == "pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml"
                                                select d).FirstOrDefault();
            ResourceDictionary metroFonts = (from d in Application.Current.Resources.MergedDictionaries
                                             where d.Source != null && d.Source.OriginalString == "pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml"
                                             select d).FirstOrDefault();
            List<ResourceDictionary> metroThemes = (from d in Application.Current.Resources.MergedDictionaries
                                                           where d.Source != null && d.Source.OriginalString.StartsWith("pack://application:,,,/MahApps.Metro;component/Styles/Themes/")
                                                           select d).ToList();
            if (metroControls != null)
                Application.Current.Resources.MergedDictionaries.Remove(metroControls);
            if (metroFonts != null)
                Application.Current.Resources.MergedDictionaries.Remove(metroFonts);
            if (metroThemes?.Count() > 0)
            {
                foreach (var dic in metroThemes)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dic);
                }
            }
            IsMetro = false;
        }
        #endregion
        #region CONSTANTS
        public const int
        faceAmount = 32,
        beardAmount = 16,
        haircutAmount = 23;
        public static Version Version => new Version(1, 0, 0, 1);
        #endregion
        #region STATIC
        public static MainWindow Instance;
        public static NPCSave CurrentNPC { get; set; } = new NPCSave();
        public static object DiscordWorker { get; set; }
        public static DispatcherTimer AutosaveTimer { get; set; }
        public static PropertyProxy Proxy { get; private set; }
        public static bool IsRGB { get; set; } = true;
        public static DateTime Started { get; set; } = DateTime.UtcNow;

        public static bool IsMetro { get; set; }
        #endregion
        #region CURRENT NPC
        public static string saveFile = "", oldFile = "";
        public static bool isSaved = true;
        #endregion
        private void AutosaveTimer_Tick(object sender, EventArgs e)
        {
            AutosaveTimer.Stop();
            if (saveFile?.Length > 0)
                Save();
            AutosaveTimer.Start();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            Config.Configuration.Save();
            e.Cancel = true;
            Proxy.ExitButtonClick(this, null);
            base.OnClosing(e);
        }
        #region SAVE_LOAD
        public void Save(bool asExample = false)
        {
#if !DEBUG
            if ((CurrentNPC?.IsReadOnly) ?? false)
            {
                DoNotification(Localize("notify_ReadOnly"));
                return;
            }
#endif
            //RebuildApparel();
            Dialogue_SaveButtonClick(null, null);
            SaveVendor_Click(null, null);
            SaveQuest_Click(null, null);
            if (saveFile == null || saveFile == "")
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = $"{Localize("save_Filter")} (*.npc)|*.npc",
                    FileName = $"{CurrentNPC.editorName}.npc",
                    OverwritePrompt = true
                };
                var result = sfd.ShowDialog();
                if (result == true)
                    saveFile = sfd.FileName;
                else
                {
                    saveFile = oldFile.Length > 0 ? oldFile : saveFile;
                    return;
                }
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
                    NPCExample example = new NPCExample(CurrentNPC);
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
                        DoNotification(Localize("notify_ReadOnly"));
                        return;
                    }
                    #endif
                    if (saveFile != null && saveFile != "")
                    {
                        using (FileStream fs = new FileStream(saveFile, FileMode.Create))
                        using (XmlWriter writer = XmlWriter.Create(fs))
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(NPCSave));
                            ser.Serialize(writer, CurrentNPC);
                        }
                        DoNotification(Localize("notify_Saved"));
                        isSaved = true;
                    }
                }
            }
            catch (Exception ex) { DoNotification($"Saving failed! Exception: {ex.Message}"); }
            if (oldFile != "")
                MainWindow.saveFile = oldFile;
        }
        public bool Load(string path, bool asExample = false, bool skipPrompt = false)
        {
            if (!skipPrompt && !SavePrompt())
                return false;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    XmlSerializer deser = new XmlSerializer(asExample ? typeof(NPCExample) : typeof(NPCSave));
                    if (asExample)
                    {
                        NPCExample save = deser.Deserialize(reader) as NPCExample;
                        CurrentNPC = save;
                    }
                    else
                    {
                        NPCSave save = deser.Deserialize(reader) as NPCSave;
                        CurrentNPC = save;
                        saveFile = path;
                    }
                    ConvertNPCToState(CurrentNPC);
                    isSaved = true;
                }
                DoNotification(Localize("notify_Loaded"));
                Started = DateTime.UtcNow;
                return true;
            }
            catch { DoNotification(Localize("load_Incompatible")); return false; }
        }
        public bool SavePrompt()
        {
            if (isSaved == true || CurrentNPC.IsReadOnly || CurrentNPC == new NPCSave())
                return true;
            var result = MessageBox.Show(Localize("app_Exit_UnsavedChanges_Text"), Localize("app_Exit_UnsavedChanges_Title"), MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
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
        #endregion
        #region STATE CONVERTERS
        public void ConvertNPCToState(NPCSave save)
        {
            CurrentNPC = save;
            apparelSkinColorBox.Text = save.skinColor.HEX;
            apparelHairColorBox.Text = save.hairColor.HEX;
            backpackIdBox.Value = save.backpack;
            maskIdBox.Value = save.mask;
            vestIdBox.Value = save.vest;
            topIdBox.Value = save.top;
            bottomIdBox.Value = save.bottom;
            glassesIdBox.Value = save.glasses;
            hatIdBox.Value = save.hat;
            primaryIdBox.Value = save.equipPrimary;
            secondaryIdBox.Value = save.equipSecondary;
            tertiaryIdBox.Value = save.equipTertiary;
            faceImageIndex.Value = save.face;
            beardImageIndex.Value = save.beard;
            hairImageIndex.Value = save.haircut;
            txtID.Value = save.id;
            txtEditorName.Text = save.editorName;
            txtDisplayName.Text = save.displayName;
            txtStartDialogueID.Value = save.startDialogueId;
            apparelLeftHandedCheckbox.IsChecked = save.leftHanded;
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
            CurrentDialogue = new NPCDialogue();
            CurrentQuest = new NPCQuest();
            CurrentVendor = new NPCVendor();
        }
        #endregion
        #region MISTAKES
        public void FindMistakes()
        {
            if (CheckableMistakes == null)
            {
                CheckableMistakes = new HashSet<Mistake>();
                string[] nspaces = {
                    "BowieD.Unturned.NPCMaker.Mistakes.Dialogue",
                    "BowieD.Unturned.NPCMaker.Mistakes.General",
                    "BowieD.Unturned.NPCMaker.Mistakes.Vendor",
                    "BowieD.Unturned.NPCMaker.Mistakes.Quests",
                    "BowieD.Unturned.NPCMaker.Mistakes.Apparel"
                };
                var q = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass && nspaces.Contains(t.Namespace) select t;
                foreach (Type t in q)
                {
                    var mistake = Activator.CreateInstance(t);
                    if (mistake is Mistake mist)
                        CheckableMistakes.Add(mist);
                }
            }
            lstMistakes.Items.Clear();
            var foundMistakes = CheckableMistakes.Where(d => d.IsMistake);
            foreach (Mistake m in foundMistakes)
            {
                lstMistakes.Items.Add(m);
            }
            if (lstMistakes.Items.Count == 0)
            {
                lstMistakes.Visibility = Visibility.Collapsed;
                noErrorsLabel.Visibility = Visibility.Visible;
            }
            else
            {
                lstMistakes.Visibility = Visibility.Visible;
                noErrorsLabel.Visibility = Visibility.Collapsed;
            }
        }
        public static HashSet<Mistake> CheckableMistakes;
        public int Advices => CheckableMistakes.Where(d => d.Importance == IMPORTANCE.ADVICE && d.IsMistake).Count();
        public int Warnings => CheckableMistakes.Where(d => d.Importance == IMPORTANCE.HIGH && d.IsMistake).Count();
        public int No_Exports => CheckableMistakes.Where(d => d.Importance == IMPORTANCE.NO_EXPORT && d.IsMistake).Count();
        private async void DA_Button_Click(object sender, RoutedEventArgs e)
        {
            bool skipCache = false;
            if (CachedUnturnedFiles?.Count() > 0)
            {
                var res = MessageBox.Show(Localize("mistakes_DA_UpdateCache"), "", MessageBoxButton.YesNoCancel);
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
                if (res == System.Windows.Forms.DialogResult.Cancel || !File.Exists(fbd.SelectedPath + @"\Unturned.exe"))
                {
                    blockActionsOverlay.Visibility = Visibility.Collapsed;
                    return;
                }
                await CacheFiles(fbd.SelectedPath);
                blockActionsOverlay.Visibility = Visibility.Collapsed;
            }
            FindMistakes();
            foreach (NPCDialogue dialogue in CurrentNPC.dialogues)
            {
                if (CachedUnturnedFiles != null && CachedUnturnedFiles.Any(d => d.Type == EAssetType.Dialogue && d.Id == dialogue.id))
                {
                    lstMistakes.Items.Add(new Mistakes.Generic(Localize("deep_dialogue", dialogue.id), "", IMPORTANCE.HIGH, true, false));
                }
                await Task.Yield();
            }
            foreach (NPCVendor vendor in CurrentNPC.vendors)
            {
                if (CachedUnturnedFiles != null && CachedUnturnedFiles.Any(d => d.Type == EAssetType.Vendor && d.Id == vendor.id))
                {
                    lstMistakes.Items.Add(new Mistakes.Generic(Localize("deep_vendor", vendor.id), "", IMPORTANCE.HIGH, true, false));
                }
                foreach (var it in vendor.items)
                {
                    if (it.type == ItemType.VEHICLE && !CachedUnturnedFiles.Any(d => d.Type == EAssetType.Vehicle && d.Id == it.id))
                    {
                        lstMistakes.Items.Add(new Mistakes.Generic(Localize("deep_vehicle", it.id), "", IMPORTANCE.HIGH, true, false));
                        continue;
                    }
                    if (it.type == ItemType.ITEM && !CachedUnturnedFiles.Any(d => d.Type == EAssetType.Item && d.Id == it.id))
                    {
                        lstMistakes.Items.Add(new Mistakes.Generic(Localize("deep_item", it.id), "", IMPORTANCE.HIGH, true, false));
                        continue;
                    }
                }
                await Task.Yield();
            }
            foreach (NPCQuest quest in CurrentNPC.quests)
            {
                if (CachedUnturnedFiles != null && CachedUnturnedFiles.Any(d => d.Type == EAssetType.Quest && d.Id == quest.id))
                {
                    lstMistakes.Items.Add(new Mistakes.Generic(Localize("deep_quest", quest.id), "", IMPORTANCE.HIGH, true, false));
                }
                await Task.Yield();
            }
            if (txtID.Value > 0)
            {
                ushort input = (ushort)txtID.Value;
                if (CachedUnturnedFiles != null && CachedUnturnedFiles.Any(d => d.Type == EAssetType.NPC && d.Id == input))
                {
                    lstMistakes.Items.Add(new Mistakes.Generic(Localize("deep_char", input), "", IMPORTANCE.HIGH, true, false));
                }
            }
            blockActionsOverlay.Visibility = Visibility.Collapsed;
            if (lstMistakes.Items.Count == 0)
            {
                lstMistakes.Visibility = Visibility.Collapsed;
                noErrorsLabel.Visibility = Visibility.Visible;
            }
            else
            {
                lstMistakes.Visibility = Visibility.Visible;
                noErrorsLabel.Visibility = Visibility.Collapsed;
            }
        }
        #endregion
        #region DIALOGUE_EDITOR
        private void Dialogue_SaveButtonClick(object sender, RoutedEventArgs e)
        {
            var dil = CurrentDialogue;
            if (dil.id == 0)
            {
                if (sender != null)
                    DoNotification(Localize("dialogue_ID_Zero"));
                return;
            }
            var o = CurrentNPC.dialogues.Where(d => d.id == dil.id);
            if (o.Count() > 0)
                CurrentNPC.dialogues.Remove(o.ElementAt(0));
            CurrentNPC.dialogues.Add(CurrentDialogue);
            if (sender != null)
                DoNotification(Localize("notify_Dialogue_Saved"));
            isSaved = false;
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
            var ulv = new Universal_ListView(CurrentNPC.dialogues.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Dialogue, false)).ToList(), Universal_ItemList.ReturnType.Dialogue);
            if (ulv.ShowDialog() == true)
            {
                Dialogue_SaveButtonClick(sender, null);
                CurrentDialogue = ulv.SelectedValue as NPCDialogue;
            }
            CurrentNPC.dialogues = ulv.Values.Cast<NPCDialogue>().ToList();
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
        }
        private void Dialogue_RemoveMessageClick(object sender, RoutedEventArgs e)
        {
            Dialogue_Message pag = Util.FindParent<Dialogue_Message>(sender as Button);
            messagePagesGrid.Children.Remove(pag);
        }
        private void SetAsStart_Button_Click(object sender, RoutedEventArgs e)
        {
            Dialogue_SaveButtonClick(null, null);
            var dial = CurrentDialogue;
            if (dial.id > 0 && txtStartDialogueID.Value != dial.id)
            {
                txtStartDialogueID.Value = dial.id;
                CurrentNPC.startDialogueId = dial.id;
                try
                {
                    DoNotification(Localize("dialogue_Start_Notify", dial.id));
                }
                catch { }
            }
        }
        public NPCDialogue CurrentDialogue
        {
            get
            {
                ushort dialogueID = (ushort)(dialogueInputIdControl.Value ?? 0);
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
                        dr.RebuildResponse();
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
                    id = (ushort)(vendorIdTxtBox.Value ?? 0),
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
            }
            Proxy.TabControl_SelectionChanged(mainTabControl, null);
        }
        private void SaveVendor_Click(object sender, RoutedEventArgs e)
        {
            NPCVendor cur = CurrentVendor;
            if (cur.id == 0)
                return;
            if (CurrentNPC.vendors.Where(d => d.id == cur.id).Count() > 0)
            {
                CurrentNPC.vendors.Remove(CurrentNPC.vendors.Where(d => d.id == cur.id).ElementAt(0));
            }
            CurrentNPC.vendors.Add(cur);
            if (sender != null)
                DoNotification(Localize("notify_Vendor_Saved"));
            isSaved = false;
        }
        private void ClearVendor_Click(object sender, RoutedEventArgs e)
        {
            vendorListBuyItems.Children.Clear();
            vendorListSellItems.Children.Clear();
        }
        private void OpenVendor_Click(object sender, RoutedEventArgs e)
        {
            BetterForms.Universal_ListView ulv = new BetterForms.Universal_ListView(CurrentNPC.vendors.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Vendor, false)).ToList(), Universal_ItemList.ReturnType.Vendor);
            if (ulv.ShowDialog() == true)
            {
                SaveVendor_Click(sender, null);
                CurrentVendor = ulv.SelectedValue as NPCVendor;
            }
            CurrentNPC.vendors = ulv.Values.Cast<NPCVendor>().ToList();
        }
        private void DeleteVendorBuy_Click(object sender, RoutedEventArgs e)
        {
            Universal_ItemList uil = Util.FindParent<Universal_ItemList>(sender as Button);
            vendorListBuyItems.Children.Remove(uil);
        }
        private void DeleteVendorSell_Click(object sender, RoutedEventArgs e)
        {
            Universal_ItemList uil = Util.FindParent<Universal_ItemList>(sender as Button);
            vendorListSellItems.Children.Remove(uil);
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
                ret.id = (ushort)(questIdBox.Value ?? 0);
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
            if (uce.ShowDialog() == true)
            {
                NPC.Condition cond = uce.Result;
                Universal_ItemList uil = new Universal_ItemList(cond, Universal_ItemList.ReturnType.Condition, true);
                uil.deleteButton.Click += RemoveQuestCondition_Click;
                listQuestConditions.Children.Add(uil);
            }
            Proxy.TabControl_SelectionChanged(mainTabControl, null);
        }
        private void RemoveQuestCondition_Click(object sender, RoutedEventArgs e)
        {
            Universal_ItemList uil = Util.FindParent<Universal_ItemList>(sender as Button);
            listQuestConditions.Children.Remove(uil);
        }
        private void AddQuestReward_Click(object sender, RoutedEventArgs e)
        {
            Universal_RewardEditor ure = new Universal_RewardEditor(null, true);
            if (ure.ShowDialog() == true)
            {
                Reward rew = ure.Result;
                Universal_ItemList uil = new Universal_ItemList(rew, Universal_ItemList.ReturnType.Reward, true);
                uil.deleteButton.Click += RemoveQuestReward_Click;
                listQuestRewards.Children.Add(uil);
            }
            Proxy.TabControl_SelectionChanged(mainTabControl, null);
        }
        private void RemoveQuestReward_Click(object sender, RoutedEventArgs e)
        {
            Universal_ItemList uil = Util.FindParent<Universal_ItemList>(sender as Button);
            listQuestRewards.Children.Remove(uil);
        }
        private void SaveQuest_Click(object sender, RoutedEventArgs e)
        {
            NPCQuest cur = CurrentQuest;
            if (cur.id == 0)
                return;
            if (CurrentNPC.quests.Where(d => d.id == questIdBox.Value).Count() > 0)
                CurrentNPC.quests.Remove(CurrentNPC.quests.Where(d => d.id == questIdBox.Value).ElementAt(0));
            CurrentNPC.quests.Add(CurrentQuest);
            if (sender != null)
                DoNotification(Localize("notify_Quest_Saved"));
            isSaved = false;
        }
        private void LoadQuest_Click(object sender, RoutedEventArgs e)
        {
            BetterForms.Universal_ListView ulv = new BetterForms.Universal_ListView(CurrentNPC.quests.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Quest, false)).ToList(), Universal_ItemList.ReturnType.Quest);
            if (ulv.ShowDialog() == true)
            {
                SaveQuest_Click(sender, null);
                CurrentQuest = ulv.SelectedValue as NPCQuest;
            }
            CurrentNPC.quests = ulv.Values.Cast<NPCQuest>().ToList();
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
                        DoNotification(Localize("notify_Loaded"));
                    }
                }
            }
            dropOverlay.Visibility = Visibility.Hidden;
        }
        #endregion
        #region UPDATE
        public async Task<bool?> IsUpdateAvailable()
        {
            checkForUpdatesButton.IsEnabled = false;
            try
            {
                using (WebClient wc = new WebClient())
                {
                    string vers = await wc.DownloadStringTaskAsync("https://raw.githubusercontent.com/iBowie/publicfiles/master/npcmakerversion.txt");
                    Version newVersion = new Version(vers);
                    bool res = newVersion > Version;
                    forceUpdateButton.IsEnabled = res;
                    return res;
                }
            }
            catch { }
            finally { checkForUpdatesButton.IsEnabled = true; }
            return null;
        }
        public void DownloadUpdater()
        {
            using (WebClient wc = new WebClient())
            using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "updater.exe", FileMode.Create))
            {
                byte[] dat = wc.DownloadData("https://raw.githubusercontent.com/iBowie/publicfiles/master/BowieD.Unturned.NPCMaker.Updater.exe");
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
            Notification.NotificationBase notificationBase = new Notification.NotificationBase(notificationsStackPanel, this.Background, textBlock);
            notificationBase.Opacity = 0.8;
            notificationsStackPanel.Children.Add(notificationBase);
        }
        public void DoNotification(TextBlock textBlock, TextBlock buttonText, Action<object, RoutedEventArgs> buttonAction)
        {
            Button b = new Button();
            b.Click += new RoutedEventHandler(buttonAction);
            b.Content = buttonText;
            Notification.NotificationBase notificationBase = new Notification.NotificationBase(notificationsStackPanel, this.Background, textBlock, b);
            notificationBase.Opacity = 0.8;
            notificationsStackPanel.Children.Add(notificationBase);
        }
        #endregion

        #region DEEP GAME ANALYSIS METHODS
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

        internal static bool AssetExist(EAssetType assetType, ushort id) => CachedUnturnedFiles.Any(d => d.Id == id && d.Type == assetType);
        internal static UnturnedFile GetAsset(EAssetType assetType, ushort id) => CachedUnturnedFiles.FirstOrDefault(d => d.Id == id && d.Type == assetType);

        #endregion
    }
}
