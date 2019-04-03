using System;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Mistakes.Apparel
{
    /// <summary>
    /// Invalid hair color
    /// </summary>
    public class NE_1001 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override bool IsMistake => MainWindow.Instance.apparelHairColorBox.Text.Length > 0 && MainWindow.Instance.apparelHairColorBox.Text.Length < 6 && !new BrushConverter().IsValid(MainWindow.Instance.apparelHairColorBox.Text);
        public override string MistakeNameKey => "NE_1001";
        public override bool TranslateName => false;
        public override string MistakeDescKey => "NE_1001_Desc";
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 1;
        };
    }
}
