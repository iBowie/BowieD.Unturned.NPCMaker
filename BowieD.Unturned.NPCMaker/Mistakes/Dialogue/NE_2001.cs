using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Linq;
using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// No pages in message
    /// </summary>
    public class NE_2001 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override bool IsMistake
        {
            get
            {
                foreach (NPCDialogue dialogue in MainWindow.CurrentProject.data.dialogues)
                {
                    if (dialogue.MessagesAmount > 0 && dialogue.messages.Any(d => d.PagesAmount == 0))
                    {
                        errorDialogue = dialogue;
                        return true;
                    }
                }
                return false;
            }
        }

        public override string MistakeNameKey => "NE_2001";
        public override string MistakeDescKey => LocUtil.LocalizeMistake("NE_2001_Desc", errorDialogue.id);
        public override bool TranslateName => false;
        public override bool TranslateDesc => false;
        private NPCDialogue errorDialogue;
        public override Action OnClick => () =>
        {
            if (MainWindow.DialogueEditor.Current.id == 0)
                return;
            MainWindow.DialogueEditor.Save();
            MainWindow.DialogueEditor.Current = errorDialogue;
            MainWindow.Instance.mainTabControl.SelectedIndex = 2;
        };
    }
}
