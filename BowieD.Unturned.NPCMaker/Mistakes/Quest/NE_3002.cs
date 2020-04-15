using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quest
{
    /// <summary>
    /// No rewards
    /// </summary>
    public class NE_3002 : QuestMistake
    {
        public NE_3002() : base()
        {
            MistakeName = "NE_3002";
            Importance = IMPORTANCE.ADVICE;
        }
        public NE_3002(string title, ushort id) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_3002_Desc", title, id);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var _quest in MainWindow.CurrentProject.data.quests)
            {
                if (_quest.rewards.Count == 0)
                {
                    yield return new NE_3002(_quest.title, _quest.id);
                }
            }
        }
    }
}
