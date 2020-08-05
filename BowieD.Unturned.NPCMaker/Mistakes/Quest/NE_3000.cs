using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quest
{
    /// <summary>
    /// Zero id
    /// </summary>
    public class NE_3000 : QuestMistake
    {
        public NE_3000() : base()
        {
            MistakeName = "NE_3000";
            Importance = IMPORTANCE.CRITICAL;
        }
        public NE_3000(string title, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_3000_Desc", title, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCQuest _quest in MainWindow.CurrentProject.data.quests)
            {
                if (_quest.ID == 0)
                {
                    yield return new NE_3000(_quest.Title, _quest.ID);
                }
            }
        }
    }
}
