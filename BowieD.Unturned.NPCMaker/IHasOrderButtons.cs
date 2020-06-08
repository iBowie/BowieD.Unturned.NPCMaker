using System.Windows;

namespace BowieD.Unturned.NPCMaker
{
    public interface IHasOrderButtons
    {
        UIElement UpButton { get; }
        UIElement DownButton { get; }
    }
}
