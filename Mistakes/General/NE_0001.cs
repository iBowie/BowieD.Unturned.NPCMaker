using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    /// <summary>
    /// Editor name contains non-english characters (Disabled)
    /// </summary>
    public class NE_0001 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.ADVICE;
        public override bool IsMistake => false;
        public override string MistakeNameKey => "NE_0001";
        public override string MistakeDescKey => "NE_0001_Desc";
        public override bool TranslateName => false;
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 0;
        };
    }
}
