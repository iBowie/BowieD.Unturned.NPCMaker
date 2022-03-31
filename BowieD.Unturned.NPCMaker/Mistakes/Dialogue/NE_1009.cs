using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    public class NE_1009 : DialogueMistake
    {
        public NE_1009()
        {
            MistakeName = "NE_1009";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_1009(int responseId, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1009_Desc", responseId, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCDialogue dial in MainWindow.CurrentProject.data.dialogues)
            {
                for (int i = 0; i < dial.Responses.Count; i++)
                {
                    NPC.NPCResponse response = dial.Responses[i];

                    if (string.IsNullOrEmpty(response.mainText))
                    {
                        yield return new NE_1009(i + 1, dial.ID);
                    }
                }
            }
        }
    }
}
