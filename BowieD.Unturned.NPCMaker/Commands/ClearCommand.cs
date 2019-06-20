namespace BowieD.Unturned.NPCMaker.Commands
{
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
}
