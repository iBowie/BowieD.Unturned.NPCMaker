using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCVendor : IHasDisplayName, IHasComment
    {
        public NPCVendor()
        {
            guid = Guid.NewGuid().ToString("N");
            Comment = "";
            items = new List<VendorItem>();
        }

        [XmlAttribute]
        public string guid;
        [XmlAttribute]
        [XmlElement("comment")]
        public string Comment { get; set; }

        public ushort id;
        public string vendorTitle;
        public string vendorDescription;
        public List<VendorItem> items;
        [XmlIgnore]
        public List<VendorItem> BuyItems => (items ?? new List<VendorItem>()).Where(d => d.isBuy).ToList();
        [XmlIgnore]
        public List<VendorItem> SellItems => (items ?? new List<VendorItem>()).Where(d => !d.isBuy).ToList();

        public string DisplayName => $"[{id}] {vendorTitle}";
    }

    public class VendorItem : IHasDisplayName
    {
        public VendorItem()
        {
            conditions = new List<Condition>();
        }

        public bool isBuy;
        public ItemType type;
        public uint cost;
        public ushort id;
        public List<Condition> conditions;
        public string spawnPointID;

        public string DisplayName
        {
            get
            {
                if (isBuy)
                {
                    return MainWindow.Localize("vendor_Item_Format_Sell").Replace("%id%", id.ToString()).Replace("%cost%", cost.ToString()).Replace("%itemType%", MainWindow.Localize($"vendor_Type_{type.ToString()}"));
                }
                return MainWindow.Localize("vendor_Item_Format_Buy").Replace("%id%", id.ToString()).Replace("%cost%", cost.ToString()).Replace("%itemType%", MainWindow.Localize($"vendor_Type_{type.ToString()}"));
            }
        }
    }
}
