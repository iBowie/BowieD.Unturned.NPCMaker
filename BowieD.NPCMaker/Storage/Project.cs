using BowieD.NPCMaker.NPC;
using System.Collections.Generic;

namespace BowieD.NPCMaker.Storage
{
    public class Project
    {
        public string name;
        public List<Character> characters;
        public List<Dialogue> dialogues;
        public List<Vendor> vendors;
        public List<Quest> quests;
    }
}
