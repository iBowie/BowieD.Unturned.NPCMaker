using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Character
{
    /// <summary>
    /// NPC id equals zero
    /// </summary>
    public class NE_0001 : Mistake
    {
        public NE_0001() { }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _char in MainWindow.CurrentProject.data.characters)
            {
                if (_char.id == 0)
                {
                    yield return new NE_0001()
                    {
                        MistakeName = "NE_0001",
                        Importance = IMPORTANCE.CRITICAL,
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_0001_Desc", _char.displayName, _char.id),
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
