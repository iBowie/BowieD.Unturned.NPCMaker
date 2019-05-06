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

        public IEnumerable<ushort> GetAllIDs()
        {
            foreach (var contentID in GetAllContentIDs())
                yield return contentID;
            foreach (var conditionID in GetAllConditionIDs())
                yield return conditionID;
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
        public IEnumerable<ushort> GetAllConditionIDs()
        {

            foreach (var character in characters)
            {
                foreach (var condition in character.visibilityConditions)
                {
                    if (condition is Condition.IHasConditionID condId)
                    {
                        yield return condId.FlagID;
                    }
                }
            }
            foreach (var dialogue in dialogues)
            {
                foreach (var response in dialogue.responses)
                {
                    foreach (var condition in response.conditions)
                    {
                        if (condition is Condition.IHasConditionID condId)
                        {
                            yield return condId.FlagID;
                        }
                    }
                }
                foreach (var message in dialogue.messages)
                {
                    foreach (var condition in message.conditions)
                    {
                        if (condition is Condition.IHasConditionID condId)
                        {
                            yield return condId.FlagID;
                        }
                    }
                }
            }
            foreach (var vendor in vendors)
            {
                foreach (var item in vendor.items)
                {
                    foreach (var condition in item.conditions)
                    {
                        if (condition is Condition.IHasConditionID condId)
                        {
                            yield return condId.FlagID;
                        }
                    }
                }
            }
            foreach (var quest in quests)
            {
                foreach (var condition in quest.conditions)
                {
                    if (condition is Condition.IHasConditionID condId)
                    {
                        yield return condId.FlagID;
                    }
                }
            }
        }

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
