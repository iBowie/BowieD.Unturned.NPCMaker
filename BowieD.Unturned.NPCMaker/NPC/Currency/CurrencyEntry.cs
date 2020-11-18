using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC.Currency
{
    public sealed class CurrencyEntry
    {
        [XmlAttribute("guid")]
        public string ItemGUID { get; set; }
        [XmlAttribute("value")]
        public uint Value { get; set; }
    }
}
