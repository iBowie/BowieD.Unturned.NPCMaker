using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Linq;
using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// Some of messages will never appear
    /// </summary>
    public class NE_2004 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.ADVICE;
        public override bool IsMistake
        {
            get
            {
                foreach (NPCDialogue dial in MainWindow.CurrentProject.data.dialogues)
                {
                    if (dial.MessagesAmount >= 2)
                    {
                        for (int k = 0; k < dial.messages.Count - 1; k++)
                        {
                            if (dial.messages[k].conditions.Count() == 0)
                            {
                                errorDialogue = dial;
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }
        public override string MistakeDescKey => LocUtil.LocalizeMistake("NE_2004_Desc", errorDialogue.id);
        public override string MistakeNameKey => "NE_2004";
        public override bool TranslateName => false;
        public override bool TranslateDesc => false;
        private NPCDialogue errorDialogue;
        public override Action OnClick
        {
            get
            {
                return new Action(() => 
                {
                    if (MainWindow.DialogueEditor.Current.id == 0)
                        return;
                    MainWindow.DialogueEditor.Save();
                    MainWindow.DialogueEditor.Current = errorDialogue;
                    MainWindow.Instance.mainTabControl.SelectedIndex = 2;
                });
            }
        }
    }
}
