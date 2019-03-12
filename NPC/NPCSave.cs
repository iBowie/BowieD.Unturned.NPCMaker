using BowieD.Unturned.NPCMaker.Examples;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [XmlInclude(typeof(NPCExample))]
    public class NPCSave
    {
        public NPCSave()
        {
            guid = Guid.NewGuid().ToString("N");
            editorName = "";
            displayName = "";
            id = 0;
            face = 0;
            beard = 0;
            haircut = 0;
            hairColor = new NPCColor(0, 0, 0);
            skinColor = new NPCColor(0, 0, 0);
            hat = 0;
            mask = 0;
            top = 0;
            bottom = 0;
            backpack = 0;
            vest = 0;
            glasses = 0;
            startDialogueId = 0;
            pose = NPC_Pose.Stand;
            leftHanded = false;
            equipPrimary = 0;
            equipSecondary = 0;
            equipTertiary = 0;
            equipped = Equip_Type.None;
            dialogues = new List<NPCDialogue>();
            vendors = new List<NPCVendor>();
            quests = new List<NPCQuest>();
            visibilityConditions = new List<Condition>();
        }
        public string editorName;
        public string displayName;
        public ushort id;
        public byte face;
        public byte beard;
        public byte haircut;
        public NPCColor hairColor;
        public NPCColor skinColor;
        public ushort hat;
        public ushort mask;
        public ushort top;
        public ushort bottom;
        public ushort backpack;
        public ushort vest;
        public ushort glasses;
        public ushort startDialogueId;
        public NPC_Pose pose;
        public bool leftHanded;
        public ushort equipPrimary;
        public ushort equipSecondary;
        public ushort equipTertiary;
        public Equip_Type equipped;

        public List<NPCDialogue> dialogues;
        public List<NPCVendor> vendors;
        public List<NPCQuest> quests;
        public List<Condition> visibilityConditions;

        [XmlAttribute]
        public string guid;
        [XmlAttribute]
        public bool IsReadOnly { get; set; }
    }
}
