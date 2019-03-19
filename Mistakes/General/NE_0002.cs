using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    /// <summary>
    /// Display name length lesser than 3 characters
    /// </summary>
    public class NE_0002 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.ADVICE;
        public override bool IsMistake => MainWindow.Instance.txtDisplayName.Text?.Length < 3;
        public override string MistakeNameKey => "NE_0002";
        public override string MistakeDescKey => "NE_0002_Desc";
        public override bool TranslateName => false;
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 0;
        };
    }
}
