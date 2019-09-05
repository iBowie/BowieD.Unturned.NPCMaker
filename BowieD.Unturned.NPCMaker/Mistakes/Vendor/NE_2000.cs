using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Vendor
{
    /// <summary>
    /// Zero id
    /// </summary>
    public class NE_2000 : Mistake
    {
        public NE_2000() { }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _vend in MainWindow.CurrentProject.data.vendors)
            {
                if (_vend.id == 0)
                {
                    yield return new NE_2000()
                    {
                        MistakeName = "NE_2000",
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_2000_Desc", _vend.vendorTitle, _vend.id),
                        Importance = IMPORTANCE.CRITICAL,
                        OnClick = new Action(() =>
                        {
                            if (MainWindow.VendorEditor.Current.id == 0)
                                return;
                            MainWindow.VendorEditor.Save();
                            MainWindow.VendorEditor.Current = _vend;
                            MainWindow.Instance.mainTabControl.SelectedIndex = 2;
                        })
                    };
                }
            }
        }
    }
}
