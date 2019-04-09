using BowieD.Unturned.NPCMaker.Logging;
using System;
using System.Text;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.BetterForms
{
    /// <summary>
    /// Логика взаимодействия для AppCrashReport.xaml
    /// </summary>
    public partial class AppCrashReport : Window
    {
        public AppCrashReport(Exception exception, bool ableToSave = true, bool ableToHandle = true)
        {
            InitializeComponent();
            Width *= Config.Configuration.Properties.scale;
            Height *= Config.Configuration.Properties.scale;
            txtBox.Text = GetText(exception);
            if (!ableToSave)
            {
                saveNpcButton.Visibility = Visibility.Collapsed;
            }
            if (!ableToHandle)
            {
                handleExceptionButton.Visibility = Visibility.Collapsed;
            }
        }

        public bool Handle { get; set; } = false;
        public string GetText(Exception exception)
        {
            var cfg = Config.Configuration.Properties;
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendLine($"NPC Maker {MainWindow.Version} Crashed!");
            }
            catch { sb.AppendLine($"NPC Maker (Unknown) Crashed!"); }
            sb.AppendLine($"Time: {DateTime.Now}");
            sb.AppendLine($"Settings:");
            sb.AppendLine();
            sb.AppendLine($"Experimental Features: {(cfg.experimentalFeatures ? "Enabled" : "Disabled")}");
            sb.AppendLine($"Language: {cfg.language.Name}");
            sb.AppendLine($"Log Level: {cfg.LogLevel.ToString()}");
            sb.AppendLine("User Colors:");
            foreach (string s in cfg.userColors ?? new string[] { "No user colors" })
            {
                sb.AppendLine(s);
            }
            sb.AppendLine();
            sb.AppendLine("Recent Files:");
            foreach (string s in cfg.recent ?? new string[] { "No recent files" })
            {
                sb.AppendLine(s);
            }
            sb.AppendLine();
            sb.AppendLine($"Autosave Option: {cfg.autosaveOption}");
            sb.AppendLine($"GUID Generation: {(cfg.generateGuids ? "Enabled" : "Disabled")}");
            sb.AppendLine($"Scale: {cfg.scale}");
            sb.AppendLine($"Theme: {cfg.currentTheme.Name}");
            sb.AppendLine($"Discord Rich Presence: {(cfg.enableDiscord ? "Detailed" : "Private")}");
            sb.AppendLine();
            sb.AppendLine($"Application log:");
            sb.AppendLine();
            foreach (string s in Logger.lines)
            {
                sb.AppendLine(s);
            }
            sb.AppendLine();
            sb.AppendLine($"Exception message: {exception.Message}");
            sb.AppendLine($"Stack trace: {exception.StackTrace}");
            sb.AppendLine("End of crash report!");
            return sb.ToString();
        }

        private void SaveNPC_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Save();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtBox.Text);
        }

        private void HandleException_Click(object sender, RoutedEventArgs e)
        {
            Handle = true;
            Close();
        }
    }
}
