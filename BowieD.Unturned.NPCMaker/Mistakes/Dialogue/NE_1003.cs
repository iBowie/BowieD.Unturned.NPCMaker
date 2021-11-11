using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    public sealed class NE_1003 : DialogueMistake
    {
        public NE_1003() : base()
        {
            MistakeName = "NE_1003";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_1003(int pageId, int messageId, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1003_Desc", pageId, messageId, id);
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
                            yield return new NE_1003(j + 1, i + 1, dial.ID);
                        }
                    }
                }
            }
        }
    }
}
