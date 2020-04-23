using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quest
{
    /// <summary>
    /// No conditions
    /// </summary>
    public class NE_3001 : QuestMistake
    {
        public NE_3001() : base()
        {
            MistakeName = "NE_3001";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_3001(string title, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_3001_Desc", title, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCQuest _quest in MainWindow.CurrentProject.data.quests)
            {
                if (_quest.conditions.Count == 0)
                {
                    yield return new NE_3001(_quest.title, _quest.id);
                }
            }
        }
    }
}
