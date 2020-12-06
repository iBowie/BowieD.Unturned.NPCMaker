using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails
{
    public interface IHasThumbnail
    {
        BitmapImage Thumbnail { get; }
    }
}
