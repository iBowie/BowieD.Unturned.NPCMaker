using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Character
{
    public class NE_0003 : Mistake
    {
        public NE_0003() { }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _char in MainWindow.CurrentProject.data.characters)
            {
                if ("<>:\"/\\|?*".Contains(_char.editorName))
                {
                    yield return new Mistake()
                    {
                        MistakeName = "NE_0003",
                        Importance = IMPORTANCE.CRITICAL,
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_0003_Desc", _char.displayName, _char.id),
                        OnClick = new Action(() =>
                        {
                            MainWindow.Instance.mainTabControl.SelectedIndex = 0;
                        })
                    };
                }
            }
        }
    }
}
