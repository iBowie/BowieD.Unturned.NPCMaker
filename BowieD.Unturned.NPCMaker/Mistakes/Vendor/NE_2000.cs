using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Vendor
{
    /// <summary>
    /// Zero id
    /// </summary>
    public class NE_2000 : VendorMistake
    {
        public NE_2000() : base()
        {
            MistakeName = "NE_2000";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_2000(string title, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_2000_Desc", title, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCVendor _vend in MainWindow.CurrentProject.data.vendors)
            {
                if (_vend.id == 0)
                {
                    yield return new NE_2000(_vend.vendorTitle, _vend.id);
                }
            }
        }
    }
}
