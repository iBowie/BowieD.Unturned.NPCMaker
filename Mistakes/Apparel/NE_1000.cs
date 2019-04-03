using System;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Mistakes.Apparel
{
    /// <summary>
    /// Skin color invalid
    /// </summary>
    public class NE_1000 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override bool IsMistake => MainWindow.Instance.apparelSkinColorBox.Text.Length > 0 && MainWindow.Instance.apparelSkinColorBox.Text.Length < 6 && !new BrushConverter().IsValid(MainWindow.Instance.apparelSkinColorBox.Text);
        public override string MistakeNameKey => "NE_1000";
        public override bool TranslateName => false;
        public override string MistakeDescKey => "NE_1000_Desc";
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 1;
        };
    }
}
