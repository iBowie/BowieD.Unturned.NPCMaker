using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    public sealed class NE_1003 : DialogueMistake
    {
        public NE_1003() : base()
        {
            MistakeName = "NE_1003";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_1003(int pageId, int messageId, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1003_Desc", pageId, messageId, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCDialogue dial in MainWindow.CurrentProject.data.dialogues)
            {
                for (int mId = 0; mId < dial.Messages.Count; mId++)
                {
                    for (int pId = 0; pId < dial.Messages[mId].pages.Count; pId++)
                    {
                        string page = dial.Messages[mId].pages[pId];
                        if (page == null || page.Length == 0)
                        {
                            yield return new NE_1003(pId + 1, mId + 1, dial.ID);
                        }
                    }
                }
            }
        }
    }
}
