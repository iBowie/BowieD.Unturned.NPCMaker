using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    /// <summary>
    /// NPC id equals zero
    /// </summary>
    public class NE_0003 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.NO_EXPORT;
        public override bool IsMistake => MainWindow.CurrentNPC.id == 0;
        public override string MistakeNameKey => "NE_0003";
        public override string MistakeDescKey => "NE_0003_Desc";
        public override bool TranslateName => false;
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 0;
        };
    }
}
