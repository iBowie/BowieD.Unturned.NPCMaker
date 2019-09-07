using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Vendor
{
    /// <summary>
    /// No items
    /// </summary>
    public class NE_2001 : Mistake
    {
        public NE_2001() { }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _vend in MainWindow.CurrentProject.data.vendors)
            {
                if (_vend.items.Count == 0)
                {
                    yield return new NE_2001()
                    {
                        MistakeName = "NE_2001",
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_2001_Desc", _vend.vendorTitle, _vend.id),
                        Importance = IMPORTANCE.WARNING,
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
