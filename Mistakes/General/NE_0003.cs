using System;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    /// <summary>
    /// NPC id equals zero
    /// </summary>
    public class NE_0003 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override bool IsMistake => MainWindow.CurrentSave.characters.Any(d => d.id == 0);
        public override string MistakeNameKey => "NE_0003";
        public override string MistakeDescKey => "NE_0003_Desc";
        public override bool TranslateName => false;
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 0;
        };
    }
}
