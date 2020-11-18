using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC.Currency
{
    public sealed class CurrencyAsset : IHasUniqueGUID
    {
        public CurrencyAsset()
        {
            GUID = Guid.NewGuid().ToString("N");
            ValueFormat = "{0:N0} ???";
            Entries = new List<CurrencyEntry>();
        }

        [XmlAttribute("guid")]
        public string GUID { get; set; }
        [XmlAttribute("valueFormat")]
        public string ValueFormat { get; set; }
        public List<CurrencyEntry> Entries { get; set; }
    }
}
