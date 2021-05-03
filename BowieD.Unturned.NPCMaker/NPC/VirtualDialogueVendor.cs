using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.GameIntegration;
using System;
using System.ComponentModel;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class VirtualDialogueVendor : IHasUIText, INotifyPropertyChanged, IHasUniqueGUID, IAXData
    {
        private ushort
            _id;
        private LimitedList<VendorItem>
            _items;
        private string
            _comment;
        private LimitedList<string>
            _pages;
        private string
            _goodbyeText,
            _buyingFormat,
            _sellingFormat;

        public event PropertyChangedEventHandler PropertyChanged;

        public VirtualDialogueVendor()
        {
            ID = 0;
            GUID = Guid.NewGuid().ToString("N");
            Comment = string.Empty;
            Items = new LimitedList<VendorItem>(byte.MaxValue - 1);
            Pages = new LimitedList<string>(byte.MaxValue);
            GoodbyeText = "Goodbye";
            BuyingFormatText = "Sell '{0}' for {1}";
            SellingFormatText = "Buy '{0}' for {1}";
            BoughtDialogueID = 0;
            SoldDialogueID = 0;
            GoodbyeDialogueID = 0;
            CurrencyGUID = string.Empty;
        }

        public string GUID { get; set; }
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
        public LimitedList<VendorItem> Items
        {
            get => _items;
            set
            {
                _items = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
            }
        }
        public LimitedList<string> Pages
        {
            get => _pages;
            set
            {
                _pages = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));
            }
        }
        public string GoodbyeText
        {
            get => _goodbyeText;
            set
            {
                _goodbyeText = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GoodbyeText)));
            }
        }
        public string BuyingFormatText
        {
            get => _buyingFormat;
            set
            {
                _buyingFormat = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BuyingFormatText)));
            }
        }
        public string SellingFormatText
        {
            get => _sellingFormat;
            set
            {
                _sellingFormat = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SellingFormatText)));
            }
        }
        public ushort BoughtDialogueID { get; set; }
        public ushort SoldDialogueID { get; set; }
        public ushort GoodbyeDialogueID { get; set; }
        public string CurrencyGUID { get; set; }

        public string UIText
        {
            get
            {
                if (string.IsNullOrEmpty(Comment))
                {
                    return $"[{ID}]";
                }
                else
                {
                    return TextUtil.Shortify($"[{ID}] - {Comment}", 24);
                }
            }
        }

        public NPCDialogue CreateDialogue()
        {
            var dial = new NPCDialogue()
            {
                Comment = Comment,
                GUID = GUID,
                ID = ID
            };

            NPCMessage mainMessage = new NPCMessage()
            {
                pages = Pages
            };

            LimitedList<NPCResponse> responses = new LimitedList<NPCResponse>(byte.MaxValue);

            foreach (var item in Items)
            {
                NPCResponse response = new NPCResponse();
                string formatText;

                if (item.isBuy)
                {
                    response.openDialogueId = BoughtDialogueID;
                    formatText = BuyingFormatText;

                    switch (item.type)
                    {
                        case ItemType.ITEM:
                            response.conditions.Add(new Conditions.ConditionItem()
                            {
                                ID = item.id,
                                Amount = 1,
                                Reset = true
                            });
                            break;
                    }

                    response.conditions.AddRange(item.conditions);

                    if (string.IsNullOrEmpty(CurrencyGUID))
                    {
                        response.rewards.Add(new Rewards.RewardExperience()
                        {
                            Value = item.cost
                        });
                    }
                    else
                    {
                        response.rewards.Add(new Rewards.RewardCurrency()
                        {
                            GUID = CurrencyGUID,
                            Value = item.cost
                        });
                    }
                }
                else
                {
                    response.openDialogueId = SoldDialogueID;
                    formatText = SellingFormatText;

                    if (string.IsNullOrEmpty(CurrencyGUID)) // xp
                    {
                        response.conditions.Add(new Conditions.ConditionExperience()
                        {
                            Logic = Logic_Type.Greater_Than_Or_Equal_To,
                            Value = item.cost,
                            Reset = true
                        });
                    }
                    else // currency
                    {
                        response.conditions.Add(new Conditions.ConditionCurrency()
                        {
                            GUID = CurrencyGUID,
                            Logic = Logic_Type.Greater_Than_Or_Equal_To,
                            Value = item.cost,
                            Reset = true
                        });
                    }

                    response.conditions.AddRange(item.conditions);

                    switch (item.type)
                    {
                        case ItemType.ITEM:
                            response.rewards.Add(new Rewards.RewardItem()
                            {
                                ID = item.id,
                                Amount = 1,
                                Auto_Equip = false
                            });
                            break;
                        case ItemType.VEHICLE:
                            response.rewards.Add(new Rewards.RewardVehicle()
                            {
                                ID = item.id,
                                Spawnpoint = item.spawnPointID
                            });
                            break;
                    }
                }

                if (GameAssetManager.TryGetAsset<GameItemAsset>(item.id, out var asset))
                    response.mainText = string.Format(formatText, asset.Name, item.cost);
                else
                    response.mainText = string.Format(formatText, item.id, item.cost);

                responses.Add(response);
            }

            responses.Add(new NPCResponse()
            {
                openDialogueId = GoodbyeDialogueID,
                mainText = GoodbyeText
            });

            dial.Messages = new LimitedList<NPCMessage>(byte.MaxValue)
            {
                mainMessage
            };
            dial.Responses = responses;

            return dial;
        }

        public void Load(XmlNode node, int version)
        {
            ID = node["id"].ToUInt16();
            Items = new LimitedList<VendorItem>(node["items"].ParseAXDataCollection<VendorItem>(version), byte.MaxValue - 1);
            Pages = new LimitedList<string>(node["pages"].ParseStringCollection(), byte.MaxValue);
            Comment = node["comment"].ToText();
            GoodbyeText = node["goodbyeText"].ToText();
            BuyingFormatText = node["buyingFormat"].ToText();
            SellingFormatText = node["sellingFormat"].ToText();
            BoughtDialogueID = node["boughtDialogueId"].ToUInt16();
            SoldDialogueID = node["soldDialogueId"].ToUInt16();
            GoodbyeDialogueID = node["goodbyeDialogueId"].ToUInt16();
            GUID = node["guid"].ToText(Guid.NewGuid().ToString("N"));
            CurrencyGUID = node["currency"].ToText();
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("id", node).WriteUInt16(ID);
            document.CreateNodeC("items", node).WriteAXDataCollection(document, "VendorItem", Items);
            document.CreateNodeC("pages", node).WriteStringCollection(document, Pages);
            document.CreateNodeC("comment", node).WriteString(Comment);
            document.CreateNodeC("goodbyeText", node).WriteString(GoodbyeText);
            document.CreateNodeC("buyingFormat", node).WriteString(BuyingFormatText);
            document.CreateNodeC("sellingFormat", node).WriteString(SellingFormatText);
            document.CreateNodeC("boughtDialogueId", node).WriteUInt16(BoughtDialogueID);
            document.CreateNodeC("soldDialogueId", node).WriteUInt16(SoldDialogueID);
            document.CreateNodeC("goodbyeDialogueId", node).WriteUInt16(GoodbyeDialogueID);
            document.CreateNodeC("guid", node).WriteString(GUID);
            document.CreateNodeC("currency", node).WriteString(CurrencyGUID);
        }
    }
}
