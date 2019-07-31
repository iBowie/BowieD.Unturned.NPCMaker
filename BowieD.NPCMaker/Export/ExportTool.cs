using BowieD.NPCMaker.Configuration;
using BowieD.NPCMaker.NPC;
using System.IO;
using BowieD.NPCMaker.Extensions;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using BowieD.NPCMaker.NPC.Condition;
using System.Reflection;
using BowieD.NPCMaker.NPC.Condition.Attributes;
using BowieD.NPCMaker.NPC.Reward;
using BowieD.NPCMaker.NPC.Reward.Attributes;

namespace BowieD.NPCMaker.Export
{
    public static class ExportTool
    {
        private const string WaterText = "// Made in NPC Maker 2.0";
        public static bool ExportCharacter(Character character, string directory)
        {
            try
            {
                string workDir = $"{directory}Characters{Path.DirectorySeparatorChar}{character.editorName}_{character.id}{Path.DirectorySeparatorChar}";
                Directory.CreateDirectory(workDir);
                using (StreamWriter assetWriter = new StreamWriter(workDir + "Asset.dat", false, Encoding.UTF8))
                {
                    assetWriter.WriteLine(WaterText);
                    if (AppConfig.Instance.exportGuid)
                        assetWriter.WriteLine($"GUID {character.guid}");
                    assetWriter.WriteLine($"ID {character.id}");
                    assetWriter.WriteLine($"Type NPC");
                    if (character.defaultClothing.shirt > 0)
                        assetWriter.WriteLine($"Shirt {character.defaultClothing.shirt}");
                    if (character.defaultClothing.pants > 0)
                        assetWriter.WriteLine($"Pants {character.defaultClothing.pants}");
                    if (character.defaultClothing.mask > 0)
                        assetWriter.WriteLine($"Mask {character.defaultClothing.mask}");
                    if (character.defaultClothing.vest > 0)
                        assetWriter.WriteLine($"Vest {character.defaultClothing.vest}");
                    if (character.defaultClothing.backpack > 0)
                        assetWriter.WriteLine($"Backpack {character.defaultClothing.backpack}");
                    if (character.defaultClothing.hat > 0)
                        assetWriter.WriteLine($"Hat {character.defaultClothing.hat}");
                    if (character.defaultClothing.glasses > 0)
                        assetWriter.WriteLine($"Glasses {character.defaultClothing.glasses}");

                    if (character.christmasClothing.shirt > 0)
                        assetWriter.WriteLine($"Christmas_Shirt {character.christmasClothing.shirt}");
                    if (character.christmasClothing.pants > 0)
                        assetWriter.WriteLine($"Christmas_Pants {character.christmasClothing.pants}");
                    if (character.christmasClothing.mask > 0)
                        assetWriter.WriteLine($"Christmas_Mask {character.christmasClothing.mask}");
                    if (character.christmasClothing.vest > 0)
                        assetWriter.WriteLine($"Christmas_Vest {character.christmasClothing.vest}");
                    if (character.christmasClothing.backpack > 0)
                        assetWriter.WriteLine($"Christmas_Backpack {character.christmasClothing.backpack}");
                    if (character.christmasClothing.hat > 0)
                        assetWriter.WriteLine($"Christmas_Hat {character.christmasClothing.hat}");
                    if (character.christmasClothing.glasses > 0)
                        assetWriter.WriteLine($"Christmas_Glasses {character.christmasClothing.glasses}");

                    if (character.halloweenClothing.shirt > 0)
                        assetWriter.WriteLine($"Halloween_Shirt {character.halloweenClothing.shirt}");
                    if (character.halloweenClothing.pants > 0)
                        assetWriter.WriteLine($"Halloween_Pants {character.halloweenClothing.pants}");
                    if (character.halloweenClothing.mask > 0)
                        assetWriter.WriteLine($"Halloween_Mask {character.halloweenClothing.mask}");
                    if (character.halloweenClothing.vest > 0)
                        assetWriter.WriteLine($"Halloween_Vest {character.halloweenClothing.vest}");
                    if (character.halloweenClothing.backpack > 0)
                        assetWriter.WriteLine($"Halloween_Backpack {character.halloweenClothing.backpack}");
                    if (character.halloweenClothing.hat > 0)
                        assetWriter.WriteLine($"Halloween_Hat {character.halloweenClothing.hat}");
                    if (character.halloweenClothing.glasses > 0)
                        assetWriter.WriteLine($"Halloween_Glasses {character.halloweenClothing.glasses}");

                    if (character.equipPrimary > 0)
                        assetWriter.WriteLine($"Primary {character.equipPrimary}");
                    if (character.equipSecondary > 0)
                        assetWriter.WriteLine($"Secondary {character.equipSecondary}");
                    if (character.equipTertiary > 0)
                        assetWriter.WriteLine($"Tertiary {character.equipTertiary}");
                    if (character.equippedSlot != ESlotType.NONE)
                        assetWriter.WriteLine($"Equipped {character.equippedSlot.ToString().FirstCharToUpper()}");
                    assetWriter.WriteLine($"Face {character.face}");
                    assetWriter.WriteLine($"Beard {character.beard}");
                    assetWriter.WriteLine($"Hair {character.hair}");
                    assetWriter.WriteLine($"Color_Skin {character.skinColor.ToHEX()}");
                    assetWriter.WriteLine($"Color_Hair {character.hairColor.ToHEX()}");
                    assetWriter.WriteLine($"Pose {character.pose.ToString()}");
                    if (character.leftHanded)
                        assetWriter.WriteLine($"Backward");
                    if (character.dialogueId > 0)
                        assetWriter.WriteLine($"Dialogue {character.dialogueId}");
                    foreach (var k in Enum.GetValues(typeof(ELanguage)).ToEnumerable<ELanguage>())
                    {
                        if (character.editorName.ContainsKey(k) || character.displayName.ContainsKey(k))
                        {
                            using (StreamWriter localWriter = new StreamWriter(workDir + k + ".dat", false, Encoding.UTF8))
                            {
                                localWriter.WriteLine(WaterText);
                                if (character.editorName.ContainsKey(k))
                                    localWriter.WriteLine($"Name {character.editorName[k]}");
                                else
                                    localWriter.WriteLine($"Name UNDEFINED");
                                if (character.displayName.ContainsKey(k))
                                    localWriter.WriteLine($"Character {character.displayName[k]}");
                                else
                                    localWriter.WriteLine($"Character UNDEFINED");
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool ExportDialogue(Dialogue dialogue, string directory)
        {
            try
            {
                string workDir = directory = $"Dialogues{Path.DirectorySeparatorChar}{dialogue.guid}_{dialogue.id}{Path.DirectorySeparatorChar}";
                Directory.CreateDirectory(workDir);
                using (StreamWriter asset = new StreamWriter(workDir + "Asset.dat", false, Encoding.UTF8))
                {
                    asset.WriteLine(WaterText);
                    if (AppConfig.Instance.exportGuid)
                        asset.WriteLine($"GUID {dialogue.guid}");
                    asset.WriteLine($"Type Dialogue");
                    asset.WriteLine($"ID {dialogue.id}");
                    if (dialogue.messages.Count > 0)
                    {
                        asset.WriteLine($"Messages {dialogue.messages.Count}");
                        for (int k = 0; k < dialogue.messages.Count; k++)
                        {
                            Message msg = dialogue.messages[k];
                            if (msg.pages.Count > 0)
                            {
                                asset.WriteLine($"Message_{k}_Pages {msg.pages.Count}");
                            }
                            List<Response> responses = dialogue.responses.Where(d => d.visibleMessages.Contains(k)).ToList();
                            if (responses.Count > 0 && responses.Count < dialogue.responses.Count)
                            {
                                asset.WriteLine($"Message_{k}_Responses {responses.Count}");
                                for (int c = 0; c < responses.Count; c++)
                                {
                                    Response response = responses[c];
                                    int id = dialogue.responses.IndexOf(response);
                                    asset.WriteLine($"Message_{k}_Response_{c} {id}");
                                }
                            }
                            if (msg.conditions.Count > 0)
                            {
                                asset.WriteLine($"Message_{k}_Conditions {msg.conditions.Count}");
                                for (int c = 0; c < msg.conditions.Count; c++)
                                {
                                    asset.WriteLine(ExportCondition(msg.conditions[c], $"Message_{k}_", c));
                                }
                            }
                        }
                    }
                    if (dialogue.responses.Count > 0)
                    {
                        asset.WriteLine($"Responses {dialogue.responses.Count}");
                        for (int k = 0; k < dialogue.responses.Count; k++)
                        {
                            Response response = dialogue.responses[k];
                            if (response.visibleMessages.Count < dialogue.messages.Count)
                            {
                                asset.WriteLine($"Response_{k}_Messages {response.visibleMessages.Count}");
                                for (int c = 0, ind = 0; c < dialogue.messages.Count; c++)
                                {
                                    if (response.visibleMessages.Contains(c))
                                    {
                                        asset.WriteLine($"Response_{k}_Message_{ind++} {c}");
                                    }
                                }
                            }
                            if (response.dialogueId > 0)
                                asset.WriteLine($"Response_{k}_Dialogue {response.dialogueId}");
                            if (response.questId > 0)
                                asset.WriteLine($"Response_{k}_Quest {response.questId}");
                            if (response.vendorId > 0)
                                asset.WriteLine($"Response_{k}_Vendor {response.vendorId}");
                            if (response.conditions.Count > 0)
                            {
                                asset.WriteLine($"Response_{k}_Conditions {response.conditions.Count}");
                                for (int c = 0; c < response.conditions.Count; c++)
                                {
                                    asset.WriteLine(ExportCondition(response.conditions[c], $"Response_{k}_", c));
                                }
                            }
                            if (response.rewards.Count > 0)
                            {
                                asset.WriteLine($"Response_{k}_Rewards {response.rewards.Count}");
                                for (int c = 0; c < response.rewards.Count; c++)
                                {
                                    asset.WriteLine(ExportReward(response.rewards[c], $"Response_{k}_", c));
                                }
                            }
                        }
                    }
                    foreach (var k in Enum.GetValues(typeof(ELanguage)).ToEnumerable<ELanguage>())
                    {
                        if (dialogue.messages.Any(d => d.pages.Any(m => m.ContainsKey(k))) ||
                            dialogue.responses.Any(d => d.text.ContainsKey(k)))
                        {
                            using (StreamWriter localWriter = new StreamWriter(workDir + k + ".dat", false, Encoding.UTF8))
                            {
                                localWriter.WriteLine(WaterText);
                                // messages
                                for (int messageId = 0; messageId < dialogue.messages.Count; messageId++)
                                {
                                    for (int pageId = 0; pageId < dialogue.messages[messageId].pages.Count; pageId++)
                                    {
                                        localWriter.Write($"Message_{messageId}_Page_{pageId} ");
                                        if (dialogue.messages[messageId].pages[pageId].ContainsKey(k))
                                            localWriter.WriteLine($"{dialogue.messages[messageId].pages[pageId][k]}");
                                        else
                                            localWriter.WriteLine($"UNDEFINED");
                                    }
                                }
                                // responses
                                for (int responseId = 0; responseId < dialogue.responses.Count; responseId++)
                                {
                                    localWriter.Write($"Response_{responseId} ");
                                    if (dialogue.responses[responseId].text.ContainsKey(k))
                                        localWriter.WriteLine($"{dialogue.responses[responseId].text[k]}");
                                    else
                                        localWriter.WriteLine($"UNDEFINED");
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static string ExportCondition(Condition condition, string prefix, int conditionIndex)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"{prefix}Condition_{conditionIndex}_Type {condition.ConditionType}");
            foreach (var field in condition.GetType().GetFields())
            {
                var conditionAttribute = field.GetCustomAttribute<ConditionFieldAttribute>();
                if (conditionAttribute == null)
                    continue;
                string conditionFieldName = conditionAttribute.NameOnExport;
                var flagAttribute = field.GetCustomAttribute<ConditionFlagAttribute>();
                if (field.FieldType == typeof(Boolean) && flagAttribute != null)
                {
                    result.AppendLine($"{prefix}Condition_{conditionIndex}_{conditionFieldName}");
                }
                else
                {
                    var fieldValue = field.GetValue(condition);
                    result.AppendLine($"{prefix}Condition_{conditionIndex}_{conditionFieldName} {fieldValue.ToString().FirstCharToUpper()}");
                }
            }
            return result.ToString();
        }
        private static string ExportReward(Reward reward, string prefix, int rewardIndex)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"{prefix}Reward_{rewardIndex}_Type {reward.RewardType}");
            foreach (var field in reward.GetType().GetFields())
            {
                var rewardAttribute = field.GetCustomAttribute<RewardFieldAttribute>();
                if (rewardAttribute == null)
                    continue;
                string rewardFieldName = rewardAttribute.NameOnExport;
                var fieldValue = field.GetValue(reward);
                result.AppendLine($"{prefix}Reward_{rewardIndex}_{rewardFieldName} {fieldValue.ToString().FirstCharToUpper()}");
            }
            return result.ToString();
        }
    }
}
