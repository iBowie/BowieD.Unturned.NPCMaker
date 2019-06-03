using BowieD.Unturned.NPCMaker.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
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
        public static bool IsOpened = false;
        public LogWindow()
        {
            InitializeComponent();
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

        public abstract class Command
        {
            public abstract string Name { get; }
            public abstract string Help { get; }
            public abstract string Syntax { get; }
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
            public static Command GetCommand(string name)
            {
                return Commands.SingleOrDefault(d => d.Name.ToLower() == name.ToLower());
            }
        }

        public class HelpCommand : Command
        {
            public override string Name => "help";
            public override string Help => "Show list of commands with help";
            public override string Syntax => "[command]";

#if DEBUG
            public override void Execute(string[] args)
            {
                Command cmd = null;
                if (args.Length == 0 || (cmd = Command.GetCommand(args[0])) == null)
                {
                    foreach (var c in Command.Commands)
                    {
                        Logger.Log($"{c.Name} {c.Syntax} - {c.Help}");
                    }
                }
                else
                {
                    Logger.Log($"{cmd.Name} {cmd.Syntax} - {cmd.Help}");
                }
            }
#endif
#if !DEBUG
            public override void Execute(string[] args)
            {
                Logger.Log("Don't beg for help! Be stronger!");
            }
#endif
        }
#if DEBUG
        public class ExitCommand : Command
        {
            public override string Name => "exit";
            public override string Help => "Force application exit";

            public override string Syntax => "";

            public override void Execute(string[] args)
            {
                MainWindow.PerformExit();
            }
        }
        public class SaveCommand : Command
        {
            public override string Name => "save";
            public override string Syntax => "";
            public override string Help => "Emits user press on \"Save\" button";
            public override void Execute(string[] args)
            {
                MainWindow.Save();
            }
        }
        public class NotifyCommand : Command
        {
            public override string Name => "notify";
            public override string Syntax => "<text>";
            public override string Help => "Sends a notification to main window.";
            public override void Execute(string[] args)
            {
                MainWindow.NotificationManager.Notify(string.Join(" ", args));
            }
        }
        public class SwitchCommand : Command
        {
            public override string Name => "switch";
            public override string Syntax => "[tab index]";
            public override string Help => "Switches tab of main window";
            public override void Execute(string[] args)
            {
                if (int.TryParse(args[0], out int tab) && tab >= 0 && tab < MainWindow.Instance.mainTabControl.Items.Count)
                {
                    MainWindow.Instance.mainTabControl.SelectedIndex = tab;
                }
                else
                {
                    Logger.Log($"Index must be a digit and higher than -1");
                }
            }
        }
        public class UpdateCommand : Command
        {
            public override string Name => "update";
            public override string Syntax => "";
            public override string Help => "Forces app to download latest version on the server";
            public override void Execute(string[] args)
            {
                Util.UpdateManager.StartUpdate();
            }
        }
        public class GCCommand : Command
        {
            public override string Name => "gc";
            public override string Help => "Interact with Garbage Collector";
            public override string Syntax => "<run/stop/resume>";
            private GCLatencyMode oldMode = GCLatencyMode.Batch;
            public override void Execute(string[] args)
            {
                if (args.Length > 0)
                {
                    switch (args[0].ToLower())
                    {
                        case "run":
                            GC.Collect();
                            Logger.Log("GC Forced");
                            break;
                        case "stop" when GCSettings.LatencyMode != GCLatencyMode.LowLatency:
                            oldMode = GCSettings.LatencyMode;
                            GCSettings.LatencyMode = GCLatencyMode.LowLatency;
                            Logger.Log("GC Paused");
                            break;
                        case "resume" when GCSettings.LatencyMode == GCLatencyMode.LowLatency:
                            GCSettings.LatencyMode = oldMode;
                            Logger.Log("GC Resumed");
                            break;
                    }
                }
            }
        }
        public class RestoreLogCommand : Command
        {
            public override string Name => "restorelog";
            public override string Help => "Prints entire log in window";
            public override string Syntax => "";
            public override void Execute(string[] args)
            {
                foreach (var k in Logger.lines)
                {
                    MainWindow.LogWindow.logBox.Text += k + Environment.NewLine;
                }
            }
        }
        public class ReflectionCommand : Command
        {
            public override string Name => "reflect_mw";

            public override string Help => "Execute any method without parameters in MainWindow [NOT RECOMMENDED]";

            public override string Syntax => "<method in MainWindow>";

            public override void Execute(string[] args)
            {
                if (args.Length > 0)
                {
                    MethodInfo method = typeof(MainWindow).GetMethod(args[0]);
                    method.Invoke(MainWindow.Instance, null);
                }
            }
        }
        public class ClearCommand : Command
        {
            public override string Name => "clear";
            public override string Help => "Clears log window";
            public override string Syntax => "";
            public override void Execute(string[] args)
            {
                MainWindow.LogWindow.logBox.Clear();
            }
        }
#endif
    }
}
