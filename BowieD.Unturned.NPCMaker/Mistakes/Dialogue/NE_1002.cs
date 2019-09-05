using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// Some messages will never appear
    /// </summary>
    public class NE_1002 : Mistake
    {
        public NE_1002() { }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _dial in MainWindow.CurrentProject.data.dialogues)
            {
                if (_dial.messages.Count >= 2)
                {
                    for (int k = 0; k < _dial.messages.Count - 1; k++)
                    {
                        if (_dial.messages[k].conditions.Length == 0)
                            yield return new NE_1002()
                            {
                                MistakeName = "NE_1002",
                                MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1002_Desc", _dial.id),
                                Importance = IMPORTANCE.ADVICE,
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
}
