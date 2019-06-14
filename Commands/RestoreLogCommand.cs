using BowieD.Unturned.NPCMaker.Logging;
using System;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public class RestoreLogCommand : Command
    {
        public override string Name => "restorelog";
        public override string Help => "Prints entire log in window";
        public override string Syntax => "";
        public override void Execute(string[] args)
        {
            foreach (var k in Logger.lines)
            {
                MainWindow.LogWindow.logBox.Text += k + Environment.NewLine;
            }
        }
    }
}
