using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    public class NE_1010 : DialogueMistake
    {
        public NE_1010()
        {
            MistakeName = "NE_1010";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_1010(int messageId, int pageId, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1010_Desc", messageId, pageId, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCDialogue dial in MainWindow.CurrentProject.data.dialogues)
            {
                for (int i = 0; i < dial.Messages.Count; i++)
                {
                    var message = dial.Messages[i];

                    for (int j = 0; j < message.pages.Count; j++)
                    {
                        var page = message.pages[j];

                        if (string.IsNullOrEmpty(page))
                        {
                            yield return new NE_1010(i + 1, j + 1, dial.ID);
                        }
                    }
                }
            }
        }
    }
}
