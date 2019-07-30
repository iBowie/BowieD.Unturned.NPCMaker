using System.Collections.Generic;

namespace BowieD.NPCMaker.NPC
{
    public sealed class Character
    {
        public Character()
        {
            editorName = new Dictionary<ELanguage, string>();
            displayName = new Dictionary<ELanguage, string>();
            visibilityConditions = new List<Condition.Condition>();
        }
        public string guid;
        public Dictionary<ELanguage, string> editorName;
        public Dictionary<ELanguage, string> displayName;
        public ushort id;
        public ushort dialogueId;
        public byte face;
        public byte beard;
        public byte hair;
        public Coloring.Color hairColor;
        public Coloring.Color skinColor;
        public Outfit defaultClothing;
        public Outfit christmasClothing;
        public Outfit halloweenClothing;
        public ENPCPose pose;
        public bool leftHanded;
        public ushort equipPrimary;
        public ushort equipSecondary;
        public ushort equipTertiary;
        public ESlotType equippedSlot;
        public List<Condition.Condition> visibilityConditions;
    }
}
