using BowieD.Unturned.NPCMaker.NPC;
using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.Vendor
{
    /// <summary>
    /// No items in vendor
    /// </summary>
    public class NE_3002 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.NO_EXPORT;
        public override bool IsMistake
        {
            get
            {
                foreach (NPCVendor vendor in MainWindow.CurrentNPC.vendors)
                {
                    if (vendor.items.Count == 0)
                    {
                        errorVendor = vendor;
                        return true;
                    }
                }
                return false;
            }
        }
        public NPCVendor errorVendor;
        public override string MistakeDescKey => MainWindow.Localize("NE_3002_Desc", errorVendor.id);
        public override string MistakeNameKey => "NE_3002";
        public override bool TranslateName => false;
        public override bool TranslateDesc => false;
        public override Action OnClick
        {
            get
            {
                return new Action(() =>
                {
                    if (MainWindow.Instance.CurrentVendor.id == 0)
                        return;
                    MainWindow.Instance.SaveVendor_Click(null, null);
                    MainWindow.Instance.CurrentVendor = errorVendor;
                    MainWindow.Instance.mainTabControl.SelectedIndex = 2;
                });
            }
        }
    }
}
