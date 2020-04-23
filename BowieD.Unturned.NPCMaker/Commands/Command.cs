using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BowieD.Unturned.NPCMaker.Commands
{
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
                    foreach (Type k in Assembly.GetExecutingAssembly().GetTypes())
                    {
                        if (k != null && k.IsSubclassOf(typeof(Command)))
                        {
                            Command commandInstance = (Command)Activator.CreateInstance(k);
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
        public static Command GetCommand<T>() where T : Command
        {
            return Commands.SingleOrDefault(d => d is T);
        }
        public static string Execute(string input)
        {
            string[] command = input.Split(' ');
            Command executionCommand = Command.Commands.SingleOrDefault(d => d.Name.ToLower() == command[0].ToLower());
            if (executionCommand == null)
            {
                return $"Command {command[0]} not found";
            }
            else
            {
                MatchCollection matches = Regex.Matches(string.Join(" ", command.Skip(1)), "[\\\"](.+?)[\\\"]|([^ ]+)", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
                string[] filtered = (from Match d in matches select d.Value.Trim('"')).ToArray();
                executionCommand.Execute(filtered);
                return $"Command {command[0]} executed";
            }
        }
    }
}
