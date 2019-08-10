using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quest
{
    /// <summary>
    /// Zero id
    /// </summary>
    public class NE_3000 : Mistake
    {
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _quest in MainWindow.CurrentProject.data.quests)
            {
                if (_quest.id == 0)
                {
                    yield return new NE_3000()
                    {
                        MistakeName = "NE_3000",
                        MistakeDesc = LocUtil.LocalizeMistake("NE_3000_Desc", _quest.title, _quest.id),
                        Importance = IMPORTANCE.CRITICAL,
                        OnClick = new Action(() =>
                        {
                            if (MainWindow.QuestEditor.Current.id == 0)
                                return;
                            MainWindow.QuestEditor.Save();
                            MainWindow.QuestEditor.Current = _quest;
                            MainWindow.Instance.mainTabControl.SelectedIndex = 4;
                        })
                    };
                }
            }
        }
    }
}
