using BowieD.Unturned.NPCMaker.Configuration;
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
        private string _comment;
        [XmlAttribute("comment")]
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Comment)));
                if (AppConfig.Instance.useCommentsInsteadOfData)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UIText)));
            }
        }

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
                if (!AppConfig.Instance.useCommentsInsteadOfData)
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

        public string UIText
        {
            get
            {
                if (AppConfig.Instance.useCommentsInsteadOfData)
                {
                    if (string.IsNullOrEmpty(Comment))
                        return $"[{ID}]";
                    return TextUtil.Shortify($"[{ID}] - {Comment}", 24);
                }
                else
                {
                    return $"[{ID}] {Title}";
                }
            }
        }
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
