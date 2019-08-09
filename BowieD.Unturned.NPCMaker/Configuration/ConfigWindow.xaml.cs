using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Themes;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Configuration
{
    /// <summary>
    /// Логика взаимодействия для ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
            Width *= AppConfig.Instance.scale;
            Height *= AppConfig.Instance.scale;
            CurrentConfig = AppConfig.Instance;
        }

        public AppConfig CurrentConfig
        {
            get => new AppConfig
            {
                currentTheme = ((Selected_Theme_Box.SelectedItem as ComboBoxItem).Tag as Theme).Name,
                autosaveOption = (byte)Autosave_Box.SelectedIndex,
                locale = (Languages_Box.SelectedItem as ComboBoxItem).Tag.ToString(),
                scale = double.Parse((Scale_Box.SelectedItem as ComboBoxItem).Tag.ToString(), CultureInfo.InvariantCulture),
                enableDiscord = Discord_Enabled_Box.IsChecked.Value,
                generateGuids = Generate_GUIDS_Box.IsChecked.Value,
                experimentalFeatures = Experimental_Box.IsChecked.Value,
                animateControls = Animation_Enabled_Box.IsChecked.Value,
                autoUpdate = Autoupdate_Box.IsChecked.Value
            };
            set
            {
                foreach (var theme in ThemeManager.Themes)
                {
                    ComboBoxItem cbi = new ComboBoxItem()
                    {
                        Content = theme.Key,
                        Tag = theme.Value
                    };
                    Selected_Theme_Box.Items.Add(cbi);
                    if (theme.Key == value.currentTheme)
                        Selected_Theme_Box.SelectedItem = cbi;
                }
                Autosave_Box.SelectedIndex = value.autosaveOption;
                foreach (var lang in LocUtil.SupportedCultures())
                {
                    ComboBoxItem cbi = new ComboBoxItem
                    {
                        Content = lang.NativeName,
                        Tag = lang.Name
                    };
                    Languages_Box.Items.Add(cbi);
                    if (lang.Name == value.locale)
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
            CurrentConfig.Save();
            App.NotificationManager.Notify(LocUtil.LocalizeInterface("config_OnExit"));
            Close();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(LocUtil.LocalizeInterface("config_Default_Confirm"), "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                AppConfig.Instance.LoadDefaults();
                Close();
            }
        }
    }
}
