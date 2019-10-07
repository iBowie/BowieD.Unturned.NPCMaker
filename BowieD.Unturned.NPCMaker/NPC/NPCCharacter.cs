using BowieD.Unturned.NPCMaker.Coloring;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCCharacter : IHasDisplayName
    {
        public NPCCharacter()
        {
            guid = Guid.NewGuid().ToString("N");
            EditorName = "";
            GameName = "";
            ID = 0;
            DialogueID = 0;
            visibilityConditions = new List<Condition>();
            FaceID = 0;
            Beard = 0;
            Haircut = 0;
            HairColor = new Color(0, 0, 0);
            SkinColor = new Color(0, 0, 0);
            Clothing = new NPCClothing();
            HalloweenClothing = new NPCClothing();
            ChristmasClothing = new NPCClothing();
            Pose = NPC_Pose.Stand;
            LeftHanded = false;
            EquipPrimary = 0;
            EquipSecondary = 0;
            EquipTertiary = 0;
            Equipped = Equip_Type.None;
        }
        [XmlElement("editorName")]
        public string EditorName { get; set; }
        [XmlElement("displayName")]
        public string GameName { get; set; }
        [XmlElement("id")]
        public ushort ID { get; set; }
        [XmlElement("startDialogueId")]
        public ushort DialogueID { get; set; }
        [XmlElement("face")]
        public byte FaceID { get; set; }
        [XmlElement("beard")]
        public byte Beard{get;set;}
        [XmlElement("haircut")]
        public byte Haircut{get;set;}
        [XmlElement("hairColor")]
        public Color HairColor{get;set;}
        [XmlElement("skinColor")]
        public Color SkinColor{get;set;}
        [XmlElement("clothing")]
        public NPCClothing Clothing{get;set;}
        [XmlElement("christmasClothing")]
        public NPCClothing ChristmasClothing{get;set;}
        [XmlElement("halloweenClothing")]
        public NPCClothing HalloweenClothing{get;set;}
        [XmlElement("pose")]
        public NPC_Pose Pose{get;set;}
        [XmlElement("leftHanded")]
        public bool LeftHanded{get;set;}
        [XmlElement("equipPrimary")]
        public ushort EquipPrimary{get;set;}
        [XmlElement("equipSecondary")]
        public ushort EquipSecondary{get;set;}
        [XmlElement("equipTertiary")]
        public ushort EquipTertiary{get;set;}
        [XmlElement("equipped")]
        public Equip_Type Equipped{get;set;}

        [XmlAttribute]
        public string guid;
        public List<Condition> visibilityConditions;

        public string DisplayName => $"[{ID}] {EditorName} - {GameName}";
    }
}
