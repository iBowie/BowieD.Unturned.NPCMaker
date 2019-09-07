using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Character
{
    /// <summary>
    /// Equipped slot is empty
    /// </summary>
    public class NE_0002 : Mistake
    {
        public NE_0002() { }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _char in MainWindow.CurrentProject.data.characters)
            {
                switch (_char.equipped)
                {
                    case NPC.Equip_Type.Primary when _char.equipPrimary == 0:
                    case NPC.Equip_Type.Secondary when _char.equipSecondary == 0:
                    case NPC.Equip_Type.Tertiary when _char.equipTertiary == 0:
                        yield return new NE_0002()
                        {
                            MistakeName = "NE_0002",
                            Importance = IMPORTANCE.CRITICAL,
                            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_0002_Desc", _char.displayName, _char.id),
                            OnClick = new Action(() =>
                            {
                                if (MainWindow.CharacterEditor.Current.id == 0)
                                    return;
                                MainWindow.CharacterEditor.Save();
                                MainWindow.CharacterEditor.Current = _char;
                                MainWindow.Instance.mainTabControl.SelectedIndex = 0;
                            })
                        };
                        break;
                }
            }
        }
    }
}
