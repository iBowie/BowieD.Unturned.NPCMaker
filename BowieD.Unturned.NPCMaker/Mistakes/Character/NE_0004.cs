using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Character
{
    public class NE_0004 : CharacterMistake
    {
        public NE_0004()
        {
            MistakeName = "NE_0004";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_0004(string displayName, ushort id, ushort min, ushort max) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_0004_Desc", displayName, id, min, max);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            var rangeMin = MainWindow.CurrentProject.data.settings.idRangeMin;
            var rangeMax = MainWindow.CurrentProject.data.settings.idRangeMax;

            foreach (NPC.NPCCharacter _char in MainWindow.CurrentProject.data.characters)
            {
                if (_char.ID < rangeMin || _char.ID > rangeMax)
                    yield return new NE_0004(_char.DisplayName, _char.ID, rangeMin, rangeMax);
            }
        }
    }
}
