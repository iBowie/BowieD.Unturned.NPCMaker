using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [XmlRoot("NPCMakerSave")]
    public class NPCSave
    {
        public NPCSave()
        {
            guid = new Guid().ToString("N");
            characters = new List<NPCCharacter>();
            dialogues = new List<NPCDialogue>();
            vendors = new List<NPCVendor>();
            quests = new List<NPCQuest>();
            objects = new List<NPCObject>();
        }

        [XmlAttribute]
        public string guid;
        public List<NPCCharacter> characters;
        public List<NPCDialogue> dialogues;
        public List<NPCVendor> vendors;
        public List<NPCQuest> quests;
        public List<NPCObject> objects;
    }
}
