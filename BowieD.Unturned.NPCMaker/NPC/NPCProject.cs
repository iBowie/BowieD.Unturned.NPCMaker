using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [XmlRoot("NPCProject")]
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

        public IEnumerable<ushort> GetAllIDs()
        {
            foreach (var contentID in GetAllContentIDs())
                yield return contentID;
        }
        public IEnumerable<ushort> GetAllContentIDs()
        {
            foreach (var character in characters)
                yield return character.id;
            foreach (var dialogue in dialogues)
                yield return dialogue.id;
            foreach (var vendor in vendors)
                yield return vendor.id;
            foreach (var quest in quests)
                yield return quest.id;
        }
        public void Save(string path)
        {
            XmlWriterSettings writerSettings;
#if DEBUG
            writerSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t"
            };
#endif
#if !DEBUG
            writerSettings = new XmlWriterSettings();
#endif
            using (FileStream fs = new FileStream(path, FileMode.Create))
            using (XmlWriter writer = XmlWriter.Create(fs, writerSettings))
            {
                XmlSerializer ser = new XmlSerializer(typeof(NPCProject));
                ser.Serialize(writer, this);
            }
        }
        public static NPCProject Load(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (XmlReader reader = XmlReader.Create(fs))
            {
                var res = new XmlSerializer(typeof(NPCProject)).Deserialize(reader) as NPCProject;
                return res;
            }
        }
        public static bool CanLoad(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    var res = new XmlSerializer(typeof(NPCProject)).CanDeserialize(reader);
                    return res;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
