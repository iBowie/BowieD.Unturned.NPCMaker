using BowieD.Unturned.NPCMaker.Commands;
using BowieD.Unturned.NPCMaker.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public static bool IsOpened = false;
        public LogWindow()
        {
            InitializeComponent();
#if DEBUG
            Title += " [DEBUG]";
#endif
            executionBox.PreviewKeyDown += ExecutionBox_PreviewKeyDown;
        }
        public new void Show()
        {
            base.Show();
            IsOpened = true;
        }
        protected override void OnClosed(EventArgs e)
        {
            IsOpened = false;
            base.OnClosed(e);
        }

        private void ExecutionBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter || executionBox.Text.Length < 1)
                return;

            string[] command = executionBox.Text.Split(' ');
            executionBox.Text = "";
            var executionCommand = Command.Commands.SingleOrDefault(d => d.Name.ToLower() == command[0].ToLower());
            if (executionCommand == null)
            {
                Logger.Log($"Command {command[0]} not found", Log_Level.Debug);
            }
            else
            {
                var matches = Regex.Matches(string.Join(" ", command.Skip(1)), "[\\\"](.+?)[\\\"]|([^ ]+)", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
                var filtered = (from Match d in matches select d.Value.Trim('"')).ToArray();
                executionCommand.Execute(filtered);
                Logger.Log($"User executed a command: {executionCommand.Name}");
            }
        }
    }
}
