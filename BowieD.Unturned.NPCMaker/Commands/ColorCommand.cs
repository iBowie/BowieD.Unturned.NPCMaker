using BowieD.Unturned.NPCMaker.Coloring;
using BowieD.Unturned.NPCMaker.Editors;
using BowieD.Unturned.NPCMaker.Logging;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public sealed class ColorCommand : Command
    {
        public override string Name => "color";
        public override string Help => "Interact with saved colors";
        public override string Syntax => "<add/remove/list> [color]";
        public override void Execute(string[] args)
        {
            if (args.Length > 0)
            {
                MainWindow.Instance.Dispatcher.Invoke(() =>
                {
                    switch (args[0])
                    {
                        case "add" when args.Length > 1 && Color.IsHEX(args[1]):
                            (MainWindow.CharacterEditor as CharacterEditor).SaveColor(args[1]);
                            (MainWindow.CharacterEditor as CharacterEditor).UpdateColorPickerFromBuffer();
                            break;
                        case "remove" when args.Length > 1 && Color.IsHEX(args[1]):
                            (MainWindow.CharacterEditor as CharacterEditor).RemoveColor(args[1]);
                            break;
                        case "list":
                            App.Logger.LogInfo($"Saved Colors: {string.Join(", ", (MainWindow.CharacterEditor as CharacterEditor).UserColors.data)}");
                            break;
                        default:
                            App.Logger.LogInfo($"Use: color {Syntax}");
                            break;
                    }
                });
            }
        }
    }
}
