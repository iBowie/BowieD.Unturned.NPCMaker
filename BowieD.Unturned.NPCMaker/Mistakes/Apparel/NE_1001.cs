using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.Apparel
{
    /// <summary>
    /// Invalid hair color (Disabled)
    /// </summary>
    public class NE_1001 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override bool IsMistake
        {
            get
            {
                return false;
            }
        }

        public override string MistakeNameKey => "NE_1001";
        public override bool TranslateName => false;
        public override string MistakeDescKey => "NE_1001_Desc";
        public override Action OnClick => () =>
        {
            //if (MainWindow.CharacterEditor.Current.id == 0)
            //    return;
            //MainWindow.CharacterEditor.Save();
            //MainWindow.CharacterEditor.Current = failChar;
            //MainWindow.Instance.mainTabControl.SelectedIndex = 0;
        };
    }
}
