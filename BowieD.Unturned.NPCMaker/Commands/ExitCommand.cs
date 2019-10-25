namespace BowieD.Unturned.NPCMaker.Commands
{
    public class ExitCommand : Command
    {
        public override string Name => "exit";
        public override string Help => "Basically the 'Exit' button";

        public override string Syntax => "";

        public override void Execute(string[] args)
        {
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                MainWindow.PerformExit();
            });
        }
    }
}
