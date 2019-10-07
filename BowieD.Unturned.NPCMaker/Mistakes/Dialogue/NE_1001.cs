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
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1001_Desc", _dial.id),
                        OnClick = new Action(() =>
                        {
                            if (MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.ID == 0)
                                return;
                            MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.SaveCommand.Execute(null);
                            MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.Dialogue = _dial;
                            MainWindow.Instance.mainTabControl.SelectedIndex = 1;
                        })
                    };
                }
            }
        }
    }
}
