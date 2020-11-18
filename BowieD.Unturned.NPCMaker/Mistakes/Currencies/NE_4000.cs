using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Currencies
{
    public class NE_4000 : CurrencyMistake
    {
        public NE_4000() : base()
        {
            MistakeName = "NE_4000";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_4000(string guid) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_4000_Desc", guid);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.Currency.CurrencyAsset _cur in MainWindow.CurrentProject.data.currencies)
            {
                if (_cur.GUID.Length != 32)
                {
                    yield return new NE_4000(_cur.GUID);
                }
            }
        }
    }
}
