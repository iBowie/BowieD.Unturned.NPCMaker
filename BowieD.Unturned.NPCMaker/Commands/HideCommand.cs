using BowieD.Unturned.NPCMaker.Logging;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public sealed class HideCommand : Command
    {
        public override string Name => "hide";
        public override string Help => "Hides console window";
        public override string Syntax => "";
        public override void Execute(string[] args)
        {
            ConsoleLogger.HideConsoleWindow();
        }
    }
}
