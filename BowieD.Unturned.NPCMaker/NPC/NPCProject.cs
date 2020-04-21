using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCProject
    {
        public const int CURRENT_SAVEDATA_VERSION = 3;

        public NPCProject()
        {
            guid = Guid.NewGuid().ToString("N");
            SAVEDATA_VERSION = CURRENT_SAVEDATA_VERSION;
            characters = new List<NPCCharacter>();
            dialogues = new List<NPCDialogue>();
            vendors = new List<NPCVendor>();
            quests = new List<NPCQuest>();
            lastCharacter = -1;
            lastDialogue = -1;
            lastVendor = -1;
            lastQuest = -1;
        }

        [XmlAttribute]
        public string guid;
        public int SAVEDATA_VERSION;
        public List<NPCCharacter> characters;
        public List<NPCDialogue> dialogues;
        public List<NPCVendor> vendors;
        public List<NPCQuest> quests;
        public int 
            lastCharacter = -1,
            lastDialogue = -1,
            lastVendor = -1,
            lastQuest = -1;
    }
}
