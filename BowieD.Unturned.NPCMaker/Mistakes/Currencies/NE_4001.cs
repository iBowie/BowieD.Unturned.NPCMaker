using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Currencies
{
    public class NE_4001 : CurrencyMistake
    {
        public NE_4001() : base()
        {
            MistakeName = "NE_4001";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_4001(string guid) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_4001_Desc", guid);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.Currency.CurrencyAsset _cur in MainWindow.CurrentProject.data.currencies)
            {
                if (!Guid.TryParse(_cur.GUID, out _))
                {
                    yield return new NE_4001(_cur.GUID);
                }
            }
        }
    }
}
