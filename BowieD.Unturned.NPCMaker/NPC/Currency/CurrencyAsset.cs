using BowieD.Unturned.NPCMaker.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC.Currency
{
    [Serializable]
    public sealed class CurrencyAsset : IHasUniqueGUID, IHasUIText, INotifyPropertyChanged, IAXData
    {
        public CurrencyAsset()
        {
            GUID = Guid.NewGuid().ToString("N");
            ValueFormat = "{0:N0} ???";
            Entries = new List<CurrencyEntry>();
        }

        private string _guid;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [XmlAttribute("guid")]
        public string GUID
        {
            get => _guid;
            set
            {
                _guid = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GUID)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UIText)));
            }
        }
        [XmlAttribute("valueFormat")]
        public string ValueFormat { get; set; }
        public List<CurrencyEntry> Entries { get; set; }

        public string UIText => GUID;

        public void Load(XmlNode node, int version)
        {
            GUID = node.Attributes["guid"].ToText();
            ValueFormat = node.Attributes["valueFormat"].ToText();
            Entries = node["Entries"].ParseAXDataCollection<CurrencyEntry>(version).ToList();
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateAttributeC("guid", node).WriteString(GUID);
            document.CreateAttributeC("valueFormat", node).WriteString(ValueFormat);
            document.CreateNodeC("Entries", node).WriteAXDataCollection(document, "CurrencyEntry", Entries);
        }
    }
}
