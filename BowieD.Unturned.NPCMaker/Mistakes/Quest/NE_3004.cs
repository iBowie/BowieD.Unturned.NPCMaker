using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quest
{
    public class NE_3004 : QuestMistake
    {
        public NE_3004()
        {
            MistakeName = "NE_3004";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_3004(string title, ushort id, NPC.Condition_Type conditionType) : this()
        {
            var localizedTypeName = LocalizationManager.Current.Condition[$"Type_{conditionType}"];

            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_3004_Desc", title, id, localizedTypeName);
        }

        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var quest in MainWindow.CurrentProject.data.quests)
            {
                foreach (var condition in quest.conditions)
                {
                    if (ConditionChecker.IsAllowed<NPC.NPCQuest>(condition.Type))
                        continue;

                    yield return new NE_3004(quest.Title, quest.ID, condition.Type);
                }
            }
        }
    }
}
