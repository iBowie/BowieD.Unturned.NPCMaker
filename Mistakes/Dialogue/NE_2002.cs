using System;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// One of dialogues has zero id (How?)
    /// </summary>
    public class NE_2002 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override bool IsMistake => MainWindow.CurrentSave.dialogues.Any(d => d.id == 0);
        public override string MistakeDescKey => "NE_2002_Desc";
        public override string MistakeNameKey => "NE_2002";
        public override bool TranslateName => false;
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 2;
        };
    }
}
