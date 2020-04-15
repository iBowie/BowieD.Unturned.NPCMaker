using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.Character
{
    public abstract class CharacterMistake : Mistake
    {
        public CharacterMistake()
        {
            OnClick = new Action(() =>
            {
                MainWindow.Instance.mainTabControl.SelectedIndex = 0;
            });
        }
    }
}
