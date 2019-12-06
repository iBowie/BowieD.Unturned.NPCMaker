using BowieD.Unturned.NPCMaker.Coloring;
using BowieD.Unturned.NPCMaker.Logging;
using System.Linq;

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
                            MainWindow.Instance.MainWindowViewModel.CharacterTabViewModel.SaveColor(args[1]);
                            MainWindow.Instance.MainWindowViewModel.CharacterTabViewModel.UpdateColorPicker();
                            App.Logger.Log($"[ColorCommand] - Color {args[1]} saved.");
                            break;
                        case "remove" when args.Length > 1 && Color.IsHEX(args[1]):
                            MainWindow.Instance.MainWindowViewModel.CharacterTabViewModel.UserColors.data = MainWindow.Instance.MainWindowViewModel.CharacterTabViewModel.UserColors.data.Where(d => d != args[1]).ToArray();
                            App.Logger.Log($"[ColorCommand] - Color {args[1]} removed.");
                            break;
                        case "list":
                            App.Logger.Log($"[ColorCommand] - Saved Colors: {string.Join(", ", MainWindow.Instance.MainWindowViewModel.CharacterTabViewModel.UserColors.data)}");
                            break;
                        default:
                            App.Logger.Log($"[ColorCommand] - Use: color {Syntax}");
                            break;
                    }
                });
            }
            else
            {
                App.Logger.Log($"[ColorCommand] - Use: color {Syntax}");
            }
        }
    }
}
