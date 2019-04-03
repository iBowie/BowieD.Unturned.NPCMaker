using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    /// <summary>
    /// NPC id between 1 and 2000 (Official content recommendation)
    /// </summary>
    public class NE_0000 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.WARNING;
        public override bool IsMistake => MainWindow.Instance.txtID.Value > 0 && MainWindow.Instance.txtID.Value <= 2000;
        public override string MistakeNameKey => "NE_0000";
        public override string MistakeDescKey => "NE_0000_Desc";
        public override bool TranslateName => false;
        public override Action OnClick => () => 
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 0;
        };
    }
}
