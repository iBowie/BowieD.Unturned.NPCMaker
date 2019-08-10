using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quest
{
    /// <summary>
    /// No rewards
    /// </summary>
    public class NE_3002 : Mistake
    {
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _quest in MainWindow.CurrentProject.data.quests)
            {
                if (_quest.rewards.Count == 0)
                {
                    yield return new NE_3002()
                    {
                        MistakeName = "NE_3002",
                        MistakeDesc = LocUtil.LocalizeMistake("NE_3002_Desc", _quest.title, _quest.id),
                        Importance = IMPORTANCE.ADVICE,
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
