using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCVendor : IHasUIText, INotifyPropertyChanged, IHasUniqueGUID, IAXData
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

        public void Load(XmlNode node, int version)
        {
            GUID = node.Attributes["guid"].ToText();
            Comment = node.Attributes["comment"].ToText();

            ID = node["id"].ToUInt16();
            Title = node["vendorTitle"].ToText();
            vendorDescription = node["vendorDescription"].ToText();

            if (version < 8)
            {
                items = node["items"].ParseAXDataCollection<VendorItem>(version).ToList();
            }
            else
            {
                var buyItems = node["buyingItems"].ParseVendorItemsNew(version, true);
                var sellItems = node["sellingItems"].ParseVendorItemsNew(version, false);

                items = buyItems.Concat(sellItems).ToList();
            }

            disableSorting = node["disableSorting"].ToBoolean();
            currency = node["currency"].ToText();
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateAttributeC("guid", node).WriteString(GUID);
            document.CreateAttributeC("comment", node).WriteString(Comment);

            document.CreateNodeC("id", node).WriteUInt16(ID);
            document.CreateNodeC("vendorTitle", node).WriteString(Title);
            document.CreateNodeC("vendorDescription", node).WriteString(vendorDescription);

            document.CreateNodeC("buyingItems", node).WriteVendorItemsNew(document, BuyItems, true);
            document.CreateNodeC("sellingItems", node).WriteVendorItemsNew(document, SellItems, false);

            document.CreateNodeC("disableSorting", node).WriteBoolean(disableSorting);
            document.CreateNodeC("currency", node).WriteString(currency);
        }
    }
    [Serializable]
    public class VendorItem : IHasUIText, IUIL_Icon, IAXData
    {
        public VendorItem()
        {
            conditions = new List<Condition>();
            rewards = new List<Reward>();
        }

        public bool isBuy;
        public ItemType type;
        public uint cost;
        public ushort id;
        public List<Condition> conditions;
        public string spawnPointID;
        public List<Reward> rewards;

        public string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                switch (type)
                {
                    case ItemType.ITEM:
                        {
                            sb.Append("Item");

                            if (GameAssetManager.TryGetAsset<GameItemAsset>(id, out var asset))
                            {
                                sb.Append($" [{asset.name}] ({cost})");
                            }
                            else
                            {
                                sb.Append($" [{id}] ({cost})");
                            }
                        }
                        break;
                    case ItemType.VEHICLE:
                        {
                            sb.Append("Vehicle");

                            if (GameAssetManager.TryGetAsset<GameVehicleAsset>(id, out var asset))
                            {
                                sb.Append($" [{asset.name}] ({cost})");
                            }
                            else
                            {
                                sb.Append($" [{id}] ({cost})");
                            }

                            sb.Append($" ({spawnPointID})");
                        }
                        break;
                }

                return sb.ToString();
            }
        }

        public bool UpdateIcon(out BitmapImage image)
        {
            if (type == ItemType.ITEM)
            {
                if (id > 0 && GameAssetManager.TryGetAsset<GameItemAsset>(id, out var asset))
                {
                    image = ThumbnailManager.CreateThumbnail(asset.ImagePath);
                    return true;
                }
                else
                {
                    image = default;
                    return false;
                }
            }
            else
            {
                image = default;
                return false;
            }
        }

        public void Load(XmlNode node, int version)
        {
            id = node["id"].ToUInt16();
            cost = node["cost"].ToUInt32();
            type = node["type"].ToEnum<ItemType>();
            isBuy = node["isBuy"].ToBoolean();
            conditions = node["conditions"].ParseAXDataCollection<Condition>(version).ToList();
            spawnPointID = node["spawnPointID"].ToText();

            if (version >= 10)
            {
                rewards = node["rewards"].ParseAXDataCollection<Reward>(version).ToList();
            }
            else
            {
                rewards = new List<Reward>();
            }
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("id", node).WriteUInt16(id);
            document.CreateNodeC("cost", node).WriteUInt32(cost);
            document.CreateNodeC("type", node).WriteEnum(type);
            document.CreateNodeC("isBuy", node).WriteBoolean(isBuy);
            document.CreateNodeC("conditions", node).WriteAXDataCollection(document, "Condition", conditions);
            document.CreateNodeC("spawnPointID", node).WriteString(spawnPointID);
            document.CreateNodeC("rewards", node).WriteAXDataCollection(document, "Reward", rewards);
        }
    }
}
