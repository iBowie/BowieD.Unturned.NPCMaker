using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Data;
using BowieD.Unturned.NPCMaker.Editors;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Managers;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Themes;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BowieD.Unturned.NPCMaker
{
    public partial class MainWindow : Window
    {
        #region MANAGERS
        public static Mistakes.DeepAnalysisManager DeepAnalysisManager { get; private set; }
        public static DiscordRPC.DiscordManager DiscordManager { get; set; }
        #endregion
        #region EDITORS
        public static IEditor<NPCCharacter> CharacterEditor { get; private set; }
        public static IEditor<NPCDialogue> DialogueEditor { get; private set; }
        public static IEditor<NPCVendor> VendorEditor { get; private set; }
        public static IEditor<NPCQuest> QuestEditor { get; private set; }
        public static void SaveAllEditors()
        {
            CharacterEditor.Save();
            DialogueEditor.Save();
            VendorEditor.Save();
            QuestEditor.Save();
        }
        public static void ResetEditors()
        {
            CharacterEditor.Reset();
            DialogueEditor.Reset();
            VendorEditor.Reset();
            QuestEditor.Reset();
        }
        #endregion
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
        }
        public new void Show()
        {
            CharacterEditor = new CharacterEditor();
            DialogueEditor = new DialogueEditor();
            VendorEditor = new VendorEditor();
            QuestEditor = new QuestEditor();
            DeepAnalysisManager = new Mistakes.DeepAnalysisManager();
            Width *= AppConfig.Instance.scale;
            Height *= AppConfig.Instance.scale;
            MinWidth *= AppConfig.Instance.scale;
            MinHeight *= AppConfig.Instance.scale;
            Proxy = new PropertyProxy(this);
            Proxy.RegisterEvents();
            #region THEME SETUP
            var theme = ThemeManager.Themes.ContainsKey(AppConfig.Instance.currentTheme ?? "") ? ThemeManager.Themes[AppConfig.Instance.currentTheme] : ThemeManager.Themes["Metro/LightGreen"];
            ThemeManager.Apply(theme);
            #endregion
            #region OPEN_WITH
            string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            App.Logger.LogInfo($"Command Line Args: {string.Join(";", args)}");
            if (args?.Length >= 1)
            {
                try
                {
                    CurrentProject.file = args[0];
                    if (CurrentProject.Load(new NPCProject()))
                    {
                        App.NotificationManager.Clear();
                        App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Loaded"));
                        AddToRecentList(CurrentProject.file);
                    }
                    else
                    {
                        CurrentProject.file = "";
                    }
                }
                catch { }
            }
            #endregion
            #region APPAREL SETUP
            faceImageIndex.Maximum = faceAmount - 1;
            beardImageIndex.Maximum = beardAmount - 1;
            hairImageIndex.Maximum = haircutAmount - 1;
            (CharacterEditor as CharacterEditor).FaceImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, 0));
            (CharacterEditor as CharacterEditor).HairImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, 0));
            (CharacterEditor as CharacterEditor).BeardImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, 0));
            #endregion
            RefreshRecentList();
            #region AFTER UPDATE
            try
            {
                if (File.Exists(AppConfig.Directory + "updater.exe"))
                {
                    Proxy.WhatsNew_Menu_Click(null, null);
                    File.Delete(AppConfig.Directory + "updater.exe");
                    App.Logger.LogInfo("Updater deleted.");
                }
                else if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "updater.exe"))
                {
                    Proxy.WhatsNew_Menu_Click(null, null);
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "updater.exe");
                    App.Logger.LogInfo("Updater deleted.");
                }
            }
            catch { App.Logger.LogWarning("Can't delete updater."); }
            #endregion
            #region AUTOSAVE INIT
            if (AppConfig.Instance.autosaveOption > 0)
            {
                AutosaveTimer = new DispatcherTimer();
                switch (AppConfig.Instance.autosaveOption)
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
            #region DISCORD
            DiscordManager = new DiscordRPC.DiscordManager(1000)
            {
                descriptive = AppConfig.Instance.enableDiscord
            };
            DiscordManager?.Initialize();
            Proxy.TabControl_SelectionChanged(mainTabControl, null);
            #endregion
            #region ENABLE EXPERIMENTAL
            if (AppConfig.Instance.experimentalFeatures)
            {

            }
            #endregion
            HolidayManager.Check();
            App.NotificationManager.Notify(LocUtil.LocalizeInterface("app_Free"));
            ConsoleLogger.StartWaitForInput();
            base.Show();
        }
        #region CONSTANTS
        public const int
        faceAmount = 32,
        beardAmount = 16,
        haircutAmount = 23;
        #endregion
        #region STATIC
        public static MainWindow Instance;
        public static ProjectData CurrentProject { get; private set; } = new ProjectData();
        public static DispatcherTimer AutosaveTimer { get; set; }
        public static PropertyProxy Proxy { get; private set; }
        public static bool IsRGB { get; set; } = true;
        public static DateTime Started { get; set; } = DateTime.UtcNow;
        #endregion
        private void AutosaveTimer_Tick(object sender, EventArgs e)
        {
            AutosaveTimer.Stop();
            if (CurrentProject.file.Length > 0)
                CurrentProject.Save();
            AutosaveTimer.Start();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            PerformExit();
            base.OnClosing(e);
        }
        public static void AddToRecentList(string path)
        {
            RecentFileList recent = new RecentFileList();
            recent.Load(new string[0]);
            if (!recent.data.Contains(MainWindow.CurrentProject.file))
            {
                var r = recent.data.AsEnumerable();
                r = r.Prepend(path);
                recent.data = r.ToArray();
                recent.Save();
            }
            Instance.RefreshRecentList();
        }

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
                string oldPath = MainWindow.CurrentProject.file;
                MainWindow.CurrentProject.file = path;
                if (MainWindow.CurrentProject.Load(null))
                {
                    App.NotificationManager.Clear();
                    App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Loaded"));
                    AddToRecentList(CurrentProject.file);
                }
                else
                {
                    MainWindow.CurrentProject.file = oldPath;
                    App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_NotLoaded"));
                }
            }
            dropOverlay.Visibility = Visibility.Hidden;
        }
        #endregion
        public void RefreshRecentList()
        {
            RecentFileList recent = new RecentFileList();
            recent.Load(new string[0]);
            RecentList.Items.Clear();
            recent.data = recent.data.Where(d => File.Exists(d)).ToArray();
            foreach (var k in recent.data)
            {
                var mItem = new MenuItem()
                {
                    Header = k,
                    Tag = "SAV"
                };
                mItem.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
                {
                    string oldPath = MainWindow.CurrentProject.file;
                    MainWindow.CurrentProject.file = k;
                    if (MainWindow.CurrentProject.Load(null))
                    {
                        App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Loaded"));
                        ResetEditors();
                    }
                    else
                    {
                        MainWindow.CurrentProject.file = oldPath;
                        App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_NotLoaded"));
                    }
                });
                RecentList.Items.Add(mItem);
            }
            if (recent.data.Length > 0)
            {
                RecentList.Items.Add(new Separator());
                var mItem = new MenuItem()
                {
                    Header = LocUtil.LocalizeInterface("menu_File_Recent_Clear"),
                    Tag = "CLR"
                };
                mItem.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
                {
                    recent.data = new string[0];
                    recent.Save();
                    RefreshRecentList();
                });
                RecentList.Items.Add(mItem);
            }
            recent.Save();
        }

        public static void PerformExit()
        {
            if (MainWindow.CurrentProject.SavePrompt() == null)
                return;
            App.Logger.LogInfo("Closing app");
            DiscordManager?.Deinitialize();
            Environment.Exit(0);
        }
    }
}