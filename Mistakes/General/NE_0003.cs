using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Linq;
using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    /// <summary>
    /// NPC id equals zero
    /// </summary>
    public class NE_0003 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override bool IsMistake => MainWindow.CurrentProject.characters.Any(d => d.id == 0);
        public override string MistakeNameKey => "NE_0003";
        public override string MistakeDescKey => LocUtil.LocalizeMistake("NE_0003_Desc", failChar.displayName, failChar.id);
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
