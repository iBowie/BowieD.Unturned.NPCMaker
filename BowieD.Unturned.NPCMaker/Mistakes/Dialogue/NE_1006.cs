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
            foreach (var dial in MainWindow.CurrentProject.data.dialogues)
            {
                for (int i = 0; i < dial.responses.Count; i++)
                {
                    var response = dial.responses[i];

                    if (response.openQuestId > 0 || response.openVendorId > 0)
                    {
                        if (response.openDialogueId == 0)
                            yield return new NE_1006(i + 1, dial.id);
                    }
                }
            }
        }
    }
}
