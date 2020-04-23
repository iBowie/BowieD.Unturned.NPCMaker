using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.Vendor
{
    public abstract class VendorMistake : Mistake
    {
        public VendorMistake()
        {
            OnClick = new Action(() =>
            {
                MainWindow.Instance.mainTabControl.SelectedIndex = 2;
            });
        }
    }
}
