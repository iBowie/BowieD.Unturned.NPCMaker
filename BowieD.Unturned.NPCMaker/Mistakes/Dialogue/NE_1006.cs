using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// Late update
    /// </summary>
    public sealed class NE_1006 : DialogueMistake
    {
        public NE_1006() : base()
        {
            MistakeName = "NE_1006";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_1006(int responseId, ushort dialogueId) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1006_Desc", responseId, dialogueId);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCDialogue dial in MainWindow.CurrentProject.data.dialogues)
            {
                for (int i = 0; i < dial.Responses.Count; i++)
                {
                    NPC.NPCResponse response = dial.Responses[i];

                    if (response.openQuestId > 0 && response.openDialogueId == 0)
                    {
                        yield return new NE_1006(i + 1, dial.ID);
                    }
                }
            }
        }
    }
}
