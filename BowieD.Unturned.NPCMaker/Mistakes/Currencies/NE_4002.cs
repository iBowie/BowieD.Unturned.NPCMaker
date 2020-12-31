using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Currencies
{
    public class NE_4002 : CurrencyMistake
    {
        public NE_4002() : base()
        {
            MistakeName = "NE_4002";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_4002(string guid, string itemGuid) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_4002_Desc", guid, itemGuid);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.Currency.CurrencyAsset _cur in MainWindow.CurrentProject.data.currencies)
            {
                foreach (var ce in _cur.Entries)
                {
                    if (ce.Value == 0)
                        yield return new NE_4002(_cur.GUID, ce.ItemGUID);
                }
            }
        }
    }
}
