using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.Apparel
{
    /// <summary>
    /// Equipped slot is empty
    /// </summary>
    public class NE_1002 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.NO_EXPORT;
        public override bool IsMistake
        {
            get
            {
                switch (MainWindow.CurrentNPC.equipped)
                {
                    case NPC.Equip_Type.Primary when MainWindow.CurrentNPC.equipPrimary == 0:
                    case NPC.Equip_Type.Secondary when MainWindow.CurrentNPC.equipSecondary == 0:
                    case NPC.Equip_Type.Tertiary when MainWindow.CurrentNPC.equipTertiary == 0:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public override string MistakeNameKey => "NE_1002";
        public override bool TranslateName => false;
        public override string MistakeDescKey => "NE_1002_Desc";
        public override Action OnClick => () =>
        {
            MainWindow.Instance.mainTabControl.SelectedIndex = 1;
        };
    }
}
