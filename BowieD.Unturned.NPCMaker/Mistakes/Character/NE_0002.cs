using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Character
{
    /// <summary>
    /// Equipped slot is empty
    /// </summary>
    public class NE_0002 : CharacterMistake
    {
        public NE_0002()
        {
            MistakeName = "NE_0002";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_0002(string displayName, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_0002_Desc", displayName, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCCharacter _char in MainWindow.CurrentProject.data.characters)
            {
                switch (_char.equipped)
                {
                    case NPC.Equip_Type.Primary when _char.equipPrimary.IsEmpty:
                    case NPC.Equip_Type.Secondary when _char.equipSecondary.IsEmpty:
                    case NPC.Equip_Type.Tertiary when _char.equipTertiary.IsEmpty:
                        yield return new NE_0002(_char.DisplayName, _char.ID);
                        break;
                }
            }
        }
    }
}
