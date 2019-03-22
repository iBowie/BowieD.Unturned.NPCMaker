using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// Quest declaring error (Disabled)
    /// </summary>
    public class NE_2000 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.HIGH;
        public override string MistakeNameKey => "NE_2000";
        public override string MistakeDescKey => "NE_2000_Desc";
        public override bool IsMistake => false;
        public override bool TranslateName => false;
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 2;
        };
    }
}
