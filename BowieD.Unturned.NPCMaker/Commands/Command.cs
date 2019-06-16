using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        public static Command GetCommand<T>() where T : Command
        {
            return Commands.SingleOrDefault(d => d is T);
        }
    }
}
