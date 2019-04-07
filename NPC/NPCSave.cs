using BowieD.Unturned.NPCMaker.Examples;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            clothing = new NPCClothing();
            halloweenClothing = new NPCClothing();
            christmasClothing = new NPCClothing();
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
        public NPCClothing clothing;
        public NPCClothing christmasClothing;
        public NPCClothing halloweenClothing;
        [Obsolete("Use clothing.hat instead")]
        public ushort hat
        {
            get => 0;
            set
            {
                clothing.hat = value;
            }
        }
        [Obsolete("Use clothing.mask instead")]
        public ushort mask
        {
            get => 0;
            set
            {
                clothing.mask = value;
            }
        }
        [Obsolete("Use clothing.top instead")]
        public ushort top
        {
            get => 0;
            set
            {
                clothing.top = value;
            }
        }
        [Obsolete("Use clothing.bottom instead")]
        public ushort bottom
        {
            get => 0;
            set
            {
                clothing.bottom = value;
            }
        }
        [Obsolete("Use clothing.backpack instead")]
        public ushort backpack
        {
            get => 0;
            set
            {
                clothing.backpack = value;
            }
        }
        [Obsolete("Use clothing.vest instead")]
        public ushort vest
        {
            get => 0;
            set
            {
                clothing.vest = value;
            }
        }
        [Obsolete("Use clothing.glasses instead")]
        public ushort glasses
        {
            get => 0;
            set
            {
                clothing.glasses = value;
            }
        }
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializehat() => false;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializemask() => false;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializetop() => false;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializevest() => false;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializebackpack() => false;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeglasses() => false;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializebottom() => false;
    }
}
