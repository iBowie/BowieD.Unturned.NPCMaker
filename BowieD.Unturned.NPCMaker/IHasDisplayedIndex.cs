using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker
{
    public interface IHasDisplayedIndex : IOrderElement
    {
        TextBlock IndexTextBlock { get; }
    }
}
