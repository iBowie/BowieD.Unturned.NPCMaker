using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    /// <summary>
    /// NPC id between 1 and 2000 (Official content recommendation)
    /// </summary>
    public class NE_0000 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.WARNING;
        //public override bool IsMistake => MainWindow.Instance.txtID.Value > 0 && MainWindow.Instance.txtID.Value <= 2000;
        public override bool IsMistake
        {
            get
            {
                foreach (var _char in MainWindow.CurrentProject.data.characters)
                {
                    if (_char.id > 0 && _char.id <= 2000)
                    {
                        failChar = _char;
                        return true;
                    }
                }
                return false;
            }
        }
        private NPCCharacter failChar = null;
        public override string MistakeNameKey => "NE_0000";
        public override string MistakeDescKey => LocUtil.LocalizeMistake("NE_0000_Desc", failChar.displayName, failChar.id);
        public override bool TranslateName => false;
        public override bool TranslateDesc => false;
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
