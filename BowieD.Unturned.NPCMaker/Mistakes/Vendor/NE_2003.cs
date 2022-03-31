using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Vendor
{
    public class NE_2003 : VendorMistake
    {
        public NE_2003()
        {
            MistakeName = "NE_2003";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_2003(string title, ushort id, int vendorItemId, bool isBuy) : this()
        {
            if (isBuy)
                MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_2003_Desc_Buy", title, id, vendorItemId);
            else
                MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_2003_Desc_Sell", title, id, vendorItemId);
        }

        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _vendor in MainWindow.CurrentProject.data.vendors)
            {
                List<NPC.VendorItem> buyItems = _vendor.BuyItems;

                for (int i = 0; i < buyItems.Count; i++)
                {
                    NPC.VendorItem _vendorItem = buyItems[i];

                    if (_vendorItem.id == 0)
                    {
                        yield return new NE_2003(_vendor.Title, _vendor.ID, i + 1, _vendorItem.isBuy);
                    }
                }

                List<NPC.VendorItem> sellItems = _vendor.SellItems;

                for (int i = 0; i < sellItems.Count; i++)
                {
                    NPC.VendorItem _vendorItem = sellItems[i];

                    if (_vendorItem.id == 0)
                    {
                        yield return new NE_2003(_vendor.Title, _vendor.ID, i + 1, _vendorItem.isBuy);
                    }
                }
            }
        }
    }
}
