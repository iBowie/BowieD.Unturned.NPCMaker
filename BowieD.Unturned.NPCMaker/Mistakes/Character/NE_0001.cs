using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Character
{
    /// <summary>
    /// NPC id equals zero
    /// </summary>
    public class NE_0001 : CharacterMistake
    {
        public NE_0001()
        {
            MistakeName = "NE_0001";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_0001(string displayName, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_0001_Desc", displayName, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCCharacter _char in MainWindow.CurrentProject.data.characters)
            {
                if (_char.ID == 0)
                {
                    yield return new NE_0001(_char.DisplayName, _char.ID);
                }
            }
        }
    }
}
