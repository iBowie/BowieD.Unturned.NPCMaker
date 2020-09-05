using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// Some messages will never appear
    /// </summary>
    public class NE_1002 : DialogueMistake
    {
        public NE_1002() : base()
        {
            MistakeName = "NE_1002";
            Importance = IMPORTANCE.ADVICE;
        }
        public NE_1002(ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1002_Desc", id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCDialogue _dial in MainWindow.CurrentProject.data.dialogues)
            {
                if (_dial.Messages.Count >= 2)
                {
                    for (int k = 0; k < _dial.Messages.Count - 1; k++)
                    {
                        if (_dial.Messages[k].conditions.Count == 0)
                        {
                            yield return new NE_1002(_dial.ID);
                        }
                    }
                }
            }
        }
    }
}
