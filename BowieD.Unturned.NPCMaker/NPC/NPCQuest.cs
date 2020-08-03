using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCQuest : IHasUIText, INotifyPropertyChanged
    {
        public NPCQuest()
        {
            conditions = new List<Condition>();
            rewards = new List<Reward>();
            Title = "";
            description = "";
            guid = Guid.NewGuid().ToString("N");
            Comment = "";
        }

        [XmlAttribute]
        public string guid;
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
        public List<Condition> conditions;
        public List<Reward> rewards;
        private string _title;
        [XmlElement("title")]
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
        public string description;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public string UIText => $"[{ID}] {Title}";
    }
}