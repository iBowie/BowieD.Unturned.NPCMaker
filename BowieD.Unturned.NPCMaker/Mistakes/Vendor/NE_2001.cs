using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Vendor
{
    /// <summary>
    /// No items
    /// </summary>
    public class NE_2001 : VendorMistake
    {
        public NE_2001() : base()
        {
            MistakeName = "NE_2001";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_2001(string title, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_2001_Desc", title, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCVendor _vend in MainWindow.CurrentProject.data.vendors)
            {
                if (_vend.items.Count == 0)
                {
                    yield return new NE_2001(_vend.vendorTitle, _vend.id);
                }
            }
        }
    }
}
