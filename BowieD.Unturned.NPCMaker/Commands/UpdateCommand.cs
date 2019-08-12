using BowieD.Unturned.NPCMaker.Logging;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public class UpdateCommand : Command
    {
        public override string Name => "update";
        public override string Syntax => "";
        public override string Help => "Forces app to download latest version on the server";
        public override void Execute(string[] args)
        {
            App.Logger.LogWarning("THIS VERSION DOES NOT SUPPORT UPDATES");
        }
    }
}
