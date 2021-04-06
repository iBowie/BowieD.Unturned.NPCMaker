using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    public class NE_1008 : DialogueMistake
    {
        public NE_1008()
        {
            MistakeName = "NE_1008";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_1008(ushort id, ushort min, ushort max) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1008_Desc", id, min, max);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            var rangeMin = MainWindow.CurrentProject.data.settings.idRangeMin;
            var rangeMax = MainWindow.CurrentProject.data.settings.idRangeMax;

            foreach (NPC.NPCDialogue _dial in MainWindow.CurrentProject.data.dialogues)
            {
                if (_dial.ID < rangeMin || _dial.ID > rangeMax)
                    yield return new NE_1008(_dial.ID, rangeMin, rangeMax);
            }
        }
    }
}
