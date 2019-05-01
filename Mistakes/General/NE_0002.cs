using BowieD.Unturned.NPCMaker.NPC;
using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    /// <summary>
    /// Display name length lesser than 3 characters
    /// </summary>
    public class NE_0002 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.ADVICE;
        public override bool IsMistake
        {
            get
            {
                foreach (var _char in MainWindow.CurrentSave.characters)
                {
                    if (_char.displayName == null || _char.displayName.Length < 3)
                    {
                        failChar = _char;
                        return true;
                    }
                }
                return false;
                //return MainWindow.Instance.txtDisplayName.Text?.Length < 3;
            }
        }

        public override string MistakeNameKey => "NE_0002";
        public override string MistakeDescKey => MainWindow.Localize("NE_0002_Desc", failChar.displayName, failChar.id);
        public override bool TranslateName => false;
        public override bool TranslateDesc => false;
        private NPCCharacter failChar = null;
        public override Action OnClick => () =>
        {
            if (MainWindow.CharacterEditor.Current.id == 0)
                return;
            MainWindow.CharacterEditor.Save();
            MainWindow.CharacterEditor.Current = failChar;
            MainWindow.Instance.mainTabControl.SelectedIndex = 0;
        };
    }
}
