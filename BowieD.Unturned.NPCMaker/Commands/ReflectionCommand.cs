#if DEBUG
using System.Reflection;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public class ReflectionCommand : Command
    {
        public override string Name => "reflect_mw";

        public override string Help => "Execute any method without parameters in MainWindow [NOT RECOMMENDED]";

        public override string Syntax => "<method in MainWindow>";

        public override void Execute(string[] args)
        {
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                if (args.Length > 0)
                {
                    MethodInfo method = typeof(MainWindow).GetMethod(args[0]);
                    method.Invoke(MainWindow.Instance, null);
                }
            });
        }
    }
}
#endif