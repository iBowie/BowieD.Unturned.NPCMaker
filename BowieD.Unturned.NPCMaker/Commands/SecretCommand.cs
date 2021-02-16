namespace BowieD.Unturned.NPCMaker.Commands
{
    public sealed class SecretCommand : Command
    {
        public override string Name => "secret";
        public override string Help => "???";
        public override string Syntax => "???";
        public override void Execute(string[] args)
        {
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                App.Achievements.TryGiveAchievement("console");
            });
        }
    }
}
