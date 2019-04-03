﻿#define RELEASE

using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Windows;
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
using System.Windows.Threading;
using BowieD.Unturned.NPCMaker.Logging;
using System.Reflection;
using BowieD.Unturned.NPCMaker.Managers;
using BowieD.Unturned.NPCMaker.Notification;
using BowieD.Unturned.NPCMaker.Editors;

namespace BowieD.Unturned.NPCMaker
{
    public partial class MainWindow : Window
    {
        #region MANAGERS
        public static INotificationManager NotificationManager { get; private set; } = new NotificationManager();
        public static Mistakes.DeepAnalysisManager DeepAnalysisManager { get; private set; }
        #endregion
        #region EDITORS
        public static IEditor<NPCDialogue> DialogueEditor { get; private set; }
        public static IEditor<NPCVendor> VendorEditor { get; private set; }
        public static IEditor<NPCQuest> QuestEditor { get; private set; }
        #endregion
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            DialogueEditor = new DialogueEditor();
            VendorEditor = new VendorEditor();
            QuestEditor = new QuestEditor();
            DeepAnalysisManager = new Mistakes.DeepAnalysisManager();
            Width *= Config.Configuration.Properties.scale;
            Height *= Config.Configuration.Properties.scale;
            Logger.Log($"Launch stage. Version: {Version}.");
            Proxy = new PropertyProxy(this);
            Proxy.RegisterEvents();
            #region THEME SETUP
            (Config.Configuration.Properties.currentTheme ?? Config.Configuration.DefaultTheme).Apply();
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
                            if (Load(args[k]))
                            {
                                notificationsStackPanel.Children.Clear();
                                MainWindow.NotificationManager.Notify(Localize("notify_Loaded"));
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
                MainWindow.NotificationManager.Notify("Happy Birthday, BowieD!");
            if (day == 30 && month == 9)
                MainWindow.NotificationManager.Notify("International Translation Day! Congratz, BowieD!");
            if (day == 30 && month == 10)
                MainWindow.NotificationManager.Notify("Happy Birthday, DimesAO!");
            #endregion
            #region OFFICIAL HOLIDAYS
            if ((day == 1 && month == 1) || (day == 31 && month == 12))
                MainWindow.NotificationManager.Notify("Happy New Year!");
            if (day == 14 && month == 2)
                MainWindow.NotificationManager.Notify("Valentine's Day!");
            if (day == 1 && month == 4)
                MainWindow.NotificationManager.Notify("April Fools!");
            if (day == 8 && month == 3)
                MainWindow.NotificationManager.Notify("Have a nice day, women!");
            if (day == 20 && month == 3)
                MainWindow.NotificationManager.Notify("Earth Day! Don't forget to do something good to Earth today!");
            if (day == 3 && month == 4)
                MainWindow.NotificationManager.Notify("Sun Day! Say hi to our beatiful star!");
            if (day == 4 && month == 4)
                MainWindow.NotificationManager.Notify("May the force be with you...");
            if (day == 11 && month == 1)
                MainWindow.NotificationManager.Notify("Thank You!");
            #endregion
            #region COMMUNITY HOLIDAYS
            if (day == 7 && month == 5)
                MainWindow.NotificationManager.Notify("Happy Birthday, Зефирка!");
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
            Config.Configuration.Properties.firstLaunch = false;
            isSaved = true;
            #region DISCORD
            DiscordWorker = new DiscordRPC.DiscordWorker(1000);
            DiscordWorker.descriptive = Config.Configuration.Properties.enableDiscord;
            DiscordWorker?.Initialize();
            Proxy.TabControl_SelectionChanged(mainTabControl, null);
            #endregion
            #region ENABLE EXPERIMENTAL
            if (Config.Configuration.Properties.experimentalFeatures)
            {

            }
            #endregion
            Proxy.ColorSliderChange(null, null);
            MainWindow.NotificationManager.Notify(MainWindow.Localize("app_Free"));
            UpdateManager = new GitHubUpdateManager();
            UpdateManager.CheckForUpdates();
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
        public static Version Version => new Version(1, 0, 2, 1);
        #endregion
        #region STATIC
        public static MainWindow Instance;
        public static NPCSave CurrentNPC { get; set; } = new NPCSave();
        public static DiscordRPC.DiscordWorker DiscordWorker { get; set; }
        public static DispatcherTimer AutosaveTimer { get; set; }
        public static PropertyProxy Proxy { get; private set; }
        public static bool IsRGB { get; set; } = true;
        public static DateTime Started { get; set; } = DateTime.UtcNow;
        public static IUpdateManager UpdateManager { get; set; }

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
                MainWindow.NotificationManager.Notify(Localize("notify_ReadOnly"));
                return;
            }
            #endif
            DialogueEditor.Save();
            VendorEditor.Save();
            QuestEditor.Save();
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
                        MainWindow.NotificationManager.Notify("EXAMPLE SAVED!");
                        isSaved = true;
                    }
                }
                else
                {
                    #if !DEBUG
                    if ((CurrentNPC?.IsReadOnly) ?? false)
                    {
                        MainWindow.NotificationManager.Notify(Localize("notify_ReadOnly"));
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
                        MainWindow.NotificationManager.Notify(Localize("notify_Saved"));
                        isSaved = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MainWindow.NotificationManager.Notify($"Saving failed! Exception: {ex.Message}");
                AppCrashReport acr = new AppCrashReport(ex, false, true);
                acr.ShowDialog();
            }
            if (oldFile != "")
                MainWindow.saveFile = oldFile;
        }
        public bool Load(string path, bool skipPrompt = false)
        {
            if (!skipPrompt && !SavePrompt())
                return false;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    XmlSerializer normalDeser = new XmlSerializer(typeof(NPCSave));
                    XmlSerializer exampleDeser = new XmlSerializer(typeof(NPCExample));
                    if (normalDeser.CanDeserialize(reader))
                    {
                        CurrentNPC = normalDeser.Deserialize(reader) as NPCSave;
                        saveFile = path;
                    }
                    else if (exampleDeser.CanDeserialize(reader))
                    {
                        CurrentNPC = exampleDeser.Deserialize(reader) as NPCExample;
                    }
                    ConvertNPCToState(CurrentNPC);
                    isSaved = true;
                }
                NotificationManager.Notify(Localize("notify_Loaded"));
                Started = DateTime.UtcNow;
                return true;
            }
            catch { NotificationManager.Notify(Localize("load_Incompatible")); return false; }
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
            DialogueEditor.Current = new NPCDialogue();
            QuestEditor.Current = new NPCQuest();
            VendorEditor.Current = new NPCVendor();
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
                if (Load(path))
                {
                    notificationsStackPanel.Children.Clear();
                    NotificationManager.Notify(Localize("notify_Loaded"));
                }
            }
            dropOverlay.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}