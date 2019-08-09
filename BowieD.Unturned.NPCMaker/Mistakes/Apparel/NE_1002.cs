using BowieD.Unturned.NPCMaker.NPC;
using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.Apparel
{
    /// <summary>
    /// Equipped slot is empty
    /// </summary>
    public class NE_1002 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.CRITICAL;
        public override bool IsMistake
        {
            get
            {
                foreach (NPCCharacter character in MainWindow.CurrentProject.data.characters)
                {
                    switch (character.equipped)
                    {
                        case NPC.Equip_Type.Primary when character.equipPrimary == 0:
                        case NPC.Equip_Type.Secondary when character.equipSecondary == 0:
                        case NPC.Equip_Type.Tertiary when character.equipTertiary == 0:
                            failChar = character;
                            return true;
                    }
                }
                return false;
            }
        }
        public override string MistakeNameKey => "NE_1002";
        public override bool TranslateName => false;
        public override string MistakeDescKey => "NE_1002_Desc";
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
