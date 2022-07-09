using BowieD.Unturned.NPCMaker.Localization;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    public class NE_1010 : DialogueMistake
    {
        public NE_1010()
        {
            MistakeName = "NE_1010";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_1010(NPC.NPCMessage message, int messageIndex, ushort id, NPC.Condition_Type conditionType) : this()
        {
            var localizedTypeName = LocalizationManager.Current.Condition[$"Type_{conditionType}"];

            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1010_Desc_Msg", messageIndex + 1, id, localizedTypeName);
        }
        public NE_1010(NPC.NPCResponse response, int responseIndex, ushort id, NPC.Condition_Type conditionType) : this()
        {
            var localizedTypeName = LocalizationManager.Current.Condition[$"Type_{conditionType}"];

            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1010_Desc_Rsp", responseIndex + 1, id, localizedTypeName);
        }

        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (var dialogue in MainWindow.CurrentProject.data.dialogues)
            {
                for (int i = 0; i < dialogue.Messages.Count; i++)
                {
                    NPC.NPCMessage message = dialogue.Messages[i];

                    foreach (var condition in message.conditions)
                    {
                        if (ConditionChecker.IsAllowed<NPC.NPCMessage>(condition.Type))
                            continue;

                        yield return new NE_1010(message, i, dialogue.ID, condition.Type);
                    }
                }

                for (int i = 0; i < dialogue.Responses.Count; i++)
                {
                    NPC.NPCResponse response = dialogue.Responses[i];

                    foreach (var condition in response.conditions)
                    {
                        if (ConditionChecker.IsAllowed<NPC.NPCResponse>(condition.Type))
                            continue;

                        yield return new NE_1010(response, i, dialogue.ID, condition.Type);
                    }
                }
            }
        }
    }
}
