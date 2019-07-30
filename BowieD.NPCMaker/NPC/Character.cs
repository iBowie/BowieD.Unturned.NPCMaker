namespace BowieD.NPCMaker.NPC
{
    public sealed class Character
    {
        public string guid;
        public string editorName;
        public string displayName;
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
        public Condition.Condition[] visibilityConditions;
    }
}
