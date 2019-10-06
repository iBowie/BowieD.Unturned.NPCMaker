using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Character
{
    /// <summary>
    /// NPC id between 1 and 2000 (Official content recommendation)
    /// </summary>
    public class NE_0000 : Mistake
    {
        public NE_0000() { }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _char in MainWindow.CurrentProject.data.characters)
            {
                if (_char.id > 0 && _char.id <= 2000)
                {
                    yield return new NE_0000()
                    {
                        MistakeName = "NE_0000",
                        Importance = IMPORTANCE.ADVICE,
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_0000_Desc", _char.displayName, _char.id),
                        OnClick = new Action(() =>
                        {
                            //if (MainWindow.CharacterEditor.Current.id == 0)
                            //    return;
                            //MainWindow.CharacterEditor.Save();
                            //MainWindow.CharacterEditor.Current = _char;
                            MainWindow.Instance.mainTabControl.SelectedIndex = 0;
                        })
                    };
                }
            }
        }
    }
}
