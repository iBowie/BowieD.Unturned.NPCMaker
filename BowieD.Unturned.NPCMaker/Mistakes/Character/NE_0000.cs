using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Character
{
    /// <summary>
    /// NPC id between 1 and 2000 (Official content recommendation)
    /// </summary>
    public class NE_0000 : CharacterMistake
    {
        public NE_0000() : base()
        {
            MistakeName = "NE_0000";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_0000(string displayName, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_0000_Desc", displayName, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCCharacter _char in MainWindow.CurrentProject.data.characters)
            {
                if (_char.ID > 0 && _char.ID <= 2000)
                {
                    yield return new NE_0000(_char.DisplayName, _char.ID);
                }
            }
        }
    }
}
