using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Currencies
{
    public class NE_4004 : CurrencyMistake
    {
        public NE_4004() : base()
        {
            MistakeName = "NE_4004";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_4004(string guid) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_4004_Desc", guid);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.Currency.CurrencyAsset _cur in MainWindow.CurrentProject.data.currencies)
            {
                if (_cur.Entries.Count == 0)
                    yield return new NE_4004(_cur.GUID);
            }
        }
    }
}
