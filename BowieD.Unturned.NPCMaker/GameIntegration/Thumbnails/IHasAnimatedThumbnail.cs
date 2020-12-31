using System.Collections.Generic;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails
{
    public interface IHasAnimatedThumbnail
    {
        IEnumerable<ImageSource> Thumbnails { get; }
    }
}
