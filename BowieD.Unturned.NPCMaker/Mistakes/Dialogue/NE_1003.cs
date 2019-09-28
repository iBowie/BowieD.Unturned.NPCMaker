using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    public sealed class NE_1003 : Mistake
    {
        public NE_1003() { }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var dial in MainWindow.CurrentProject.data.dialogues)
            {
                for (int mId = 0; mId < dial.messages.Count; mId++)
                {
                    for (int pId = 0; pId < dial.messages[mId].pages.Count; pId++)
                    {
                        var page = dial.messages[mId].pages[pId];
                        if (page == null || page.Length == 0)
                        {
                            yield return new Mistake()
                            {
                                Importance = IMPORTANCE.WARNING,
                                MistakeName = "NE_1003",
                                MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1003_Desc", pId + 1, mId + 1, dial.id),
                                OnClick = new Action(() =>
                                {
                                    if (MainWindow.DialogueEditor.Current.id == 0)
                                        return;
                                    MainWindow.DialogueEditor.Save();
                                    MainWindow.DialogueEditor.Current = dial;
                                    MainWindow.Instance.mainTabControl.SelectedIndex = 1;
                                })
                            };
                        }
                    }
                }
            }
        }
    }
}
