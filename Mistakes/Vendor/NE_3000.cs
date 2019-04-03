using System;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.Vendor
{
    /// <summary>
    /// One of vendor have zero id
    /// </summary>
    public class NE_3000 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override bool IsMistake => MainWindow.CurrentNPC.vendors.Any(d => d.id == 0);
        public override string MistakeNameKey => "NE_3000";
        public override string MistakeDescKey => "NE_3000_Desc";
        public override bool TranslateName => false;
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 3;
        };
    }
}
