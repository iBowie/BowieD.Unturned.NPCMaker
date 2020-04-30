using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class Simulation : INotifyPropertyChanged
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
            Quests = new HashSet<ushort>(ushort.MaxValue);
            Currencies = new Dictionary<string, uint>();
            Items = new List<Item>();
        }

        public string Name { get; set; }
        public int Time { get; set; }
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
                NPCQuest questAsset = MainWindow.CurrentProject.data.quests.Single(d => d.id == id);
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
    }
}
