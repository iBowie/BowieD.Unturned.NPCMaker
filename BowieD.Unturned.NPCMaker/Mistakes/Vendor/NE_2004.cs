using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Vendor
{
    public class NE_2004 : VendorMistake
    {
        public NE_2004()
        {
            MistakeName = "NE_2004";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_2004(string title, ushort id, int itemIndex, bool isBuy, NPC.Condition_Type conditionType) : this()
        {
            var localizedTypeName = LocalizationManager.Current.Condition[$"Type_{conditionType}"];

            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_2004_Desc" + (isBuy ? "_Buy" : "_Sell"), itemIndex + 1, title, id, localizedTypeName);
        }

        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var vendor in MainWindow.CurrentProject.data.vendors)
            {
                for (int i = 0; i < vendor.BuyItems.Count; i++)
                {
                    NPC.VendorItem item = vendor.BuyItems[i];

                    foreach (var condition in item.conditions)
                    {
                        if (ConditionChecker.IsAllowed<NPC.VendorItem>(condition.Type))
                            continue;

                        yield return new NE_2004(vendor.Title, vendor.ID, i, true, condition.Type);
                    }
                }

                for (int i = 0; i < vendor.SellItems.Count; i++)
                {
                    NPC.VendorItem item = vendor.SellItems[i];

                    foreach (var condition in item.conditions)
                    {
                        if (ConditionChecker.IsAllowed<NPC.VendorItem>(condition.Type))
                            continue;

                        yield return new NE_2004(vendor.Title, vendor.ID, i, false, condition.Type);
                    }
                }
            }
        }
    }
}
