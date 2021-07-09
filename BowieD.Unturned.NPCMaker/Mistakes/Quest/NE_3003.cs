using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quest
{
    public class NE_3003 : QuestMistake
    {
        public NE_3003()
        {
            MistakeName = "NE_3003";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_3003(string title, ushort id, ushort min, ushort max) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_3003_Desc", title, id, min, max);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            var rangeMin = MainWindow.CurrentProject.data.settings.idRangeMin;
            var rangeMax = MainWindow.CurrentProject.data.settings.idRangeMax;

            foreach (NPC.NPCQuest _quest in MainWindow.CurrentProject.data.quests)
            {
                if (_quest.ID < rangeMin || _quest.ID > rangeMax)
                    yield return new NE_3003(_quest.Title, _quest.ID, rangeMin, rangeMax);
            }
        }
    }
}
