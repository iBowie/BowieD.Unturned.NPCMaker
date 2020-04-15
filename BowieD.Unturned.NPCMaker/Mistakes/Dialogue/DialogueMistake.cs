using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    public abstract class DialogueMistake : Mistake
    {
        public DialogueMistake()
        {
            OnClick = new Action(() =>
            {
                MainWindow.Instance.mainTabControl.SelectedIndex = 1;
            });
        }
    }
}
