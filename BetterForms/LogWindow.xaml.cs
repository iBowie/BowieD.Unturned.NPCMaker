using BowieD.Unturned.NPCMaker.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.BetterForms
{
    /// <summary>
    /// Логика взаимодействия для LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
            executionBox.PreviewKeyDown += ExecutionBox_PreviewKeyDown;
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
            }
        }

        public abstract class Command
        {
            public abstract string Name { get; }
            public abstract void Execute(string[] args);

            public static HashSet<Command> Commands
            {
                get
                {
                    if (_commands == null)
                    {
                        _commands = new HashSet<Command>();
                        foreach (var k in Assembly.GetExecutingAssembly().GetTypes())
                        {
                            if (k != null && k.IsSubclassOf(typeof(Command)))
                            {
                                var commandInstance = (Command)Activator.CreateInstance(k);
                                if (commandInstance != null)
                                {
                                    _commands.Add(commandInstance);
                                }
                            }
                        }
                    }
                    return _commands;
                }
            }
            private static HashSet<Command> _commands;
        }

        public class HelpCommand : Command
        {
            public override string Name => "help";

            public override void Execute(string[] args)
            {
                Logger.Log("Nobody would help you.");
            }
        }
    }
}
