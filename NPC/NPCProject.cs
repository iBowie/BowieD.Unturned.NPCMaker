using BowieD.Unturned.NPCMaker.NPC.Conditions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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
    }
}
