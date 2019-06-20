using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Config
{
    /// <summary>
    /// Логика взаимодействия для ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
            Width *= Configuration.Properties.scale;
            Height *= Configuration.Properties.scale;
            InitThemeList();
            CurrentConfig = Configuration.Properties;
        }

        public Configuration.CFG CurrentConfig
        {
            get
            {
                Configuration.CFG nc = new Configuration.CFG();
                Configuration.CFG cc = Configuration.Properties;
                #region NOT CONFIG INFO
                nc.userColors = cc.userColors;
                nc.recent = cc.recent;
                nc.firstLaunch = cc.firstLaunch;
                #endregion
                nc.currentTheme = (Selected_Theme_Box.SelectedItem as ComboBoxItem).Tag as MetroTheme;
                nc.autosaveOption = (byte)Autosave_Box.SelectedIndex;
                nc.language = (CultureInfo)(Languages_Box.SelectedItem as ComboBoxItem).Tag;
                nc.scale = double.Parse((Scale_Box.SelectedItem as ComboBoxItem).Tag.ToString(), CultureInfo.InvariantCulture);
                nc.enableDiscord = Discord_Enabled_Box.IsChecked.Value;
                nc.generateGuids = Generate_GUIDS_Box.IsChecked.Value;
                nc.experimentalFeatures = Experimental_Box.IsChecked.Value;
                nc.animateControls = Animation_Enabled_Box.IsChecked.Value;
                nc.autoUpdate = Autoupdate_Box.IsChecked.Value;
                return nc;
            }
            set
            {
                foreach (ComboBoxItem cbi in Selected_Theme_Box.Items)
                {
                    if ((cbi?.Tag as MetroTheme).Name == value.currentTheme.Name)
                    {
                        Selected_Theme_Box.SelectedItem = cbi;
                        break;
                    }
                }
                Autosave_Box.SelectedIndex = value.autosaveOption;
                foreach (var lang in LocUtil.SupportedCultures())
                {
                    ComboBoxItem cbi = new ComboBoxItem
                    {
                        Content = lang.NativeName,
                        Tag = lang
                    };
                    Languages_Box.Items.Add(cbi);
                    if (lang.Name == value.language.Name)
                        Languages_Box.SelectedItem = cbi;
                }
                foreach (ComboBoxItem cbi in Scale_Box.Items)
                {
                    if (cbi?.Tag.ToString() == value.scale.ToString().Replace(',', '.'))
                    {
                        Scale_Box.SelectedItem = cbi;
                        break;
                    }
                }
                Experimental_Box.IsChecked = value.experimentalFeatures;
                Discord_Enabled_Box.IsChecked = value.enableDiscord;
                Generate_GUIDS_Box.IsChecked = value.generateGuids;
                Animation_Enabled_Box.IsChecked = value.animateControls;
                Autoupdate_Box.IsChecked = value.autoUpdate;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Force(CurrentConfig);
            Configuration.Save();
            MainWindow.NotificationManager.Notify(LocUtil.LocalizeInterface("config_OnExit"));
            Close();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(LocUtil.LocalizeInterface("config_Default_Confirm"), "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                CurrentConfig = Configuration.GetDefaults();
            }
        }

        private IEnumerable<(string, Theme)> themeList()
        {
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_LightGreen"),
                new MetroTheme()
                {
                    Name = "LightGreen",
                    DictionaryName = "Light.Green",
                    R = 84,
                    G = 142,
                    B = 25
                });
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_LightPink"),
                new MetroTheme()
                {
                    Name = "LightPink",
                    DictionaryName = "Light.Pink",
                    R = 246,
                    G = 142,
                    B = 217
                });
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_LightRed"),
                new MetroTheme()
                {
                    Name = "LightRed",
                    DictionaryName = "Light.Red",
                    R = 234,
                    G = 67,
                    B = 51
                });
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_LightBlue"),
                new MetroTheme()
                {
                    Name = "LightBlue",
                    DictionaryName = "Light.Blue",
                    R = 51,
                    G = 115,
                    B = 242
                });
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_LightPurple"),
                new MetroTheme()
                {
                    Name = "LightPurple",
                    DictionaryName = "Light.Purple",
                    R = 87,
                    G = 78,
                    B = 185
                });
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_LightOrange"),
                new MetroTheme()
                {
                    Name = "LightOrange",
                    DictionaryName = "Light.Orange",
                    R = 255,
                    G = 106,
                    B = 0
                });
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_DarkGreen"),
                new MetroTheme()
                {
                    Name = "DarkGreen",
                    DictionaryName = "Dark.Green",
                    R = 84,
                    G = 142,
                    B = 25
                });
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_DarkPink"),
                new MetroTheme()
                {
                    Name = "DarkPink",
                    DictionaryName = "Dark.Pink",
                    R = 246,
                    G = 142,
                    B = 217
                });
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_DarkRed"),
                new MetroTheme()
                {
                    Name = "DarkRed",
                    DictionaryName = "Dark.Red",
                    R = 234,
                    G = 67,
                    B = 51
                });
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_DarkBlue"),
                new MetroTheme()
                {
                    Name = "DarkBlue",
                    DictionaryName = "Dark.Blue",
                    R = 51,
                    G = 115,
                    B = 242
                });
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_DarkPurple"),
                new MetroTheme()
                {
                    Name = "DarkPurple",
                    DictionaryName = "Dark.Purple",
                    R = 87,
                    G = 78,
                    B = 185
                });
            yield return (
                LocUtil.LocalizeInterface("config_Tab_Appearance_Theme_DarkOrange"),
                new MetroTheme()
                {
                    Name = "DarkOrange",
                    DictionaryName = "Dark.Orange",
                    R = 255,
                    G = 106,
                    B = 0
                });
        }
        private void InitThemeList()
        {
            foreach (var k in themeList())
            {
                Selected_Theme_Box.Items.Add(new ComboBoxItem()
                {
                    Content = k.Item1,
                    Tag = k.Item2
                });
            }
        }
    }
}
