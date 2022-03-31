using BowieD.Unturned.NPCMaker.Forms;
using System.Windows;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.Common
{
    internal static class KeyBindHelper
    {
        public static void BindFindReplace(this UIElement element, FindReplace.FindReplaceFormat? format = null)
        {
            RoutedCommand routedCommand = new RoutedCommand();

            routedCommand.InputGestures.Add(new KeyGesture(Key.F, ModifierKeys.Control));
            routedCommand.InputGestures.Add(new KeyGesture(Key.H, ModifierKeys.Control));

            element.CommandBindings.Add(new CommandBinding(routedCommand, new ExecutedRoutedEventHandler((sender, e) =>
            {
                FindReplaceDialog frd = new FindReplaceDialog(format);

                frd.ShowDialog();
            })));
        }
    }
}
