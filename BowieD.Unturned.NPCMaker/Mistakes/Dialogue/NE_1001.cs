using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// Dialogue has zero id
    /// </summary>
    public class NE_1001 : DialogueMistake
    {
        public NE_1001() : base()
        {
            MistakeName = "NE_1001";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_1001(ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1001_Desc", id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _dial in MainWindow.CurrentProject.data.dialogues)
            {
                if (_dial.id == 0)
                {
                    yield return new NE_1001(_dial.id);
                }
            }
        }
    }
}
