using System;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.Vendor
{
    /// <summary>
    /// Vendor duplicate
    /// </summary>
    public class NE_3001 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override string MistakeNameKey => "NE_3001";
        public override bool TranslateName => false;
        public override string MistakeDescKey => "NE_3001_Desc";
        public override bool IsMistake => MainWindow.CurrentProject.vendors.Any(d => MainWindow.CurrentProject.vendors.Count(k => k.id == d.id) > 1);
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 2;
        };
    }
}
