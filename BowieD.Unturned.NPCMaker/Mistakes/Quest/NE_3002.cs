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
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_3002_Desc", _quest.title, _quest.id),
                        Importance = IMPORTANCE.ADVICE,
                        OnClick = new Action(() =>
                        {
                            if (MainWindow.Instance.MainWindowViewModel.QuestTabViewModel.ID == 0)
                                return;
                            MainWindow.Instance.MainWindowViewModel.QuestTabViewModel.SaveCommand.Execute(null);
                            MainWindow.Instance.MainWindowViewModel.QuestTabViewModel.Quest = _quest;
                            MainWindow.Instance.mainTabControl.SelectedIndex = 3;
                        })
                    };
                }
            }
        }
    }
}
