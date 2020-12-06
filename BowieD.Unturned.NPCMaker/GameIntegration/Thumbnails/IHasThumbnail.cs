using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails
{
    public interface IHasThumbnail
    {
        ImageSource Thumbnail { get; }
    }
}
