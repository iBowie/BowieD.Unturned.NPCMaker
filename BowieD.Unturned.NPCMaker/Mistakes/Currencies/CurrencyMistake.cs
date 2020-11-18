using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.Currencies
{
    public abstract class CurrencyMistake : Mistake
    {
        public CurrencyMistake()
        {
            OnClick = new Action(() =>
            {
                MainWindow.Instance.mainTabControl.SelectedIndex = 4;
            });
        }
    }
}
