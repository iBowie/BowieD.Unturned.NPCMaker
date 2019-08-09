using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCProject
    {
        public NPCProject()
        {
            guid = Guid.NewGuid().ToString("N");
            SAVEDATA_VERSION = 2;
            characters = new List<NPCCharacter>();
            dialogues = new List<NPCDialogue>();
            vendors = new List<NPCVendor>();
            quests = new List<NPCQuest>();
        }

        [XmlAttribute]
        public string guid;
        public int SAVEDATA_VERSION;
        public List<NPCCharacter> characters;
        public List<NPCDialogue> dialogues;
        public List<NPCVendor> vendors;
        public List<NPCQuest> quests;
    }
}
