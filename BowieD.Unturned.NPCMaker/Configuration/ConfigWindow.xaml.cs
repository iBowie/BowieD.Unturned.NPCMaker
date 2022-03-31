using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Themes;
using BowieD.Unturned.NPCMaker.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace BowieD.Unturned.NPCMaker.Configuration
{
    /// <summary>
    /// Логика взаимодействия для ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window, INotifyPropertyChanged
    {
        public ConfigWindow()
        {
            InitializeComponent();
            Width *= AppConfig.Instance.scale;
            Height *= AppConfig.Instance.scale;
            
            DataContext = this;

            CurrentConfig = AppConfig.Instance;

            void addStandardColor(string name, string hex)
            {
                Coloring.Color color = new Coloring.Color(hex);

                AccentColor_Picker.StandardColors.Add(new Xceed.Wpf.Toolkit.ColorItem(color, name));
            }

            AccentColor_Picker.StandardColors = new System.Collections.ObjectModel.ObservableCollection<Xceed.Wpf.Toolkit.ColorItem>();

            addStandardColor("Amber", "#F0A30A");
            addStandardColor("Blue", "#119EDA");
            addStandardColor("Brown", "#825A2C");
            addStandardColor("Cobalt", "#0050EF");
            addStandardColor("Crimson", "#A20025");
            addStandardColor("Cyan", "#1BA1E2");
            addStandardColor("Emerald", "#008A00");
            addStandardColor("Green", "#60A917");
            addStandardColor("Indigo", "#6A00FF");
            addStandardColor("Lime", "#A4C400");
            addStandardColor("Magenta", "#D80073");
            addStandardColor("Mauve", "#76608A");
            addStandardColor("Olive", "#6D8764");
            addStandardColor("Orange", "#FA6800");
            addStandardColor("Pink", "#F472D0");
            addStandardColor("Purple", "#6459DF");
            addStandardColor("Red", "#E51400");
            addStandardColor("Sienna", "#A0522D");
            addStandardColor("Steel", "#647687");
            addStandardColor("Taupe", "#87794E");
            addStandardColor("Teal", "#00ABA9");
            addStandardColor("Violet", "#AA00FF");
            addStandardColor("Yellow", "#FEDE06");
        }

        private EExportSchema _currentExportSchema;
        public EExportSchema CurrentExportSchema
        {
            get => _currentExportSchema;
            set
            {
                _currentExportSchema = value;

                string disp;

                switch (value)
                {
                    case EExportSchema.Default:
                        disp = ExportSchemaStrings.SCHEMA_DEFAULT;
                        break;
                    case EExportSchema.GUID_All_The_Way:
                        disp = ExportSchemaStrings.SCHEMA_GUID_ALL_THE_WAY;
                        break;
                    case EExportSchema.Verbose_Comment:
                        disp = ExportSchemaStrings.SCHEMA_VERBOSE_COMMENT;
                        break;
                    case EExportSchema.Verbose_No_Comment:
                        disp = ExportSchemaStrings.SCHEMA_VERBOSE_NO_COMMENT;
                        break;
                    default:
                        disp = "what";
                        break;
                }

                ExportSchema_Structure_Preview_TextBlock.Text = disp;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentExportSchema)));
            }
        }
        public string[] CurrentDisabledErrors
        {
            get
            {
                IEnumerable<string> iterate(FrameworkElement element)
                {
                    if (element is Panel panel)
                    {
                        foreach (var e in panel.Children)
                        {
                            if (e is FrameworkElement fe)
                            {
                                foreach (var r in iterate(fe))
                                    yield return r;
                            }
                        }
                    }

                    if (element is ContentControl contentControl)
                    {
                        if (contentControl.Content is FrameworkElement fe)
                        {
                            foreach (var r in iterate(fe))
                                yield return r;
                        }
                    }

                    if (element is CheckBox cbox)
                    {
                        if (cbox.IsChecked == true)
                        {
                            yield return cbox.Tag.ToString();
                        }
                    }
                }

                return iterate(disabledErrorsPanel).ToArray();
            }
            set
            {
                void iterate(FrameworkElement element)
                {
                    if (element is Panel panel)
                    {
                        foreach (var e in panel.Children)
                        {
                            if (e is FrameworkElement fe)
                            {
                                iterate(fe);
                            }
                        }
                    }

                    if (element is ContentControl contentControl)
                    {
                        if (contentControl.Content is FrameworkElement fe)
                        {
                            iterate(fe);
                        }
                    }

                    if (element is CheckBox cbox)
                    {
                        cbox.IsChecked = value.Contains(cbox.Tag.ToString());
                    }
                }

                iterate(disabledErrorsPanel);
            }
        }

        public AppConfig CurrentConfig
        {
            get => new AppConfig
            {
                accentColor = AccentColor_Picker.SelectedColor ?? new Coloring.Color("#FFFFFF"),
                useDarkMode = UseDarkMode_CheckBox.IsChecked.Value,
                autosaveOption = (byte)Autosave_Box.SelectedIndex,
                language = (ELanguage)(Languages_Box.SelectedItem as ComboBoxItem).Tag,
                exportSchema = CurrentExportSchema,
                scale = double.Parse((Scale_Box.SelectedItem as ComboBoxItem).Tag.ToString(), CultureInfo.InvariantCulture),
                enableDiscord = Discord_Enabled_Box.IsChecked.Value,
                generateGuids = Generate_GUIDS_Box.IsChecked.Value,
                experimentalFeatures = Experimental_Box.IsChecked.Value,
                animateControls = Animation_Enabled_Box.IsChecked.Value,
                autoUpdate = Autoupdate_Box.IsChecked.Value,
                downloadPrerelease = DownloadPrerelease_Box.IsChecked.Value,
                alternateLogicTranslation = AlternateLogicTranslation_Box.IsChecked.Value,
                replaceMissingKeysWithEnglish = ReplaceMissingKeysWithEnglish_Box.IsChecked.Value,
                useCommentsInsteadOfData = UseCommentsInsteadOfData_Box.IsChecked.Value,
                importVanilla = Import_Vanilla_Box.IsChecked.Value,
                importWorkshop = Import_Workshop_Box.IsChecked.Value,
                generateThumbnailsBeforehand = Generate_Thumbnails_Box.IsChecked.Value,
                highlightSearch = Highlight_Search_Box.IsChecked.Value,
                useOldStyleMoveUpDown = Use_Old_Style_Move_Up_Down_Box.IsChecked.Value,
                automaticallyCheckForErrors = AutomaticallyCheckForErrors_Box.IsChecked.Value,
                disabledErrors = CurrentDisabledErrors,
                preferLegacyIDsOverGUIDs = PreferLegacyIDsOverGUIDs_Box.IsChecked.Value,
                autoCloseOpenBoomerangs = Auto_Close_Open_Boomerangs_Box.IsChecked.Value,
                unturnedDir = curUntDir,
                mainWindowBackgroundImage = curMainWindowBackgroundPath,
                mainWindowBackgroundImageBlurRadius = MainWindowBackgroundBlurRadius_Slider.Value,
                alternateBoolValue = AlternateBoolValue_Box.IsChecked.Value,
                forceSoftwareRendering = ForceSoftwareRendering_Box.IsChecked.Value,
            };
            set
            {
                UseDarkMode_CheckBox.IsChecked = value.useDarkMode;
                AccentColor_Picker.SelectedColor = value.accentColor;
                Autosave_Box.SelectedIndex = value.autosaveOption;
                foreach (ELanguage lang in LocalizationManager.SupportedLanguages())
                {
                    ComboBoxItem cbi = new ComboBoxItem
                    {
                        Content = LocalizationManager.GetLanguageName(lang),
                        Tag = lang
                    };
                    Languages_Box.Items.Add(cbi);
                    if (lang == value.language)
                    {
                        Languages_Box.SelectedItem = cbi;
                    }
                }
                foreach (ComboBoxItem cbi in Scale_Box.Items)
                {
                    if (cbi?.Tag.ToString() == value.scale.ToString().Replace(',', '.'))
                    {
                        Scale_Box.SelectedItem = cbi;
                        break;
                    }
                }
                CurrentExportSchema = value.exportSchema;
                Experimental_Box.IsChecked = value.experimentalFeatures;
                Discord_Enabled_Box.IsChecked = value.enableDiscord;
                Generate_GUIDS_Box.IsChecked = value.generateGuids;
                Animation_Enabled_Box.IsChecked = value.animateControls;
                Autoupdate_Box.IsChecked = value.autoUpdate;
                DownloadPrerelease_Box.IsChecked = value.downloadPrerelease;
                AlternateLogicTranslation_Box.IsChecked = value.alternateLogicTranslation;
                ReplaceMissingKeysWithEnglish_Box.IsChecked = value.replaceMissingKeysWithEnglish;
                UseCommentsInsteadOfData_Box.IsChecked = value.useCommentsInsteadOfData;
                Import_Vanilla_Box.IsChecked = value.importVanilla;
                Import_Workshop_Box.IsChecked = value.importWorkshop;
                Generate_Thumbnails_Box.IsChecked = value.generateThumbnailsBeforehand;
                Highlight_Search_Box.IsChecked = value.highlightSearch;
                Use_Old_Style_Move_Up_Down_Box.IsChecked = value.useOldStyleMoveUpDown;
                AutomaticallyCheckForErrors_Box.IsChecked = value.automaticallyCheckForErrors;
                CurrentDisabledErrors = value.disabledErrors;
                PreferLegacyIDsOverGUIDs_Box.IsChecked = value.preferLegacyIDsOverGUIDs;
                Auto_Close_Open_Boomerangs_Box.IsChecked = value.autoCloseOpenBoomerangs;
                curUntDir = value.unturnedDir;
                curMainWindowBackgroundPath = value.mainWindowBackgroundImage;
                MainWindowBackgroundBlurRadius_Slider.Value = value.mainWindowBackgroundImageBlurRadius;
                AlternateBoolValue_Box.IsChecked = value.alternateBoolValue;
                ForceSoftwareRendering_Box.IsChecked = value.forceSoftwareRendering;
            }
        }

        private string curUntDir;
        private string curMainWindowBackgroundPath;

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            AppConfig currentConfig = CurrentConfig;

            currentConfig.Save();
            AppConfig.Instance.Apply(currentConfig, out var hasToRestart);

            if (hasToRestart)
            {
                App.NotificationManager.Notify(LocalizationManager.Current.Notification["Configuration_OnExit"]);
            }
            else
            {
                App.NotificationManager.Notify(LocalizationManager.Current.Notification["Configuration_OnExit_NoRestart"]);
            }

            Close();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(LocalizationManager.Current.Notification["Configuration_Default_Confirm"], "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                AppConfig.Instance.LoadDefaults();
                Close();
            }
        }

        private ICommand importChangeFolderCommand, importResetFolderCommand;
        private ICommand changeMainWindowBackgroundImageCommand, resetMainWindowBackgroundImageCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ImportChangeFolderCommand
        {
            get
            {
                if (importChangeFolderCommand == null)
                {
                    importChangeFolderCommand = new BaseCommand(() =>
                    {
                        CommonOpenFileDialog cofd = new CommonOpenFileDialog
                        {
                            IsFolderPicker = true,
                            Multiselect = false,
                            RestoreDirectory = false,
                            InitialDirectory = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Unturned",
                            Title = LocalizationManager.Current.General.Translate("ImportGameAssets_Directory_Title"),
                        };

                        switch (cofd.ShowDialog())
                        {
                            case CommonFileDialogResult.Ok:
                                {
                                    curUntDir = cofd.FileName;
                                }
                                break;
                        }
                    });
                }

                return importChangeFolderCommand;
            }
        }
        public ICommand ImportResetFolderCommand
        {
            get
            {
                if (importResetFolderCommand == null)
                {
                    importResetFolderCommand = new AdvancedCommand(() =>
                    {
                        curUntDir = null;
                    }, (arg) =>
                    {
                        return !string.IsNullOrEmpty(curUntDir);
                    });
                }

                return importResetFolderCommand;
            }
        }
        public ICommand ChangeMainWindowBackgroundImageCommand
        {
            get
            {
                if (changeMainWindowBackgroundImageCommand is null)
                {
                    changeMainWindowBackgroundImageCommand = new BaseCommand(() =>
                    {
                        OpenFileDialog ofd = new OpenFileDialog()
                        {
                            Filter = "Supported image formats|*.bmp;*.jpeg;*.jpg;*.png;*.tiff;*.tif",
                            Multiselect = false,
                            CheckFileExists = true,
                        };

                        if (ofd.ShowDialog() == true)
                        {
                            curMainWindowBackgroundPath = ofd.FileName;
                        }
                    });
                }

                return changeMainWindowBackgroundImageCommand;
            }
        }
        public ICommand ResetMainWindowBackgroundImageCommand
        {
            get
            {
                if (resetMainWindowBackgroundImageCommand is null)
                {
                    resetMainWindowBackgroundImageCommand = new AdvancedCommand(() =>
                    {
                        curMainWindowBackgroundPath = string.Empty;
                    }, (p) =>
                    {
                        return !string.IsNullOrEmpty(curMainWindowBackgroundPath);
                    });
                }

                return resetMainWindowBackgroundImageCommand;
            }
        }
    }
}
