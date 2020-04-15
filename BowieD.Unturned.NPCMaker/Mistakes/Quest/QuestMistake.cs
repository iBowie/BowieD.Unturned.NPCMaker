using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quest
{
    public abstract class QuestMistake : Mistake
    {
        public QuestMistake()
        {
            OnClick = new Action(() =>
            {
                MainWindow.Instance.mainTabControl.SelectedIndex = 3;
            });
        }
    }
}
