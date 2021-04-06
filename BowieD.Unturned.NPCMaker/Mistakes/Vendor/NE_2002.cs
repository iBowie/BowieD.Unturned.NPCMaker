using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Vendor
{
    public class NE_2002 : VendorMistake
    {
        public NE_2002()
        {
            MistakeName = "NE_2002";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_2002(string title, ushort id, ushort min, ushort max) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_2002_Desc", title, id, min, max);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            var rangeMin = MainWindow.CurrentProject.data.settings.idRangeMin;
            var rangeMax = MainWindow.CurrentProject.data.settings.idRangeMax;

            foreach (NPC.NPCVendor _vendor in MainWindow.CurrentProject.data.vendors)
            {
                if (_vendor.ID < rangeMin || _vendor.ID > rangeMax)
                    yield return new NE_2002(_vendor.Title, _vendor.ID, rangeMin, rangeMax);
            }
        }
    }
}
