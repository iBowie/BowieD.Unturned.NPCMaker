using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quest
{
    /// <summary>
    /// No conditions
    /// </summary>
    public class NE_3001 : Mistake
    {
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _quest in MainWindow.CurrentProject.data.quests)
            {
                if (_quest.conditions.Count == 0)
                {
                    yield return new NE_3001()
                    {
                        MistakeName = "NE_3001",
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_3001_Desc", _quest.title, _quest.id),
                        Importance = IMPORTANCE.WARNING,
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
