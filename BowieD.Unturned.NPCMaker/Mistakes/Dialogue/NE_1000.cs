using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// No pages in message
    /// </summary>
    public class NE_1000 : DialogueMistake
    {
        public NE_1000() : base()
        {
            MistakeName = "NE_1000";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_1000(ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1000_Desc", id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _dial in MainWindow.CurrentProject.data.dialogues)
            {
                if (_dial.messages.Count > 0 && _dial.messages.Any(d => d.pages.Count == 0))
                {
                    yield return new NE_1000(_dial.id);
                }
            }
        }
    }
}
