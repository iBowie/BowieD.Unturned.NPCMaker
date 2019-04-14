using System;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quests
{
    /// <summary>
    /// One of quests have zero id
    /// </summary>
    public class NE_4000 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override bool IsMistake => MainWindow.CurrentSave.quests.Any(d => d.id == 0);
        public override string MistakeNameKey => "NE_4000";
        public override string MistakeDescKey => "NE_4000_Desc";
        public override bool TranslateName => false;
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 4;
        };
    }
}
