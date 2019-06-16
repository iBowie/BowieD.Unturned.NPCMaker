namespace BowieD.Unturned.NPCMaker.Commands
{
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
}
