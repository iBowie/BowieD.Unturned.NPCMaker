using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCQuest : IHasUIText, INotifyPropertyChanged, IHasUniqueGUID, IAXData
    {
        public NPCQuest()
        {
            conditions = new LimitedList<Condition>(byte.MaxValue);
            rewards = new LimitedList<Reward>(byte.MaxValue);
            Title = "";
            description = "";
            GUID = Guid.NewGuid().ToString("N");
            Comment = "";
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
        public LimitedList<Condition> conditions;
        public LimitedList<Reward> rewards;
        private string _title;
        [XmlElement("title")]
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
        public string description;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

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
            GUID = node.Attributes["guid"].Value;
            Comment = node.Attributes["comment"].Value;

            ID = node["id"].ToUInt16();
            Title = node["title"].ToText();
            description = node["description"].ToText();

            conditions = node["conditions"].ParseAXDataCollection<Condition>(version).ToLimitedList(byte.MaxValue);
            rewards = node["rewards"].ParseAXDataCollection<Reward>(version).ToLimitedList(byte.MaxValue);
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateAttributeC("guid", node).WriteString(GUID);
            document.CreateAttributeC("comment", node).WriteString(Comment);

            document.CreateNodeC("id", node).WriteUInt16(ID);
            document.CreateNodeC("title", node).WriteString(Title);
            document.CreateNodeC("description", node).WriteString(description);

            document.CreateNodeC("conditions", node).WriteAXDataCollection(document, "Condition", conditions);
            document.CreateNodeC("rewards", node).WriteAXDataCollection(document, "Reward", rewards);
        }
    }
}