using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.Currencies
{
    public class NE_4003 : CurrencyMistake
    {
        public NE_4003() : base()
        {
            MistakeName = "NE_4003";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_4003(string guid) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_4003_Desc", guid);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.Currency.CurrencyAsset _cur in MainWindow.CurrentProject.data.currencies)
            {
                if (_cur.Entries.All(ce => ce.Value > 1))
                {
                    yield return new NE_4003(_cur.GUID);
                }
            }
        }
    }
}
