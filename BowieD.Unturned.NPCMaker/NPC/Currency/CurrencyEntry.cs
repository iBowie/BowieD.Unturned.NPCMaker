using BowieD.Unturned.NPCMaker.Common;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC.Currency
{
    [Serializable]
    public sealed class CurrencyEntry : IAXData
    {
        public CurrencyEntry()
        {
            IsVisibleInVendorMenu = true;
        }

        [XmlAttribute("guid")]
        public string ItemGUID { get; set; }
        [XmlAttribute("value")]
        public uint Value { get; set; }
        [XmlAttribute("isVisibleInVendorMenu")]
        public bool IsVisibleInVendorMenu { get; set; }

        public void Load(XmlNode node, int version)
        {
            ItemGUID = node.Attributes["guid"].ToText();
            Value = node.Attributes["value"].ToUInt32();
            if (version < 16)
                IsVisibleInVendorMenu = true;
            else
                IsVisibleInVendorMenu = node.Attributes["isVisibleInVendorMenu"].ToBoolean();
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateAttributeC("guid", node).WriteString(ItemGUID);
            document.CreateAttributeC("value", node).WriteUInt32(Value);
            document.CreateAttributeC("isVisibleInVendorMenu", node).WriteBoolean(IsVisibleInVendorMenu);
        }
    }
}
