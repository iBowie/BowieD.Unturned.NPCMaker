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
                            if (MainWindow.Instance.MainWindowViewModel.VendorTabViewModel.ID == 0)
                                return;
                            MainWindow.Instance.MainWindowViewModel.VendorTabViewModel.SaveCommand.Execute(null);
                            MainWindow.Instance.MainWindowViewModel.VendorTabViewModel.Vendor = _vend;
                            MainWindow.Instance.mainTabControl.SelectedIndex = 2;
                        })
                    };
                }
            }
        }
    }
}
