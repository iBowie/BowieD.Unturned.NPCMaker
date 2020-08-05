using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCVendor : IHasUIText, INotifyPropertyChanged, IHasUniqueGUID
    {
        public NPCVendor()
        {
            GUID = Guid.NewGuid().ToString("N");
            Comment = "";
            items = new List<VendorItem>();
            currency = "";
        }

        [XmlAttribute("guid")]
        public string GUID { get; set; }
        [XmlAttribute("comment")]
        public string Comment { get; set; }

        private ushort _id;
        [XmlElement("id")]
        public ushort ID
        {
            get => _id;
            set
            {
                _id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UIText)));
            }
        }
        private string _title;
        [XmlElement("vendorTitle")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UIText)));
            }
        }
        public string vendorDescription;
        public List<VendorItem> items;
        public bool disableSorting;
        public string currency;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [XmlIgnore]
        public List<VendorItem> BuyItems => (items ?? new List<VendorItem>()).Where(d => d.isBuy).ToList();
        [XmlIgnore]
        public List<VendorItem> SellItems => (items ?? new List<VendorItem>()).Where(d => !d.isBuy).ToList();

        public string UIText => $"[{ID}] {Title}";
    }
    [Serializable]
    public class VendorItem : IHasUIText
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

        public string UIText
        {
            get
            {
                return $"{(type == ItemType.ITEM ? "Item" : "Vehicle")} [{id}] ({cost}) {(type == ItemType.VEHICLE ? $"({spawnPointID})" : "")}"; ;
            }
        }
    }
}
