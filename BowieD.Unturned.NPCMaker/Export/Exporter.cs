using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Export
{
    public static class Exporter
    {
        private const string WaterText = "// Made in NPC Maker by BowieD";
        private const string NoValue = "UNDEFINED";
        public static void ExportNPC(NPCProject save)
        {
            try
            {
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $@"\results\{save.guid}"))
                    Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + $@"\results\{save.guid}", true);
                dir = AppDomain.CurrentDomain.BaseDirectory + $@"\results\{save.guid}\";
                Export_Characters(save.characters);
                Export_Dialogues(save.dialogues);
                Export_Quests(save.quests);
                Export_Vendors(save.vendors);
                Button button = new Button
                {
                    Content = new TextBlock
                    {
                        Text = LocalizationManager.Current.Notification["Export_Done_Goto"]
                    }
                };
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $@"results\{save.guid}"))
                {
                    Action<object, RoutedEventArgs> action = new Action<object, RoutedEventArgs>((sender, e) => { Process.Start(AppDomain.CurrentDomain.BaseDirectory + $@"results\{save.guid}"); });
                    button.Click += new RoutedEventHandler(action);
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification["Export_Done"], buttons: button);
                }
            }
            catch (Exception ex)
            {
                App.Logger.LogException("Unable to export NPC.", ex);
                switch (ex)
                {
                    case IOException iOException:
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["Export_Error_IOException"]);
                        break;
                    case UnauthorizedAccessException unauthorizedAccessException:
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["Export_Error_UnauthorizedAccessException"]);
                        break;
                    default:
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["Export_Error"]);
                        break;
                }
            }
        }
        private static string dir = "";

        private static void Export_Characters(IEnumerable<NPCCharacter> characters)
        {
            foreach (NPCCharacter character in characters)
            {
                try
                {
                    Directory.CreateDirectory(dir + $@"Characters\{character.editorName}_{character.id}");
                    using (StreamWriter asset = new StreamWriter(dir + $@"Characters\{character.editorName}_{character.id}\Asset.dat", false, Encoding.UTF8))
                    using (StreamWriter local = new StreamWriter(dir + $@"Characters\{character.editorName}_{character.id}\English.dat", false, Encoding.UTF8))
                    {
                        asset.WriteLine(WaterText);
                        local.WriteLine(WaterText);
                        if (AppConfig.Instance.generateGuids)
                            asset.WriteLine($"GUID {character.guid}");
                        asset.WriteLine($"ID {character.id}");
                        asset.WriteLine($"Type NPC");
                        if (character.clothing.Shirt > 0)
                            asset.WriteLine($"Shirt {character.clothing.Shirt}");
                        if (character.clothing.Pants > 0)
                            asset.WriteLine($"Pants {character.clothing.Pants}");
                        if (character.clothing.Mask > 0)
                            asset.WriteLine($"Mask {character.clothing.Mask}");
                        if (character.clothing.Vest > 0)
                            asset.WriteLine($"Vest {character.clothing.Vest}");
                        if (character.clothing.Backpack > 0)
                            asset.WriteLine($"Backpack {character.clothing.Backpack}");
                        if (character.clothing.Hat > 0)
                            asset.WriteLine($"Hat {character.clothing.Hat}");
                        if (character.clothing.Glasses > 0)
                            asset.WriteLine($"Glasses {character.clothing.Glasses}");

                        if (!character.christmasClothing.IsEmpty)
                        {
                            asset.WriteLine("Has_Christmas_Outfit True");
                            if (character.christmasClothing.Shirt > 0)
                                asset.WriteLine($"Christmas_Shirt {character.christmasClothing.Shirt}");
                            if (character.christmasClothing.Pants > 0)
                                asset.WriteLine($"Christmas_Pants {character.christmasClothing.Pants}");
                            if (character.christmasClothing.Mask > 0)
                                asset.WriteLine($"Christmas_Mask {character.christmasClothing.Mask}");
                            if (character.christmasClothing.Vest > 0)
                                asset.WriteLine($"Christmas_Vest {character.christmasClothing.Vest}");
                            if (character.christmasClothing.Backpack > 0)
                                asset.WriteLine($"Christmas_Backpack {character.christmasClothing.Backpack}");
                            if (character.christmasClothing.Hat > 0)
                                asset.WriteLine($"Christmas_Hat {character.christmasClothing.Hat}");
                            if (character.christmasClothing.Glasses > 0)
                                asset.WriteLine($"Christmas_Glasses {character.christmasClothing.Glasses}");
                        }

                        if (!character.halloweenClothing.IsEmpty)
                        {
                            asset.WriteLine("Has_Halloween_Outfit True");
                            if (character.halloweenClothing.Shirt > 0)
                                asset.WriteLine($"Halloween_Shirt {character.halloweenClothing.Shirt}");
                            if (character.halloweenClothing.Pants > 0)
                                asset.WriteLine($"Halloween_Pants {character.halloweenClothing.Pants}");
                            if (character.halloweenClothing.Mask > 0)
                                asset.WriteLine($"Halloween_Mask {character.halloweenClothing.Mask}");
                            if (character.halloweenClothing.Vest > 0)
                                asset.WriteLine($"Halloween_Vest {character.halloweenClothing.Vest}");
                            if (character.halloweenClothing.Backpack > 0)
                                asset.WriteLine($"Halloween_Backpack {character.halloweenClothing.Backpack}");
                            if (character.halloweenClothing.Hat > 0)
                                asset.WriteLine($"Halloween_Hat {character.halloweenClothing.Hat}");
                            if (character.halloweenClothing.Glasses > 0)
                                asset.WriteLine($"Halloween_Glasses {character.halloweenClothing.Glasses}");
                        }

                        if (character.equipPrimary > 0)
                            asset.WriteLine($"Primary {character.equipPrimary}");
                        if (character.equipSecondary > 0)
                            asset.WriteLine($"Secondary {character.equipSecondary}");
                        if (character.equipTertiary > 0)
                            asset.WriteLine($"Tertiary {character.equipTertiary}");
                        if (character.equipped != Equip_Type.None)
                            asset.WriteLine($"Equipped {character.equipped.ToString()}");
                        asset.WriteLine($"Face {character.face}");
                        asset.WriteLine($"Beard {character.beard}");
                        asset.WriteLine($"Hair {character.haircut}");
                        asset.WriteLine($"Color_Skin {character.skinColor.ToHEX()}");
                        asset.WriteLine($"Color_Hair {character.hairColor.ToHEX()}");
                        asset.WriteLine($"Pose {character.pose.ToString()}");
                        if (character.leftHanded)
                            asset.WriteLine($"Backward");
                        if (character.startDialogueId > 0)
                            asset.WriteLine($"Dialogue {character.startDialogueId}");

                        if (character.visibilityConditions?.Count() > 0)
                        {
                            int condCount = character.visibilityConditions.Count();
                            asset.WriteLine($"Conditions {condCount}");
                            for (int k = 0; k < condCount; k++)
                            {
                                var cond = character.visibilityConditions.ElementAt(k);
                                asset.WriteLine(ExportCondition(cond, "", k, true));
                            }
                        }

                        local.WriteLine($"Name {character.editorName}");
                        local.WriteLine($"Character {character.displayName}");
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"Can't export character {character.id}", ex);
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Export_Character_Error", character.id));
                }
            }
        }
        private static void Export_Dialogues(IEnumerable<NPCDialogue> dialogues)
        {
            foreach (NPCDialogue dialogue in dialogues)
            {
                try
                {
                    Directory.CreateDirectory(dir + $@"Dialogues\{dialogue.guid}_{dialogue.id}");
                    using (StreamWriter asset = new StreamWriter(dir + $@"Dialogues\{dialogue.guid}_{dialogue.id}\Asset.dat", false, Encoding.UTF8))
                    using (StreamWriter local = new StreamWriter(dir + $@"Dialogues\{dialogue.guid}_{dialogue.id}\English.dat", false, Encoding.UTF8))
                    {
                        asset.WriteLine(WaterText);
                        local.WriteLine(WaterText);
                        if (AppConfig.Instance.generateGuids)
                            asset.WriteLine($"GUID {dialogue.guid}");
                        asset.WriteLine($"Type Dialogue");
                        asset.WriteLine($"ID {dialogue.id}");

                        if (dialogue.MessagesAmount > 0)
                        {
                            asset.WriteLine($"Messages {dialogue.MessagesAmount}");
                            for (int k = 0; k < dialogue.MessagesAmount; k++)
                            {
                                NPCMessage message = dialogue.messages[k];
                                if (message.PagesAmount > 0)
                                {
                                    asset.WriteLine($"Message_{k}_Pages {message.PagesAmount}");
                                }
                                List<NPCResponse> visibleResponses = dialogue.responses.Where(d => d.VisibleInAll || d.visibleIn.Length <= k || d.visibleIn[k] == 1).ToList();
                                if (visibleResponses.Count() > 0 && visibleResponses.Count() < dialogue.responses.Count())
                                {
                                    asset.WriteLine($"Message_{k}_Responses {visibleResponses.Count()}");
                                    int visResCnt = visibleResponses.Count();
                                    for (int c = 0; c < visResCnt; c++)
                                    {
                                        NPCResponse response = visibleResponses[c];
                                        int id = dialogue.responses.IndexOf(response);
                                        asset.WriteLine($"Message_{k}_Response_{c} {id}");
                                    }
                                }
                                if (message.conditions.Count() > 0)
                                {
                                    asset.WriteLine($"Message_{k}_Conditions {message.conditions.Count()}");
                                    int msgCnt = message.conditions.Count();
                                    for (int c = 0; c < msgCnt; c++)
                                    {
                                        asset.WriteLine(ExportCondition(message.conditions[c], $"Message_{k}_", c));
                                    }
                                }
                            }
                        }
                        if (dialogue.ResponsesAmount > 0)
                        {
                            asset.WriteLine($"Responses {dialogue.ResponsesAmount}");
                            for (int k = 0; k < dialogue.ResponsesAmount; k++)
                            {
                                NPCResponse response = dialogue.responses[k];
                                if (!response.VisibleInAll)
                                {
                                    asset.WriteLine($"Response_{k}_Messages {response.visibleIn.Count(d => d == 1)}");
                                    for (int c = 0, ind = 0; c < dialogue.MessagesAmount; c++)
                                    {
                                        var currentMessage = dialogue.messages[c];
                                        if (response.visibleIn.Length <= c || response.visibleIn[c] == 1)
                                        {
                                            asset.WriteLine($"Response_{k}_Message_{ind++} {dialogue.messages.IndexOf(currentMessage)}");
                                        }
                                    }
                                }
                                if (response.openDialogueId > 0)
                                    asset.WriteLine($"Response_{k}_Dialogue {response.openDialogueId}");
                                if (response.openQuestId > 0)
                                    asset.WriteLine($"Response_{k}_Quest {response.openQuestId}");
                                if (response.openVendorId > 0)
                                    asset.WriteLine($"Response_{k}_Vendor {response.openVendorId}");
                                if (response.conditions.Count() > 0)
                                {
                                    asset.WriteLine($"Response_{k}_Conditions {response.conditions.Count()}");
                                    int cndCnt = response.conditions.Count();
                                    for (int c = 0; c < cndCnt; c++)
                                    {
                                        asset.WriteLine(ExportCondition(response.conditions[c], $"Response_{k}_", c));
                                    }
                                }
                                if (response.rewards.Count() > 0)
                                {
                                    asset.WriteLine($"Response_{k}_Rewards {response.rewards.Count()}");
                                    int rwrdCnt = response.rewards.Count();
                                    for (int c = 0; c < rwrdCnt; c++)
                                    {
                                        asset.WriteLine(ExportReward(response.rewards[c], $"Response_{k}_", c));
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < dialogue.MessagesAmount; k++)
                        {
                            for (int c = 0; c < dialogue.messages[k].PagesAmount; c++)
                            {
                                local.WriteLine($"Message_{k}_Page_{c} {dialogue.messages[k].pages[c]}");
                            }
                        }
                        for (int k = 0; k < dialogue.ResponsesAmount; k++)
                        {
                            local.WriteLine($"Response_{k} {dialogue.responses[k].mainText}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"Can't export dialogue {dialogue.id}", ex);
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Export_Dialogue_Error", dialogue.id));
                }
            }
        }
        private static void Export_Vendors(IEnumerable<NPCVendor> vendors)
        {
            foreach (NPCVendor vendor in vendors)
            {
                try
                {
                    Directory.CreateDirectory(dir + $@"Vendors\{vendor.guid}_{vendor.id}");
                    using (StreamWriter asset = new StreamWriter(dir + $@"Vendors\{vendor.guid}_{vendor.id}\Asset.dat", false, Encoding.UTF8))
                    using (StreamWriter local = new StreamWriter(dir + $@"Vendors\{vendor.guid}_{vendor.id}\English.dat", false, Encoding.UTF8))
                    {
                        asset.WriteLine(WaterText);
                        local.WriteLine(WaterText);
                        if (AppConfig.Instance.generateGuids)
                            asset.WriteLine($"GUID {vendor.guid}");
                        asset.WriteLine($"Type Vendor");
                        asset.WriteLine($"ID {vendor.id}");

                        if (vendor.BuyItems.Count > 0)
                        {
                            VendorItem[] buy = vendor.BuyItems.ToArray();
                            asset.WriteLine($"Buying {buy.Length}");
                            for (int k = 0; k < buy.Length; k++)
                            {
                                asset.WriteLine($"Buying_{k}_ID {buy[k].id}");
                                asset.WriteLine($"Buying_{k}_Cost {buy[k].cost}");
                                if (buy[k].conditions?.Count > 0)
                                {
                                    asset.WriteLine($"Buying_{k}_Conditions {buy[k].conditions.Count}");
                                    for (int c = 0; c < buy[k].conditions.Count; c++)
                                    {
                                        asset.WriteLine(ExportCondition(buy[k].conditions[c], $"Buying_{k}_", c));
                                    }
                                }
                            }
                        }
                        if (vendor.SellItems.Count > 0)
                        {
                            VendorItem[] sell = vendor.SellItems.ToArray();
                            asset.WriteLine($"Selling {sell.Length}");
                            for (int k = 0; k < sell.Length; k++)
                            {
                                if (sell[k].type == ItemType.VEHICLE)
                                {
                                    asset.WriteLine($"Selling_{k}_Type Vehicle");
                                    asset.WriteLine($"Selling_{k}_Spawnpoint {sell[k].spawnPointID}");
                                }
                                asset.WriteLine($"Selling_{k}_ID {sell[k].id}");
                                asset.WriteLine($"Selling_{k}_Cost {sell[k].cost}");
                                if (sell[k].conditions?.Count > 0)
                                {
                                    asset.WriteLine($"Selling_{k}_Conditions {sell[k].conditions.Count}");
                                    for (int c = 0; c < sell[k].conditions.Count; c++)
                                    {
                                        asset.WriteLine(ExportCondition(sell[k].conditions[c], $"Selling_{k}_", c));
                                    }
                                }
                            }
                        }
                        if (vendor.disableSorting)
                            asset.WriteLine("Disable_Sorting");

                        local.WriteLine($"Name {vendor.vendorTitle}");
                        local.WriteLine($"Description {vendor.vendorDescription}");
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"Can't export vendor {vendor.id}", ex);
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Export_Vendor_Error", vendor.id));
                }
            }
        }
        private static void Export_Quests(IEnumerable<NPCQuest> quests)
        {
            foreach (NPCQuest quest in quests)
            {
                try
                {
                    Directory.CreateDirectory(dir + $@"Quests\{quest.guid}_{quest.id}");
                    using (StreamWriter asset = new StreamWriter(dir + $@"Quests\{quest.guid}_{quest.id}\Asset.dat", false, Encoding.UTF8))
                    using (StreamWriter local = new StreamWriter(dir + $@"Quests\{quest.guid}_{quest.id}\English.dat", false, Encoding.UTF8))
                    {
                        asset.WriteLine(WaterText);
                        local.WriteLine(WaterText);
                        if (AppConfig.Instance.generateGuids)
                            asset.WriteLine($"GUID {quest.guid}");
                        asset.WriteLine($"Type Quest");
                        asset.WriteLine($"ID {quest.id}");

                        if (quest.conditions?.Count > 0)
                        {
                            asset.WriteLine($"Conditions {quest.conditions.Count}");
                            for (int k = 0; k < quest.conditions.Count; k++)
                            {
                                asset.WriteLine(ExportCondition(quest.conditions[k], "", k));
                            }
                        }

                        if (quest.rewards?.Count > 0)
                        {
                            asset.WriteLine($"Rewards {quest.rewards.Count}");
                            for (int k = 0; k < quest.rewards.Count; k++)
                            {
                                asset.WriteLine(ExportReward(quest.rewards[k], "", k));
                            }
                        }

                        local.WriteLine($"Name {quest.title}");
                        local.WriteLine($"Description {quest.description}");
                        for (int k = 0; k < quest.conditions?.Count; k++)
                        {
                            if (quest.conditions[k].Localization.Length > 0)
                            {
                                local.WriteLine($"Condition_{k} {quest.conditions[k].Localization}");
                            }
                        }
                        for (int k = 0; k < quest.rewards?.Count; k++)
                        {
                            if (quest.rewards[k].Localization.Length > 0)
                            {
                                local.WriteLine($"Reward_{k} {quest.rewards[k].Localization}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"Can't export quest {quest.id}", ex);
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Export_Quest_Error", quest.id));
                }
            }
        }
        internal static string ExportCondition(NPC.Conditions.Condition condition, string prefix, int conditionIndex, bool skipLocalization = true)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"{prefix}Condition_{conditionIndex}_Type {condition.Type}");
            foreach (var prop in condition.GetType().GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;
                var propName = prop.Name;
                var skipPropA = prop.GetCustomAttribute<ConditionSkipFieldAttribute>();
                if ((skipLocalization && propName == "Localization") || skipPropA != null)
                    continue;
                var propValue = prop.GetValue(condition);
                var noValueA = prop.GetCustomAttribute<ConditionNoValueAttribute>();
                var optionalA = prop.GetCustomAttribute<ConditionOptionalAttribute>();
                if (skipPropA != null)
                    continue;
                if (noValueA != null)
                {
                    if (propValue.Equals(true))
                        propValue = "";
                    else
                        continue;
                }
                if (optionalA != null && optionalA.ConditionApplied(propValue))
                    continue;
                result.AppendLine($"{prefix}Condition_{conditionIndex}_{propName} {propValue}");
            }
            return result.ToString();
        }
        internal static string ExportReward(Reward reward, string prefix, int rewardIndex, bool skipLocalization = true)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"{prefix}Reward_{rewardIndex}_Type {reward.Type}");
            foreach (var prop in reward.GetType().GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;
                var propName = prop.Name;
                var skipPropA = prop.GetCustomAttribute<RewardSkipFieldAttribute>();
                if ((skipLocalization && propName == "Localization") || skipPropA != null)
                    continue;
                var propValue = prop.GetValue(reward);
                var noValueA = prop.GetCustomAttribute<RewardNoValueAttribute>();
                var optionalA = prop.GetCustomAttribute<RewardOptionalAttribute>();
                if (skipPropA != null)
                    continue;
                if (noValueA != null)
                {
                    if (propValue.Equals(true))
                        propValue = "";
                    else
                        continue;
                }
                if (optionalA != null && optionalA.ConditionApplied(propValue))
                    continue;
                result.AppendLine($"{prefix}Reward_{rewardIndex}_{propName} {propValue}");
            }
            return result.ToString();
        }
    }
}
