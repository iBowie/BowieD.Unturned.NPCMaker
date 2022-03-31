using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.Managers;
using BowieD.Unturned.NPCMaker.Mistakes;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Themes;
using BowieD.Unturned.NPCMaker.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace BowieD.Unturned.NPCMaker
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            MainWindowViewModel = new MainWindowViewModel(this);
            DataContext = MainWindowViewModel;
        }
        public MainWindowViewModel MainWindowViewModel { get; }
        #region MANAGERS
        public static DiscordRPC.DiscordManager DiscordManager { get; set; }
        #endregion
        public new void Show()
        {
            #region THEME SETUP
            ThemeManager.Init(AppConfig.Instance.accentColor, AppConfig.Instance.useDarkMode);
            SetBackground(AppConfig.Instance.mainWindowBackgroundImage, AppConfig.Instance.useDarkMode);
            #endregion

            ImportAssetsForm iaf = new ImportAssetsForm();
            iaf.ShowDialog();

            Width *= AppConfig.Instance.scale;
            Height *= AppConfig.Instance.scale;
            MinWidth *= AppConfig.Instance.scale;
            MinHeight *= AppConfig.Instance.scale;
            #region OPEN_WITH
            string[] args = Environment.GetCommandLineArgs();
            App.Logger.Log($"Command Line Args: {string.Join(";", args)}");
            if (args?.Length >= 2)
            {
                for (int k = 1; k < args.Length; k++)
                {
                    try
                    {
                        CurrentProject.file = args[k];
                        if (CurrentProject.Load(new NPCProject()))
                        {
                            App.NotificationManager.Clear();
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Project_Loaded"]);
                            AddToRecentList(CurrentProject.file);
                            break;
                        }
                        else
                        {
                            CurrentProject.file = "";
                        }
                    }
                    catch { }
                }
            }
            #endregion
            #region APPAREL SETUP
            faceImageIndex.Maximum = faceAmount - 1;
            beardImageIndex.Maximum = beardAmount - 1;
            hairImageIndex.Maximum = haircutAmount - 1;
            //(CharacterEditor as CharacterEditor).FaceImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, 0));
            //(CharacterEditor as CharacterEditor).HairImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, 0));
            //(CharacterEditor as CharacterEditor).BeardImageIndex_Changed(null, new RoutedPropertyChangedEventArgs<double?>(0, 0));
            #endregion
            RefreshRecentList();
            #region AFTER UPDATE
            try
            {
                string updaterPath = Path.Combine(AppConfig.Directory, "updater.exe");
                if (File.Exists(updaterPath))
                {
                    OpenPatchNotes();
                    File.Delete(updaterPath);
                    App.Logger.Log("Updater deleted.");
                }
            }
            catch { App.Logger.Log("Can't delete updater.", ELogLevel.WARNING); }
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
            #region AppUpdate
            AppUpdateTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            AppUpdateTimer.Tick += AppUpdateTimer_Tick;
            AppUpdateTimer.Start();
            #endregion
            #region ErrorCheck
            ErrorCheckTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            ErrorCheckTimer.Tick += ErrorCheckTimer_Tick;

            if (AppConfig.Instance.automaticallyCheckForErrors)
            {
                statusNoErrorsItem.Visibility = Visibility.Visible;
                ErrorCheckTimer.Start();
            }
            else
            {
                statusNoErrorsItem.Visibility = Visibility.Collapsed;
                ErrorCheckTimer.Stop();
            }
            #endregion
            #region VERSION SPECIFIC CODE
#if !DEBUG
            debugOverlayText.Visibility = Visibility.Collapsed;
#endif
#if PREVIEW
            previewOverlayText.Visibility = Visibility.Visible;
#endif
            #endregion
            #region DISCORD
            DiscordManager = new DiscordRPC.DiscordManager(1000)
            {
                descriptive = AppConfig.Instance.enableDiscord
            };
            DiscordManager?.Initialize();
            MainWindowViewModel.TabControl_SelectionChanged(mainTabControl, null);
            #endregion
            #region ENABLE EXPERIMENTAL
            if (AppConfig.Instance.experimentalFeatures)
            {

            }
            #endregion
            #region CONTEXT MENUS

            #endregion
            HolidayManager.Check();
            if (App.Package.Guides.Count > 0)
            {
                foreach (KeyValuePair<string, string> guide in App.Package.Guides)
                {
                    guidesMenuItem.Items.Add(new MenuItem()
                    {
                        Header = guide.Key,
                        Command = new BaseCommand(() =>
                        {
                            System.Diagnostics.Process.Start(guide.Value);
                        })
                    });
                }
            }
            else
            {
                guidesMenuItem.IsEnabled = false;
            }

            if (string.IsNullOrEmpty(App.Package.GetTemplatesURL))
            {
                getTemplatesMenuItem.IsEnabled = false;
            }
            else
            {
                getTemplatesMenuItem.Click += (sender, e) =>
                {
                    Process.Start(App.Package.GetTemplatesURL);
                };
            }

            if (App.Package.FeedbackLinks.Length > 0)
            {
                foreach (Data.AppPackage.FeedbackLink link in App.Package.FeedbackLinks)
                {
                    MenuItem newItem = new MenuItem()
                    {
                        Header = link.Localize ? LocalizationManager.Current.Interface[link.Text] : link.Text,
                        Command = new BaseCommand(() =>
                        {
                            System.Diagnostics.Process.Start(link.URL);
                        })
                    };
                    if (!string.IsNullOrEmpty(link.Icon))
                    {
                        try
                        {
                            newItem.Icon = new Image()
                            {
                                Width = 16,
                                Height = 16,
                                Source = new BitmapImage(new Uri(link.Icon))
                            };
                        }
                        catch (Exception ex)
                        {
                            App.Logger.LogException("Could not load feedback icon", ex: ex);
                        }
                    }
                    commMenuItem.Items.Add(newItem);
                }
            }
            else
            {
                commMenuItem.IsEnabled = false;
            }

            try
            {
                foreach (Data.AppPackage.Notification n in App.Package.Notifications)
                {
                    string text = n.Localize ? LocalizationManager.Current.Notification.Translate(n.Text) : n.Text;

                    List<Button> _buttons = new List<Button>();

                    foreach (Data.AppPackage.Notification.Button b in n.Buttons)
                    {
                        string bText = b.Localize ? LocalizationManager.Current.Notification.Translate(b.Text) : b.Text;

                        Button elem = new Button()
                        {
                            Content = new TextBlock()
                            {
                                Text = bText
                            }
                        };
                        elem.Click += (_, __) => b.GetButtonAction().Invoke();

                        _buttons.Add(elem);
                    }

                    App.NotificationManager.Notify(text, buttons: _buttons.ToArray());
                }
            }
            catch (Exception ex)
            {
                App.Logger.LogException("Could not display notification(s)", ex: ex);
            }

            if (App.Package.Patrons.Length > 0)
            {
                var pList = App.Package.Patrons.ToList();
                pList.Shuffle();
                string pjoined = string.Join(", ", pList.Take(5));

                App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("StartUp_Patreon_Patrons", pjoined));
            }

            Loaded += (sender, e) =>
            {
                var scr = ScreenHelper.GetCurrentScreen(this);

                var sz = scr.Size;

                if (sz.Width < MinWidth || sz.Height < MinHeight)
                {
                    var res = MessageBox.Show(LocalizationManager.Current.Interface["Warning_Scale"], Title, MessageBoxButton.YesNo);

                    if (res == MessageBoxResult.Yes)
                    {
                        MinWidth /= AppConfig.Instance.scale;
                        MinHeight /= AppConfig.Instance.scale;
                        Width /= AppConfig.Instance.scale;
                        Height /= AppConfig.Instance.scale;

                        this.Left = (sz.Width - Width) / 2 + sz.Left;
                        this.Top = (sz.Height - Height) / 2 + sz.Top;

                        Application.Current.Resources["Scale"] = 1.0;

                        AppConfig.Instance.scale = 1.0;

                        AppConfig.Instance.Save();
                    }
                }

                if (GameAssetManager.HasImportedAssets)
                {
                    if (!File.Exists(Path.Combine(AppConfig.Instance.unturnedDir, "Extras", "Icons", "Bag_MRE_81.png")))
                    {
                        MessageBox.Show(LocalizationManager.Current.Notification["StartUp_IconsNotGenerated"], Title, MessageBoxButton.OK);
                    }
                }

                if (!CurrentProject.hasLoadedAtLeastOnce)
                {
                    string crashSavePath = Path.Combine(AppConfig.ExeDirectory, App.CRASH_SAVE_FILENAME);

                    if (File.Exists(crashSavePath))
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(crashSavePath);

                            var lastWrite = fi.LastWriteTime;
                            var creationTime = fi.CreationTime;

                            DateTime latestTime = lastWrite > creationTime ? lastWrite : creationTime;

                            var res = MessageBox.Show(LocalizationManager.Current.Notification.Translate("RecoverOnStart", latestTime), LocalizationManager.Current.Notification["RecoverOnStart_Title"], MessageBoxButton.YesNo);

                            if (res == MessageBoxResult.Yes)
                            {
                                CurrentProject.file = crashSavePath;
                                CurrentProject.Load(null);
                                CurrentProject.file = string.Empty;

                            }

                            File.Delete(crashSavePath);
                        }
                        catch (Exception ex)
                        {
                            App.Logger.LogException("Could not load crash save.", ex: ex);
                        }
                    }
                }
            };

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
        public static ProjectData CurrentProject => ProjectData.CurrentProject;
        public static DispatcherTimer AutosaveTimer { get; set; }
        public static DispatcherTimer AppUpdateTimer { get; set; }
        public static DispatcherTimer ErrorCheckTimer { get; set; }
        public static DateTime Started { get; set; } = DateTime.UtcNow;
        #endregion
        private void AutosaveTimer_Tick(object sender, EventArgs e)
        {
            AutosaveTimer.Stop();
            if (string.IsNullOrEmpty(CurrentProject.file))
            {
                var old = CurrentProject.file;
                CurrentProject.file = Path.Combine(AppConfig.ExeDirectory, "autosave.npcproj");
                CurrentProject.Save();
                CurrentProject.file = old;
            }
            else
            {
                CurrentProject.Save();
            }

            AutosaveTimer.Start();
        }
        private void AppUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentProject.file))
            {
                menuCurrentFileLabel.Content = LocalizationManager.Current.Interface.Translate("Main_Menu_CurrentFile", LocalizationManager.Current.Interface["Main_Menu_CurrentFile_None"]);
            }
            else
            {
                menuCurrentFileLabel.Content = LocalizationManager.Current.Interface.Translate("Main_Menu_CurrentFile", Path.GetFileName(CurrentProject.file));
            }

            menuCurrentFileLabel.ToolTip = CurrentProject.file;
        }
        private void ErrorCheckTimer_Tick(object sender, EventArgs e)
        {
            MistakesManager.FindMistakes();

            var errorCount = MistakesManager.Criticals_Count;
            var warnCount = MistakesManager.Warnings_Count;

            if (errorCount > 0)
            {
                statusErrorsItem.Visibility = Visibility.Visible;

                statusErrorsItemText.Text = LocalizationManager.Current.Interface.Translate("Main_Status_Errors_Errors", errorCount);
            }
            else
            {
                statusErrorsItem.Visibility = Visibility.Collapsed;
            }

            if (warnCount > 0)
            {
                statusWarningsItem.Visibility = Visibility.Visible;

                statusWarningsItemText.Text = LocalizationManager.Current.Interface.Translate("Main_Status_Errors_Warnings", warnCount);
            }
            else
            {
                statusWarningsItem.Visibility = Visibility.Collapsed;
            }

            if (errorCount == 0 && warnCount == 0)
            {
                statusNoErrorsItem.Visibility = Visibility.Visible;
            }
            else
            {
                statusNoErrorsItem.Visibility = Visibility.Collapsed;
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            PerformExit();
            base.OnClosing(e);
        }
        public static void AddToRecentList(string path)
        {
            if (!DataManager.RecentFileData.data.Contains(MainWindow.CurrentProject.file))
            {
                IEnumerable<string> r = DataManager.RecentFileData.data.AsEnumerable();
                r = r.Prepend(path);
                DataManager.RecentFileData.data = r.ToArray();
                DataManager.RecentFileData.Save();
            }
            Instance.RefreshRecentList();
        }

        public void SetBackground(string imagePath, bool darkMode)
        {
            try
            {
                BitmapImage bmp = new BitmapImage();

                bmp.BeginInit();

                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = new Uri(imagePath);

                bmp.EndInit();

                bmp.Freeze();

                mainWindowBackgroundImage.Source = bmp;
            }
            catch
            {
                mainWindowBackgroundImage.Source = null;
            }

            if (darkMode)
            {
                mainWindowBackgroundImage.Opacity = 0.25;
            }
            else
            {
                mainWindowBackgroundImage.Opacity = 0.75;
            }

            mainWindowBackgroundImageBlurEffect.Radius = MathUtil.Clamp(AppConfig.Instance.mainWindowBackgroundImageBlurRadius, 0, 50);
        }

        #region DRAG AND DROP
        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                dropOverlay.Visibility = Visibility.Visible;
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
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification["Project_Loaded"]);
                    AddToRecentList(CurrentProject.file);
                }
                else
                {
                    MainWindow.CurrentProject.file = oldPath;
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification["Project_NotLoaded_Incompatible"]);
                }
            }
            dropOverlay.Visibility = Visibility.Hidden;
        }
        #endregion
        public void RefreshRecentList()
        {
            RecentList.Items.Clear();
            DataManager.RecentFileData.data = DataManager.RecentFileData.data.Where(d => File.Exists(d)).ToArray();
            foreach (string k in DataManager.RecentFileData.data)
            {
                MenuItem mItem = new MenuItem()
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
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["Project_Loaded"]);
                    }
                    else
                    {
                        MainWindow.CurrentProject.file = oldPath;
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["Project_NotLoaded_Incompatible"]);
                    }
                });
                RecentList.Items.Add(mItem);
            }
            if (DataManager.RecentFileData.data.Length > 0)
            {
                RecentList.Items.Add(new Separator());
                MenuItem mItem = new MenuItem()
                {
                    Header = LocalizationManager.Current.Interface["Main_Menu_File_Recent_Clear"],
                    Tag = "CLR"
                };
                mItem.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
                {
                    DataManager.RecentFileData.data = new string[0];
                    DataManager.RecentFileData.Save();
                    RefreshRecentList();
                });
                RecentList.Items.Add(mItem);
            }
            DataManager.RecentFileData.Save();
        }

        public static void PerformExit()
        {
            if (MainWindow.CurrentProject.SavePrompt() == null)
            {
                return;
            }

            App.Logger.Log("Closing app");
            DiscordManager?.Deinitialize();
            Environment.Exit(0);
        }
        internal void OpenPatchNotes()
        {
            try
            {
                new Whats_New().ShowDialog();
            }
            catch (Exception ex)
            {
                App.Logger.LogException("Could not open update notes window.", ex: ex);
            }
        }
    }
}