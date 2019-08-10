using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// Dialogue has zero id
    /// </summary>
    public class NE_1001 : Mistake
    {
        public NE_1001() { }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _dial in MainWindow.CurrentProject.data.dialogues)
            {
                if (_dial.id == 0)
                {
                    yield return new NE_1001()
                    {
                        MistakeName = "NE_1001",
                        Importance = IMPORTANCE.CRITICAL,
                        MistakeDesc = LocUtil.LocalizeMistake("NE_1001_Desc", _dial.id),
                        OnClick = new Action(() =>
                        {
                            if (MainWindow.DialogueEditor.Current.id == 0)
                                return;
                            MainWindow.DialogueEditor.Save();
                            MainWindow.DialogueEditor.Current = _dial;
                            MainWindow.Instance.mainTabControl.SelectedIndex = 1;
                        })
                    };
                }
            }
        }
    }
}
