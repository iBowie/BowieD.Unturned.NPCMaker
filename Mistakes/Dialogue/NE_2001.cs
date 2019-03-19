using System;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// No pages in message
    /// </summary>
    public class NE_2001 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.NO_EXPORT;
        public override bool IsMistake => MainWindow.CurrentNPC.dialogues == null ? false : MainWindow.CurrentNPC.dialogues.Any(d => d.messages.Any(k => k.PagesAmount == 0));
        public override string MistakeNameKey => "NE_2001";
        public override string MistakeDescKey => "NE_2001_Desc";
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 2;
        };
    }
}
