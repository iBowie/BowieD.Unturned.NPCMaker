using BowieD.NPCMaker.Data;
using BowieD.NPCMaker.NPC;
using System.Collections.Generic;

namespace BowieD.NPCMaker.Storage
{
    public class Project : JsonData<Project>
    {
        public Project()
        {
            characters = new List<Character>();
            dialogues = new List<Dialogue>();
            vendors = new List<Vendor>();
            quests = new List<Quest>();
        }
        public string name;
        public List<Character> characters;
        public List<Dialogue> dialogues;
        public List<Vendor> vendors;
        public List<Quest> quests;
    }
}
