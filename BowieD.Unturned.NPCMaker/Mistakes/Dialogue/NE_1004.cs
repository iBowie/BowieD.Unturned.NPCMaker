using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// Preview quest and no condition or reward
    /// </summary>
    public sealed class NE_1004 : DialogueMistake
    {
        public NE_1004() : base()
        {
            MistakeName = "NE_1004";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_1004(int responseId, ushort dialogueId) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1004_Desc", responseId, dialogueId);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCDialogue dial in MainWindow.CurrentProject.data.dialogues)
            {
                for (int i = 0; i < dial.Responses.Count; i++)
                {
                    NPC.NPCResponse response = dial.Responses[i];

                    if (response.openQuestId > 0)
                    {
                        ConditionQuest questCondition = null;
                        RewardQuest questReward = null;

                        foreach (Condition condition in response.conditions)
                        {
                            if (condition is ConditionQuest quest)
                            {
                                if (quest.ID == response.openQuestId)
                                {
                                    questCondition = quest;
                                    break;
                                }
                            }
                        }

                        foreach (Reward reward in response.rewards)
                        {
                            if (reward is RewardQuest quest)
                            {
                                if (quest.ID == response.openQuestId)
                                {
                                    questReward = quest;
                                    break;
                                }
                            }
                        }

                        if (questCondition == null && questReward == null) // no quest condition yet previewing quest
                        {
                            yield return new NE_1004(i + 1, dial.ID);
                        }
                    }
                }
            }
        }
    }
}
