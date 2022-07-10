using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Character
{
    public class NE_0005 : CharacterMistake
    {
        public NE_0005()
        {
            MistakeName = "NE_0005";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_0005(string displayName, ushort id, NPC.Condition_Type conditionType) : this()
        {
            var localizedTypeName = LocalizationManager.Current.Condition[$"Type_{conditionType}"];

            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_0005_Desc", displayName, id, localizedTypeName);
        }

        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var character in MainWindow.CurrentProject.data.characters)
            {
                foreach (var condition in character.visibilityConditions)
                {
                    if (ConditionChecker.IsAllowed<NPC.NPCCharacter>(condition.Type))
                        continue;

                    yield return new NE_0005(character.DisplayName, character.ID, condition.Type);
                }
            }
        }
    }
}
