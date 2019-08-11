namespace BowieD.Unturned.NPCMaker.Commands
{
    public class SaveCommand : Command
    {
        public override string Name => "save";
        public override string Syntax => "";
        public override string Help => "Emits user press on \"Save\" button";
        public override void Execute(string[] args)
        {
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                MainWindow.Proxy.SaveClick(null, null);
            });
        }
    }
}
