using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// Recursive call
    /// </summary>
    public sealed class NE_1007 : DialogueMistake
    {
        public NE_1007() : base()
        {
            MistakeName = "NE_1007";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_1007(int responseId, ushort dialogueId) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1007_Desc", responseId, dialogueId);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCDialogue dial in MainWindow.CurrentProject.data.dialogues)
            {
                for (int i = 0; i < dial.responses.Count; i++)
                {
                    NPC.NPCResponse response = dial.responses[i];

                    if (response.openDialogueId == dial.id)
                    {
                        yield return new NE_1007(i + 1, dial.id);
                    }
                }
            }
        }
    }
}
