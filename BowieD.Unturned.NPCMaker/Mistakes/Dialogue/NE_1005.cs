using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    /// <summary>
    /// Does not have valid quest reward
    /// </summary>
    public sealed class NE_1005 : DialogueMistake
    {
        public NE_1005() : base()
        {
            MistakeName = "NE_1005";
            Importance = IMPORTANCE.WARNING;
        }
        public NE_1005(int responseId, ushort dialogueId) : this()
        {
            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("NE_1005_Desc", responseId, dialogueId);
        }
        public override IEnumerable<Mistake> CheckMistake()
        {
            foreach (NPC.NPCDialogue dial in MainWindow.CurrentProject.data.dialogues)
            {
                for (int i = 0; i < dial.responses.Count; i++)
                {
                    NPC.NPCResponse response = dial.responses[i];

                    if (response.openQuestId > 0)
                    {
                        ConditionQuest questCondition = null;

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

                        if (questCondition != null && questCondition.Status == NPC.Quest_Status.None) // has quest condition
                        {
                            RewardQuest rewardQuest = null;

                            foreach (Reward reward in response.rewards)
                            {
                                if (reward is RewardQuest rq)
                                {
                                    if (rq.ID == questCondition.ID)
                                    {
                                        rewardQuest = rq;
                                        break;
                                    }
                                }
                            }

                            if (rewardQuest == null)
                            {
                                yield return new NE_1004(i + 1, dial.id);
                            }
                        }
                    }
                }
            }
        }
    }
}
