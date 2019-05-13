using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.Editors;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Notification;
using BowieD.Unturned.NPCMaker.NPC;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker
{
    public partial class MainWindow : Window
    {
        #if DEBUG
        public static LogWindow LogWindow { get; private set; } = new LogWindow();
#endif
        #region MANAGERS
        public static INotificationManager NotificationManager { get; private set; } = new NotificationManager();
        public static Mistakes.DeepAnalysisManager DeepAnalysisManager { get; private set; }
        public static DiscordRPC.DiscordManager DiscordManager { get; set; }
        #endregion
        #region EDITORS
        public static IEditor<NPCCharacter> CharacterEditor { get; private set; }
        public static IEditor<NPCDialogue> DialogueEditor { get; private set; }
        public static IEditor<NPCVendor> VendorEditor { get; private set; }
        public static IEditor<NPCQuest> QuestEditor { get; private set; }
        //public static IEditor<NPCObject> ObjectEditor { get; private set; }
        #endregion
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            CharacterEditor = new CharacterEditor();
            DialogueEditor = new DialogueEditor();
            VendorEditor = new VendorEditor();
            QuestEditor = new QuestEditor();
            //ObjectEditor = new ObjectEditor();
            DeepAnalysisManager = new Mistakes.DeepAnalysisManager();
            if (Config.Configuration.Properties == null)
                Config.Configuration.Load();
            Width *= Config.Configuration.Properties.scale;
            Height *= Config.Configuration.Properties.scale;
            Logger.Log($"Launch stage. Version: {Version}.");
            Proxy = new PropertyProxy(this);
            Proxy.RegisterEvents();
            #region THEME SETUP
            (Config.Configuration.Properties.currentTheme ?? Config.Configuration.DefaultTheme).Apply();
            Logger.Log($"Theme set to {(Config.Configuration.Properties.currentTheme ?? Config.Configuration.DefaultTheme).Name}");
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
                                MainWindow.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Loaded"));
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
            foreach (var k in Config.Configuration.Properties.recent)
            {
                RecentList.Items.Add(Config.Configuration.Properties.recent);
            }
            RecentList.Items.Add(new Separator());
            RecentList.Items.Add(new MenuItem()
            {
                Header = LocUtil.LocalizeInterface("menu_File_Recent_Clear")
            });
            #endregion
            #region APPAREL SETUP
            faceImageIndex.Maximum = faceAmount - 1;
            beardImageIndex.Maximum = beardAmount - 1;
            hairImageIndex.Maximum = haircutAmount - 1;
            (CharacterEditor as CharacterEditor).FaceImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, 0));
            (CharacterEditor as CharacterEditor).HairImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, 0));
            (CharacterEditor as CharacterEditor).BeardImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, 0));
            Proxy.UserColorListChanged();
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
                    Logger.Log("Updater deleted.");
                }
            }
            catch { Logger.Log("Can't delete updater."); }
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
            #if DEBUG
            LogWindow.Show();
            #endif
            #endregion
            Config.Configuration.Properties.firstLaunch = false;
            isSaved = true;
            #region DISCORD
            DiscordManager = new DiscordRPC.DiscordManager(1000)
            {
                descriptive = Config.Configuration.Properties.enableDiscord
            };
            DiscordManager?.Initialize();
            Proxy.TabControl_SelectionChanged(mainTabControl, null);
            #endregion
            #region ENABLE EXPERIMENTAL
            if (Config.Configuration.Properties.experimentalFeatures)
            {

            }
            #endregion
            Proxy.ColorSliderChange(null, null);
            MainWindow.NotificationManager.Notify(LocUtil.LocalizeInterface("app_Free"));
        }
        #region CONSTANTS
        public const int
        faceAmount = 32,
        beardAmount = 16,
        haircutAmount = 23;
        public static Version Version => new Version(1, 0, 5, 0);
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
            Config.Configuration.Save();
            e.Cancel = true;
            PerformExit();
            base.OnClosing(e);
        }
        #region SAVE_LOAD
        public static void Save()
        {
            CharacterEditor.Save();
            DialogueEditor.Save();
            VendorEditor.Save();
            QuestEditor.Save();
            //ObjectEditor.Save();
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
                if (saveFile != null && saveFile != "")
                {
                    using (FileStream fs = new FileStream(saveFile, FileMode.Create))
                    using (XmlWriter writer = XmlWriter.Create(fs))
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(NPCProject));
                        ser.Serialize(writer, CurrentProject);
                    }
                    MainWindow.NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Saved"));
                    isSaved = true;
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
                    XmlSerializer oldDeser = new XmlSerializer(typeof(NPCSaveOld));
                    XmlSerializer newDeser = new XmlSerializer(typeof(NPCProject));
                    if (oldDeser.CanDeserialize(reader))
                    {
                        if (MessageBox.Show(LocUtil.LocalizeInterface("save_Old_Content"), LocUtil.LocalizeInterface("save_Old_Title"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            CurrentProject = (NPCProject)(oldDeser.Deserialize(reader) as NPCSaveOld);
                            ConvertNPCToState(CurrentProject);
                            isSaved = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (newDeser.CanDeserialize(reader))
                    {
                        CurrentProject = newDeser.Deserialize(reader) as NPCProject;
                        saveFile = path;
                        ConvertNPCToState(CurrentProject);
                        isSaved = true;
                    }
                }
                NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Loaded"));
                Started = DateTime.UtcNow;
                return true;
            }
            catch (Exception ex) { Logger.Log(ex, Log_Level.Errors); NotificationManager.Notify(LocUtil.LocalizeInterface("load_Incompatible")); return false; }
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
                        XmlSerializer oldDeserializer = new XmlSerializer(typeof(NPC.NPCSaveOld));
                        XmlSerializer newDeserializer = new XmlSerializer(typeof(NPCProject));
                        return oldDeserializer.CanDeserialize(reader) || newDeserializer.CanDeserialize(reader);
                    }
                }
            }
            catch { return false; }
        }

        #endregion
        #region STATE CONVERTERS
        public void ConvertNPCToState(NPCProject save)
        {
            CurrentProject = save;
            CharacterEditor.Current = new NPCCharacter();
            DialogueEditor.Current = new NPCDialogue();
            QuestEditor.Current = new NPCQuest();
            VendorEditor.Current = new NPCVendor();
            //ObjectEditor.Current = new NPCObject();
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
                    NotificationManager.Notify(LocUtil.LocalizeInterface("notify_Loaded"));
                }
            }
            dropOverlay.Visibility = Visibility.Hidden;
        }
        #endregion

        public static void PerformExit()
        {
            if (!isSaved)
            {
                if (!SavePrompt())
                {
                    return;
                }
            }
            Config.Configuration.Save();
            DiscordManager?.Deinitialize();
            Environment.Exit(0);
        }
    }
}