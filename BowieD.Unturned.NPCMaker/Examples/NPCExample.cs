using BowieD.Unturned.NPCMaker.NPC;

namespace BowieD.Unturned.NPCMaker.Examples
{
    public class NPCExample : NPCSaveOld
    {
        public string Title { get; set; }

        public NPCExample() : base() // todo
        {
            IsReadOnly = true;
        }

        public NPCExample(NPCSaveOld save)
        {
            this.IsReadOnly = true;
            this.clothing = save.clothing;
            this.christmasClothing = save.christmasClothing;
            this.halloweenClothing = save.halloweenClothing;
            this.beard = save.beard;
            this.dialogues = save.dialogues;
            this.displayName = save.displayName;
            this.editorName = save.editorName;
            this.equipped = save.equipped;
            this.equipPrimary = save.equipPrimary;
            this.equipSecondary = save.equipSecondary;
            this.equipTertiary = save.equipTertiary;
            this.face = save.face;
            this.guid = save.guid;
            this.hairColor = save.hairColor;
            this.haircut = save.haircut;
            this.id = save.id;
            this.leftHanded = save.leftHanded;
            this.pose = save.pose;
            this.quests = save.quests;
            this.skinColor = save.skinColor;
            this.startDialogueId = save.startDialogueId;
            this.vendors = save.vendors;
            this.visibilityConditions = save.visibilityConditions;
        }
    }
}
