using BowieD.Unturned.NPCMaker.NPC;

namespace BowieD.Unturned.NPCMaker.Examples
{
    public class NPCExample : NPCSave
    {
        public string Title { get; set; }

        public NPCExample()
        {
            IsReadOnly = true;
        }

        public NPCExample(NPCSave save)
        {
            this.IsReadOnly = true;
            this.backpack = save.backpack;
            this.beard = save.beard;
            this.bottom = save.bottom;
            this.dialogues = save.dialogues;
            this.displayName = save.displayName;
            this.editorName = save.editorName;
            this.equipped = save.equipped;
            this.equipPrimary = save.equipPrimary;
            this.equipSecondary = save.equipSecondary;
            this.equipTertiary = save.equipTertiary;
            this.face = save.face;
            this.glasses = save.glasses;
            this.guid = save.guid;
            this.hairColor = save.hairColor;
            this.haircut = save.haircut;
            this.hat = save.hat;
            this.id = save.id;
            this.leftHanded = save.leftHanded;
            this.mask = save.mask;
            this.pose = save.pose;
            this.quests = save.quests;
            this.skinColor = save.skinColor;
            this.startDialogueId = save.startDialogueId;
            this.top = save.top;
            this.vendors = save.vendors;
            this.vest = save.vest;
            this.visibilityConditions = save.visibilityConditions;
        }
    }
}
