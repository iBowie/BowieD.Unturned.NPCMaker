using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class Simulation : INotifyPropertyChanged, IAXData
    {
        public Simulation()
        {
            Name = string.Empty;
            Time = 0;
            Skillset = ESkillset.Farm;
            Experience = 0;
            Reputation = 0;
            Holiday = ENPCHoliday.None;

            Health = 100;
            Food = 100;
            Water = 100;
            Virus = 100;

            Flags = new Dictionary<ushort, short>(ushort.MaxValue);
            Quests = new HashSet<ushort>();
            Currencies = new Dictionary<string, uint>();
            Items = new List<Item>();
        }

        private int _time;

        public string Name { get; set; }
        public int Time
        {
            get => _time;
            set
            {
                _time = value;

                OnPropertyChanged(nameof(Time));
                OnPropertyChanged(nameof(DisplayTime));
            }
        }
        public TimeSpan DisplayTime
        {
            get => TimeSpan.FromSeconds(Time);
            set => Time = (int)value.TotalSeconds;
        }
        public ESkillset Skillset { get; set; }
        public uint Experience { get; set; }
        public int Reputation { get; set; }
        public ENPCHoliday Holiday { get; set; }

        public byte Health { get; set; }
        public byte Food { get; set; }
        public byte Water { get; set; }
        public byte Virus { get; set; }

        public Dictionary<ushort, short> Flags { get; }
        public HashSet<ushort> Quests { get; }
        public Dictionary<string, uint> Currencies { get; }
        public List<Item> Items { get; }

        public class Item
        {
            public ushort ID { get; set; }
            public byte Quality { get; set; }
            public byte Amount { get; set; }

            public string DisplayName
            {
                get
                {
                    if (GameAssetManager.TryGetAsset<GameItemAsset>(ID, out var asset))
                    {
                        return asset.name;
                    }
                    else
                    {
                        return ID.ToString();
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Quest_Status GetQuestStatus(ushort id)
        {
            if (Quests.Contains(id))
            {
                NPCQuest questAsset = MainWindow.CurrentProject.data.quests.Single(d => d.ID == id);
                if (questAsset.conditions.All(d => d.Check(this)))
                {
                    return Quest_Status.Ready;
                }

                return Quest_Status.Active;
            }
            if (Flags.ContainsKey(id))
            {
                return Quest_Status.Completed;
            }

            return Quest_Status.None;
        }

        public void Load(XmlNode node, int version)
        {
            Name = node["Name"].ToText();
            Time = node["Time"].ToInt32();
            Skillset = node["Skillset"].ToEnum<ESkillset>();
            Experience = node["Experience"].ToUInt32();
            Reputation = node["Reputation"].ToInt32();
            Holiday = node["Holiday"].ToEnum<ENPCHoliday>();
            Health = node["Health"].ToByte(100);
            Food = node["Food"].ToByte(100);
            Water = node["Water"].ToByte(100);
            Virus = node["Virus"].ToByte(100);

            var flagsNode = node["Flags"];

            Flags.Clear();

            foreach (XmlNode cNode in flagsNode.ChildNodes)
            {
                var id = cNode.Attributes["id"].ToUInt16();
                var val = cNode.Attributes["value"].ToInt16();

                Flags[id] = val;
            }

            var questsNode = node["Quests"];

            Quests.Clear();

            foreach (XmlNode cNode in questsNode.ChildNodes)
            {
                Quests.Add(cNode.ToUInt16());
            }

            var currenciesNode = node["Currencies"];

            Currencies.Clear();

            foreach (XmlNode cNode in currenciesNode.ChildNodes)
            {
                var guid = cNode.Attributes["guid"].ToText();
                var amnt = cNode.Attributes["amount"].ToUInt32();

                Currencies[guid] = amnt;
            }

            var itemsNode = node["Items"];

            Items.Clear();

            foreach (XmlNode cNode in itemsNode.ChildNodes)
            {
                var id = cNode.Attributes["id"].ToUInt16();
                var q = cNode.Attributes["quality"].ToByte();
                var amnt = cNode.Attributes["amount"].ToByte();

                Items.Add(new Item()
                {
                    ID = id,
                    Amount = amnt,
                    Quality = q
                });
            }
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("Name", node).WriteString(Name);
            document.CreateNodeC("Time", node).WriteInt32(Time);
            document.CreateNodeC("Skillset", node).WriteEnum(Skillset);
            document.CreateNodeC("Experience", node).WriteUInt32(Experience);
            document.CreateNodeC("Reputation", node).WriteInt32(Reputation);
            document.CreateNodeC("Holiday", node).WriteEnum(Holiday);
            document.CreateNodeC("Health", node).WriteByte(Health);
            document.CreateNodeC("Food", node).WriteByte(Food);
            document.CreateNodeC("Water", node).WriteByte(Water);
            document.CreateNodeC("Virus", node).WriteByte(Virus);

            var flagsNode = document.CreateNodeC("Flags", node);

            foreach (var flag in Flags)
            {
                var flagNode = document.CreateNodeC("Flag", flagsNode);

                document.CreateAttributeC("id", flagNode).WriteUInt16(flag.Key);
                document.CreateAttributeC("value", flagNode).WriteInt16(flag.Value);
            }

            var questsNode = document.CreateNodeC("Quests", node);

            foreach (var quest in Quests)
            {
                document.CreateNodeC("Quest", questsNode).WriteUInt16(quest);
            }

            var currNode = document.CreateNodeC("Currencies", node);

            foreach (var currency in Currencies)
            {
                var c = document.CreateNodeC("Currency", currNode);

                document.CreateAttributeC("guid", c).WriteString(currency.Key);
                document.CreateAttributeC("amount", c).WriteUInt32(currency.Value);
            }

            var itemsNode = document.CreateNodeC("Items", node);

            foreach (var item in Items)
            {
                var c = document.CreateNodeC("Item", itemsNode);

                document.CreateAttributeC("id", c).WriteUInt16(item.ID);
                document.CreateAttributeC("quality", c).WriteByte(item.Quality);
                document.CreateAttributeC("amount", c).WriteByte(item.Amount);
            }
        }
    }
}
