using System;
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
            foreach (ComboBoxItem cbi in Selected_Theme_Box.Items)
            {
                if (cbi?.Tag.ToString() == Configuration.Properties.theme)
                {
                    Selected_Theme_Box.SelectedItem = cbi;
                    break;
                }
            }
            Autosave_Box.SelectedIndex = Configuration.Properties.autosaveOption;
            foreach (var lang in App.Languages)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = lang.NativeName;
                cbi.Tag = lang;
                Languages_Box.Items.Add(cbi);
                if (lang.Name == Configuration.Properties.language.Name)
                    Languages_Box.SelectedItem = cbi;
            }
            foreach (ComboBoxItem cbi in Scale_Box.Items)
            {
                if (cbi?.Tag.ToString() == Configuration.Properties.scale.ToString().Replace(',', '.'))
                {
                    Scale_Box.SelectedItem = cbi;
                    break;
                }
            }
            foreach (ComboBoxItem cbi in Log_Level_Box.Items)
            {
                if (cbi?.Tag.ToString() == Configuration.Properties.LogLevel.ToString())
                {
                    Log_Level_Box.SelectedItem = cbi;
                    break;
                }
            }
            Experimental_Box.IsChecked = Configuration.Properties.experimentalFeatures;
            Discord_Enabled_Box.IsChecked = Configuration.Properties.enableDiscord;
            Generate_GUIDS_Box.IsChecked = Configuration.Properties.generateGuids;
            
        }

        public Configuration.CFG GetConfig
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
                nc.theme = (Selected_Theme_Box.SelectedItem as ComboBoxItem).Tag.ToString();
                nc.autosaveOption = (byte)Autosave_Box.SelectedIndex;
                nc.Language = ((CultureInfo)(Languages_Box.SelectedItem as ComboBoxItem).Tag).Name;
                nc.scale = double.Parse((Scale_Box.SelectedItem as ComboBoxItem).Tag.ToString(), CultureInfo.InvariantCulture);
                nc.enableDiscord = Discord_Enabled_Box.IsChecked.Value;
                nc.generateGuids = Generate_GUIDS_Box.IsChecked.Value;
                nc.experimentalFeatures = Experimental_Box.IsChecked.Value;
                nc.LogLevel = (Logging.Log_Level)Enum.Parse(typeof(Logging.Log_Level), (Log_Level_Box.SelectedItem as ComboBoxItem).Tag.ToString());
                return nc;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Force(GetConfig);
            Configuration.Save();
            MainWindow.Instance.DoNotification((string)TryFindResource("config_OnExit"));
            Close();
        }
    }
}
