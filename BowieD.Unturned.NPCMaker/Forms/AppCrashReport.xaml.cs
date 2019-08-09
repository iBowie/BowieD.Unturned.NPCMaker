using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Logging;
using System;
using System.Text;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для AppCrashReport.xaml
    /// </summary>
    public partial class AppCrashReport : Window
    {
        public AppCrashReport(Exception exception, bool ableToSave = true, bool ableToHandle = true)
        {
            InitializeComponent();
            Width *= AppConfig.Instance.scale;
            Height *= AppConfig.Instance.scale;
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
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendLine($"NPC Maker {MainWindow.Version} Crashed!");
            }
            catch { sb.AppendLine($"NPC Maker (Unknown) Crashed!"); }
            Commands.Command.GetCommand<Commands.InfoCommand>().Execute(null);
            sb.AppendLine();
            sb.AppendLine($"Application log:");
            sb.AppendLine();
            sb.AppendLine(FileLogger.GetContents());
            sb.AppendLine();
            sb.AppendLine($"Exception message: {exception.Message}");
            sb.AppendLine($"Stack trace: {exception.StackTrace}");
            sb.AppendLine("End of crash report!");
            return sb.ToString();
        }

        private void SaveNPC_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CurrentProject.Save();
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
