using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [XmlRoot("NPCProject")]
    public class NPCProject
    {
        public NPCProject()
        {
            guid = Guid.NewGuid().ToString("N");
            SAVEDATA_VERSION = 1;
            characters = new List<NPCCharacter>();
            dialogues = new List<NPCDialogue>();
            vendors = new List<NPCVendor>();
            quests = new List<NPCQuest>();
            //objects = new List<NPCObject>();
        }

        [XmlAttribute]
        public string guid;
        public int SAVEDATA_VERSION;
        public List<NPCCharacter> characters;
        public List<NPCDialogue> dialogues;
        public List<NPCVendor> vendors;
        public List<NPCQuest> quests;
        //public List<NPCObject> objects;

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

        #pragma warning disable CS0618
        public static explicit operator NPCProject(NPCSaveOld old)
        {
            return new NPCProject
            {
                dialogues = old.dialogues,
                vendors = old.vendors,
                quests = old.quests,
                characters = new List<NPCCharacter>()
                {
                    new NPCCharacter()
                    {
                        id = old.id,
                        startDialogueId = old.startDialogueId,
                        clothing = old.clothing,
                        christmasClothing = old.christmasClothing,
                        halloweenClothing = old.halloweenClothing,
                        beard = old.beard,
                        displayName = old.displayName,
                        editorName = old.editorName,
                        equipped = old.equipped,
                        equipPrimary = old.equipPrimary,
                        equipSecondary = old.equipSecondary,
                        equipTertiary = old.equipTertiary,
                        face = old.face,
                        guid = old.guid,
                        hairColor = old.hairColor,
                        haircut = old.haircut,
                        leftHanded = old.leftHanded,
                        pose = old.pose,
                        skinColor = old.skinColor,
                        visibilityConditions = old.visibilityConditions
                    }
                }/*,*/
                //objects = new List<NPCObject>()
            };
        }
        #pragma warning restore CS0618

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
        public static NPCProject LoadOld(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (XmlReader reader = XmlReader.Create(fs))
            {
                var res = (NPCProject)(new XmlSerializer(typeof(NPCSaveOld)).Deserialize(reader) as NPCSaveOld);
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
        public static bool CanLoadOld(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    var res = new XmlSerializer(typeof(NPCSaveOld)).CanDeserialize(reader);
                    return res;
                }
            }
            catch (Exception) { return false; }
        }
    }
}
