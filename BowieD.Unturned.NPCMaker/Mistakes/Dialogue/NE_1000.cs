using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// No pages in message
    /// </summary>
    public class NE_1000 : Mistake
    {
        public NE_1000() { }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _dial in MainWindow.CurrentProject.data.dialogues)
            {
                if (_dial.messages.Count > 0 && _dial.messages.Any(d => d.pages.Count == 0))
                {
                    yield return new NE_1000()
                    {
                        MistakeName = "NE_1000",
                        Importance = IMPORTANCE.CRITICAL,
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1000_Desc", _dial.id),
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
