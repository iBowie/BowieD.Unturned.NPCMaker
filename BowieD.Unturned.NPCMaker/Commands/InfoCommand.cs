using BowieD.Unturned.NPCMaker.Common.Utility;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public sealed class InfoCommand : Command
    {
        public override string Name => "info";
        public override string Help => "Display debug information";
        public override string Syntax => "";
        public override void Execute(string[] args)
        {
            string text = DebugUtility.GetDebugInformation();
            App.Logger.Log(text);
        }
    }
}
