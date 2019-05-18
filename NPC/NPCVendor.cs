using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCVendor : IHasDisplayName
    {
        public NPCVendor()
        {
            guid = Guid.NewGuid().ToString("N");
            Comment = "";
            items = new List<VendorItem>();
        }

        [XmlAttribute]
        public string guid;
        [XmlAttribute("comment")]
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
        public byte amount;
        public List<Condition> conditions;
        public string spawnPointID;

        public string DisplayName
        {
            get
            {
                return $"{(type == ItemType.ITEM ? "Item" : "Vehicle")} [{id}] x{amount} ({cost}) {(type == ItemType.VEHICLE ? $"({spawnPointID})" : "")}"; ;
            }
        }
    }
}
