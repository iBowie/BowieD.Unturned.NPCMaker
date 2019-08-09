using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Editors;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Managers;
using BowieD.Unturned.NPCMaker.Notification;
using BowieD.Unturned.NPCMaker.NPC;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Themes;
using BowieD.Unturned.NPCMaker.Data;
using System.Collections;
using System.Collections.Generic;

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
            App.Logger.LogInfo($"Launch stage. Version: {Version}.");
            Proxy = new PropertyProxy(this);
            Proxy.RegisterEvents();
            #region THEME SETUP
            var theme = ThemeManager.Themes.ContainsKey(AppConfig.Instance.currentTheme ?? "") ? ThemeManager.Themes[AppConfig.Instance.currentTheme] : ThemeManager.Themes["Metro/LightGreen"];
            ThemeManager.Apply(theme);
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
                                App.NotificationManager.Clear();
                                App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Loaded"));
                            }
                            break;
                        }
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
            isSaved = true;
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
            base.Show();
        }
#region CONSTANTS
        public const int
        faceAmount = 32,
        beardAmount = 16,
        haircutAmount = 23;
        private static Version _readVersion = null;
        public static Version Version
        {
            get
            {
                try
                {

                    if (_readVersion == null)
                        _readVersion = new Version(FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion);
                    return _readVersion;
                }
                catch { return new Version("0.0.0.0"); }
            }
        }
        #endregion
        #region STATIC
        public static MainWindow Instance;
        public static NPCProject CurrentProject { get; set; } = new NPCProject();
        public static DispatcherTimer AutosaveTimer { get; set; }
        public static PropertyProxy Proxy { get; private set; }
        public static bool IsRGB { get; set; } = true;
        public static DateTime Started { get; set; } = DateTime.UtcNow;
#endregion
#region CURRENT SAVE
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
            e.Cancel = true;
            PerformExit();
            base.OnClosing(e);
        }
#region SAVE_LOAD
        public static void Save()
        {
            SaveAllEditors();
            if (saveFile == null || saveFile == "")
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = $"{LocUtil.LocalizeInterface("save_Filter")} (*.npcproj)|*.npcproj",
                    FileName = $"{(CharacterEditor.Current?.editorName?.Length > 0 ? CharacterEditor.Current?.editorName : "Unnamed")}.npcproj",
                    OverwritePrompt = true
                };
                var result = sfd.ShowDialog();
                if (result == true)
                {
                    saveFile = sfd.FileName;
                    AddToRecentList(saveFile);
                }
                else
                {
                    saveFile = oldFile.Length > 0 ? oldFile : saveFile;
                    return;
                }
            }
            try
            {
                CurrentProject.Save(saveFile);
                App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Saved"));
                isSaved = true;
            }
            catch (Exception ex)
            {
                App.NotificationManager.Notify($"Saving failed! Exception: {ex.Message}");
                AppCrashReport acr = new AppCrashReport(ex, false, true);
                acr.ShowDialog();
            }
            if (oldFile != "")
                MainWindow.saveFile = oldFile;
        }
        public static void AddToRecentList(string path)
        {
            RecentFileList recent = new RecentFileList();
            recent.Load(new string[0]);
            if (!recent.data.Contains(saveFile))
            {
                var r = recent.data.AsEnumerable();
                r = r.Prepend(path);
                recent.data = r.ToArray();
                recent.Save();
            }
            Instance.RefreshRecentList();
        }
        public bool Load(string path, bool skipPrompt = false)
        {
            if (!skipPrompt && !SavePrompt())
                return false;
            try
            {
                if (NPCProject.CanLoad(path))
                {
                    CurrentProject = NPCProject.Load(path);
                    saveFile = path;
                    ResetEditors();
                    isSaved = true;
                    AddToRecentList(saveFile);
                    App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Loaded"));
                    Started = DateTime.UtcNow;
                    return true;
                }
                return false;
            }
            catch (Exception ex) { App.Logger.LogException("Error occured while loading project.", ex); App.NotificationManager.Notify(LocUtil.LocalizeInterface("load_Incompatible")); return false; }
        }
        public static bool SavePrompt()
        {
            if (isSaved == true || CurrentProject == new NPCProject())
                return true;
            var result = MessageBox.Show(LocUtil.LocalizeInterface("app_Exit_UnsavedChanges_Text"), LocUtil.LocalizeInterface("app_Exit_UnsavedChanges_Title"), MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
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
                        XmlSerializer deserializer = new XmlSerializer(typeof(NPCProject));
                        return deserializer.CanDeserialize(reader);
                    }
                }
            }
            catch { return false; }
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
                    App.NotificationManager.Clear();
                    App.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Loaded"));
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
                    Load(k);
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
                    RefreshRecentList();
                });
                RecentList.Items.Add(mItem);
            }
            recent.Save();
        }

        public static void PerformExit()
        {
            if (!isSaved)
            {
                if (!SavePrompt())
                {
                    return;
                }
            }
            DiscordManager?.Deinitialize();
            Environment.Exit(0);
        }
    }
}