using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [XmlRoot("NPCMakerSave")]
    public class NPCSave
    {
        public NPCSave()
        {
            guid = Guid.NewGuid().ToString("N");
            characters = new List<NPCCharacter>();
            dialogues = new List<NPCDialogue>();
            vendors = new List<NPCVendor>();
            quests = new List<NPCQuest>();
            //objects = new List<NPCObject>();
        }

        [XmlAttribute]
        public string guid;
        public List<NPCCharacter> characters;
        public List<NPCDialogue> dialogues;
        public List<NPCVendor> vendors;
        public List<NPCQuest> quests;
        //public List<NPCObject> objects;

        public static explicit operator NPCSave(NPCSaveOld old)
        {
            return new NPCSave
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
    }
}
