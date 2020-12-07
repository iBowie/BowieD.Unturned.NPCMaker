using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails
{
    public interface IHasTextToolTip
    {
        IEnumerable<string> GetToolTipLines();
    }
}
