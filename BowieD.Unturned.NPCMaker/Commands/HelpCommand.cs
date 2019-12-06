namespace BowieD.Unturned.NPCMaker.Commands
{
    public class HelpCommand : Command
    {
        public override string Name => "help";
        public override string Help => "Show list of commands with help";
        public override string Syntax => "[command]";
        public override void Execute(string[] args)
        {
            Command cmd;
            if (args.Length == 0 || (cmd = Command.GetCommand(args[0])) == null)
            {
                foreach (var c in Command.Commands)
                {
                    App.Logger.Log($"[HelpCommand] - {c.Name} {c.Syntax} - {c.Help}");
                }
            }
            else
            {
                App.Logger.Log($"[HelpCommand] - {cmd.Name} {cmd.Syntax} - {cmd.Help}");
            }
        }
    }
}
