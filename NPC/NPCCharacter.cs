using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCCharacter : IHasDisplayName
    {
        public NPCCharacter()
        {
            guid = Guid.NewGuid().ToString("N");
            editorName = "";
            displayName = "";
            id = 0;
            startDialogueId = 0;
            visibilityConditions = new List<Condition>();
            face = 0;
            beard = 0;
            haircut = 0;
            hairColor = new NPCColor(0, 0, 0);
            skinColor = new NPCColor(0, 0, 0);
            clothing = new NPCClothing();
            halloweenClothing = new NPCClothing();
            christmasClothing = new NPCClothing();
            pose = NPC_Pose.Stand;
            leftHanded = false;
            equipPrimary = 0;
            equipSecondary = 0;
            equipTertiary = 0;
            equipped = Equip_Type.None;
        }
        public string editorName;
        public string displayName;
        public ushort id;
        public ushort startDialogueId;
        public byte face;
        public byte beard;
        public byte haircut;
        public NPCColor hairColor;
        public NPCColor skinColor;
        public NPCClothing clothing;
        public NPCClothing christmasClothing;
        public NPCClothing halloweenClothing;
        public NPC_Pose pose;
        public bool leftHanded;
        public ushort equipPrimary;
        public ushort equipSecondary;
        public ushort equipTertiary;
        public Equip_Type equipped;

        [XmlAttribute]
        public string guid;
        public List<Condition> visibilityConditions;

        public string DisplayName => $"[{id}] {editorName} - {displayName}";
    }
}
