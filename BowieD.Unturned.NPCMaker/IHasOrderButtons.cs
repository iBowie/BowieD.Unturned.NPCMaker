using System.Windows;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker
{
    public interface IHasOrderButtons : IOrderElement
    {
        UIElement UpButton { get; }
        UIElement DownButton { get; }
        Transform Transform { get; }
    }
}
