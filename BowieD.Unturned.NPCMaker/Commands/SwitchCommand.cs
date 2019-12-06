#if DEBUG
using BowieD.Unturned.NPCMaker.Logging;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public class SwitchCommand : Command
    {
        public override string Name => "switch";
        public override string Syntax => "[tab index]";
        public override string Help => "Switches tab of main window";
        public override void Execute(string[] args)
        {
            if (int.TryParse(args[0], out int tab) && tab >= 0 && tab < MainWindow.Instance.mainTabControl.Items.Count)
            {
                MainWindow.Instance.mainTabControl.SelectedIndex = tab;
            }
            else
            {
                App.Logger.Log($"[SwitchCommand] - Index must be a digit and higher than -1");
            }
        }
    }
}
#endif
