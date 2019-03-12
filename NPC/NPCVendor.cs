using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCVendor
    {
        public NPCVendor()
        {
            guid = Guid.NewGuid().ToString("N");
            comment = "";
            items = new List<VendorItem>();
        }

        [XmlAttribute]
        public string guid;
        [XmlAttribute]
        public string comment;

        public ushort id;
        public string vendorTitle;
        public string vendorDescription;
        public List<VendorItem> items;
        [XmlIgnore]
        public List<VendorItem> BuyItems => (items ?? new List<VendorItem>()).Where(d => d.isBuy).ToList();
        [XmlIgnore]
        public List<VendorItem> SellItems => (items ?? new List<VendorItem>()).Where(d => !d.isBuy).ToList();

        public override string ToString()
        {
            return $"[{id}] {vendorTitle}";
        }
    }

    public class VendorItem
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
        public ushort spawnPointID;

        public override string ToString()
        {
            if (isBuy)
            {
                return ((string)MainWindow.Instance.TryFindResource("vendor_Item_Format_Sell")).Replace("%id%", id.ToString()).Replace("%cost%", cost.ToString()).Replace("%itemType%", (string)MainWindow.Instance.TryFindResource($"vendor_Type_{type.ToString()}"));
            }
            return ((string)MainWindow.Instance.TryFindResource("vendor_Item_Format_Buy")).Replace("%id%", id.ToString()).Replace("%cost%", cost.ToString()).Replace("%itemType%", (string)MainWindow.Instance.TryFindResource($"vendor_Type_{type.ToString()}"));
        }
    }
}
