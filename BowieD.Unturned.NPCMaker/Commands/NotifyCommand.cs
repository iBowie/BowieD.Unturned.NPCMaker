#if DEBUG
namespace BowieD.Unturned.NPCMaker.Commands
{
    public class NotifyCommand : Command
    {
        public override string Name => "notify";
        public override string Syntax => "<text>";
        public override string Help => "Sends a notification to main window.";
        public override void Execute(string[] args)
        {
            MainWindow.NotificationManager.Notify(string.Join(" ", args));
        }
    }
}
#endif