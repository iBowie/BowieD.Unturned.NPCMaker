using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Currency;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Export
{
    public static class Exporter
    {
        private const string WaterText = "// Made in NPC Maker by BowieD";

        public static void ExportNPC(NPCProject save, string directory, bool bundlify = false)
        {
            try
            {
                dir = Path.Combine(directory, $"{save.guid}");

                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                }

                if (bundlify)
                {
                    dir = Path.Combine(dir, "Bundles", "NPCs");
                }

                Export_Characters(save.characters);
                Export_Dialogues(save.dialogues);
                Export_Quests(save.quests);
                Export_Vendors(save.vendors);
                Export_Currencies(save.currencies);
                Button button = new Button
                {
                    Content = new TextBlock
                    {
                        Text = LocalizationManager.Current.Notification["Export_Done_Goto"]
                    }
                };
                if (Directory.Exists(dir))
                {
                    button.Click += (sender, e) =>
                    {
                        Process.Start(dir);
                    };
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification["Export_Done"], buttons: button);
                }
            }
            catch (Exception ex)
            {
                App.Logger.LogException("Unable to export NPC.", ex: ex);
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
                    string aPath = Path.Combine(dir, "Characters", GetCharacterFolderName(character));

                    if (!Directory.Exists(aPath))
                        Directory.CreateDirectory(aPath);

                    using (StreamWriter asset = new StreamWriter(Path.Combine(aPath, "Asset.dat"), false, Encoding.UTF8))
                    using (StreamWriter local = new StreamWriter(Path.Combine(aPath, "English.dat"), false, Encoding.UTF8))
                    {
                        asset.WriteLine(WaterText);
                        local.WriteLine(WaterText);
                        if (AppConfig.Instance.generateGuids)
                        {
                            asset.WriteLine($"GUID {character.GUID}");
                        }

                        asset.WriteLine($"ID {character.ID}");
                        asset.WriteLine($"Type NPC");
                        if (!character.clothing.Shirt.IsEmpty)
                        {
                            asset.WriteLine($"Shirt {character.clothing.Shirt.ExportValue}");
                        }

                        if (!character.clothing.Pants.IsEmpty)
                        {
                            asset.WriteLine($"Pants {character.clothing.Pants.ExportValue}");
                        }

                        if (!character.clothing.Mask.IsEmpty)
                        {
                            asset.WriteLine($"Mask {character.clothing.Mask.ExportValue}");
                        }

                        if (!character.clothing.Vest.IsEmpty)
                        {
                            asset.WriteLine($"Vest {character.clothing.Vest.ExportValue}");
                        }

                        if (!character.clothing.Backpack.IsEmpty)
                        {
                            asset.WriteLine($"Backpack {character.clothing.Backpack.ExportValue}");
                        }

                        if (!character.clothing.Hat.IsEmpty)
                        {
                            asset.WriteLine($"Hat {character.clothing.Hat.ExportValue}");
                        }

                        if (!character.clothing.Glasses.IsEmpty)
                        {
                            asset.WriteLine($"Glasses {character.clothing.Glasses.ExportValue}");
                        }

                        if (!character.christmasClothing.IsEmpty)
                        {
                            asset.WriteLine("Has_Christmas_Outfit True");
                            if (!character.christmasClothing.Shirt.IsEmpty)
                            {
                                asset.WriteLine($"Christmas_Shirt {character.christmasClothing.Shirt.ExportValue}");
                            }

                            if (!character.christmasClothing.Pants.IsEmpty)
                            {
                                asset.WriteLine($"Christmas_Pants {character.christmasClothing.Pants.ExportValue}");
                            }

                            if (!character.christmasClothing.Mask.IsEmpty)
                            {
                                asset.WriteLine($"Christmas_Mask {character.christmasClothing.Mask.ExportValue}");
                            }

                            if (!character.christmasClothing.Vest.IsEmpty)
                            {
                                asset.WriteLine($"Christmas_Vest {character.christmasClothing.Vest.ExportValue}");
                            }

                            if (!character.christmasClothing.Backpack.IsEmpty)
                            {
                                asset.WriteLine($"Christmas_Backpack {character.christmasClothing.Backpack.ExportValue}");
                            }

                            if (!character.christmasClothing.Hat.IsEmpty)
                            {
                                asset.WriteLine($"Christmas_Hat {character.christmasClothing.Hat.ExportValue}");
                            }

                            if (!character.christmasClothing.Glasses.IsEmpty)
                            {
                                asset.WriteLine($"Christmas_Glasses {character.christmasClothing.Glasses.ExportValue}");
                            }
                        }

                        if (!character.halloweenClothing.IsEmpty)
                        {
                            asset.WriteLine("Has_Halloween_Outfit True");
                            if (!character.halloweenClothing.Shirt.IsEmpty)
                            {
                                asset.WriteLine($"Halloween_Shirt {character.halloweenClothing.Shirt.ExportValue}");
                            }

                            if (!character.halloweenClothing.Pants.IsEmpty)
                            {
                                asset.WriteLine($"Halloween_Pants {character.halloweenClothing.Pants.ExportValue}");
                            }

                            if (!character.halloweenClothing.Mask.IsEmpty)
                            {
                                asset.WriteLine($"Halloween_Mask {character.halloweenClothing.Mask.ExportValue}");
                            }

                            if (!character.halloweenClothing.Vest.IsEmpty)
                            {
                                asset.WriteLine($"Halloween_Vest {character.halloweenClothing.Vest.ExportValue}");
                            }

                            if (!character.halloweenClothing.Backpack.IsEmpty)
                            {
                                asset.WriteLine($"Halloween_Backpack {character.halloweenClothing.Backpack.ExportValue}");
                            }

                            if (!character.halloweenClothing.Hat.IsEmpty)
                            {
                                asset.WriteLine($"Halloween_Hat {character.halloweenClothing.Hat.ExportValue}");
                            }

                            if (!character.halloweenClothing.Glasses.IsEmpty)
                            {
                                asset.WriteLine($"Halloween_Glasses {character.halloweenClothing.Glasses.ExportValue}");
                            }
                        }

                        if (!character.equipPrimary.IsEmpty)
                        {
                            asset.WriteLine($"Primary {character.equipPrimary.ExportValue}");
                        }

                        if (!character.equipSecondary.IsEmpty)
                        {
                            asset.WriteLine($"Secondary {character.equipSecondary.ExportValue}");
                        }

                        if (!character.equipTertiary.IsEmpty)
                        {
                            asset.WriteLine($"Tertiary {character.equipTertiary.ExportValue}");
                        }

                        if (character.equipped != Equip_Type.None)
                        {
                            asset.WriteLine($"Equipped {character.equipped}");
                        }

                        asset.WriteLine($"Face {character.face}");
                        asset.WriteLine($"Beard {character.beard}");
                        asset.WriteLine($"Hair {character.haircut}");
                        asset.WriteLine($"Color_Skin {character.skinColor.ToHEX()}");
                        asset.WriteLine($"Color_Hair {character.hairColor.ToHEX()}");
                        asset.WriteLine($"Pose {character.pose}");
                        if (character.leftHanded)
                        {
                            asset.WriteLine($"Backward");
                        }

                        if (character.startDialogueId > 0)
                        {
                            asset.WriteLine($"Dialogue {character.startDialogueId}");
                        }

                        if (character.visibilityConditions?.Count() > 0)
                        {
                            int condCount = character.visibilityConditions.Count();
                            asset.WriteLine($"Conditions {condCount}");
                            for (int k = 0; k < condCount; k++)
                            {
                                NPC.Conditions.Condition cond = character.visibilityConditions.ElementAt(k);

                                asset.WriteLine(ExportCondition(cond, "", k, true));

                                if (!string.IsNullOrEmpty(cond.Localization))
                                    local.WriteLine($"Condition_{k} {cond.Localization}");
                            }
                        }

                        if (!character.poseLean.ApproximateEquals(0))
                            asset.WriteLine($"Pose_Lean {character.poseLean.ToString(CultureInfo.InvariantCulture)}");
                        if (!character.posePitch.ApproximateEquals(90))
                            asset.WriteLine($"Pose_Pitch {character.posePitch.ToString(CultureInfo.InvariantCulture)}");
                        if (!character.poseHeadOffset.ApproximateEquals(0))
                            asset.WriteLine($"Pose_Head_Offset {character.poseHeadOffset.ToString(CultureInfo.InvariantCulture)}");

                        local.WriteLine($"Name {character.EditorName}");
                        local.WriteLine($"Character {character.DisplayName}");
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"Can't export character {character.ID}", ex: ex);
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Export_Character_Error", character.ID));
                }
            }
        }
        private static void Export_Dialogues(IEnumerable<NPCDialogue> dialogues)
        {
            foreach (NPCDialogue dialogue in dialogues)
            {
                try
                {
                    string aPath = Path.Combine(dir, "Dialogues", GetDialogueFolderName(dialogue));

                    if (!Directory.Exists(aPath))
                        Directory.CreateDirectory(aPath);

                    using (StreamWriter asset = new StreamWriter(Path.Combine(aPath, "Asset.dat"), false, Encoding.UTF8))
                    using (StreamWriter local = new StreamWriter(Path.Combine(aPath, "English.dat"), false, Encoding.UTF8))
                    {
                        asset.WriteLine(WaterText);
                        local.WriteLine(WaterText);
                        if (AppConfig.Instance.generateGuids)
                        {
                            asset.WriteLine($"GUID {dialogue.GUID}");
                        }

                        asset.WriteLine($"Type Dialogue");
                        asset.WriteLine($"ID {dialogue.ID}");

                        if (dialogue.Messages.Count > 0)
                        {
                            asset.WriteLine($"Messages {dialogue.Messages.Count}");
                            for (int k = 0; k < dialogue.Messages.Count; k++)
                            {
                                NPCMessage message = dialogue.Messages[k];
                                if (message.pages.Count > 0)
                                {
                                    asset.WriteLine($"Message_{k}_Pages {message.pages.Count}");
                                }
                                List<NPCResponse> visibleResponses = dialogue.Responses.Where(d => d.VisibleInAll || d.visibleIn.Length <= k || d.visibleIn[k] == 1).ToList();
                                if (visibleResponses.Count() > 0 && visibleResponses.Count() < dialogue.Responses.Count())
                                {
                                    asset.WriteLine($"Message_{k}_Responses {visibleResponses.Count()}");
                                    int visResCnt = visibleResponses.Count();
                                    for (int c = 0; c < visResCnt; c++)
                                    {
                                        NPCResponse response = visibleResponses[c];
                                        int id = dialogue.Responses.IndexOf(response);
                                        asset.WriteLine($"Message_{k}_Response_{c} {id}");
                                    }
                                }
                                if (message.conditions.Count > 0)
                                {
                                    asset.WriteLine($"Message_{k}_Conditions {message.conditions.Count}");
                                    for (int c = 0; c < message.conditions.Count; c++)
                                    {
                                        NPC.Conditions.Condition cond = message.conditions[c];

                                        asset.WriteLine(ExportCondition(cond, $"Message_{k}_", c));

                                        if (!string.IsNullOrEmpty(cond.Localization))
                                            local.WriteLine($"Message_{k}_Condition_{c} {cond.Localization}");
                                    }
                                }
                                if (message.rewards.Count > 0)
                                {
                                    asset.WriteLine($"Message_{k}_Rewards {message.rewards.Count}");
                                    for (int c = 0; c < message.rewards.Count; c++)
                                    {
                                        Reward rew = message.rewards[c];

                                        asset.WriteLine(ExportReward(rew, $"Message_{k}_", c));

                                        if (!string.IsNullOrEmpty(rew.Localization))
                                            local.WriteLine($"Message_{k}_Reward_{c} {rew.Localization}");
                                    }
                                }
                                if (message.prev > 0)
                                    asset.WriteLine($"Message_{k}_Prev {message.prev}");
                            }
                        }
                        if (dialogue.Responses.Count > 0)
                        {
                            asset.WriteLine($"Responses {dialogue.Responses.Count}");
                            for (int k = 0; k < dialogue.Responses.Count; k++)
                            {
                                NPCResponse response = dialogue.Responses[k];
                                if (!response.VisibleInAll)
                                {
                                    asset.WriteLine($"Response_{k}_Messages {response.visibleIn.Count(d => d == 1)}");
                                    for (int c = 0, ind = 0; c < dialogue.Messages.Count; c++)
                                    {
                                        NPCMessage currentMessage = dialogue.Messages[c];
                                        if (response.visibleIn.Length <= c || response.visibleIn[c] == 1)
                                        {
                                            asset.WriteLine($"Response_{k}_Message_{ind++} {dialogue.Messages.IndexOf(currentMessage)}");
                                        }
                                    }
                                }
                                if (response.openDialogueId > 0)
                                {
                                    asset.WriteLine($"Response_{k}_Dialogue {response.openDialogueId}");
                                }

                                if (response.openQuestId > 0)
                                {
                                    asset.WriteLine($"Response_{k}_Quest {response.openQuestId}");
                                }

                                if (response.openVendorId > 0)
                                {
                                    asset.WriteLine($"Response_{k}_Vendor {response.openVendorId}");
                                }

                                if (response.conditions.Count() > 0)
                                {
                                    asset.WriteLine($"Response_{k}_Conditions {response.conditions.Count()}");
                                    int cndCnt = response.conditions.Count();
                                    for (int c = 0; c < cndCnt; c++)
                                    {
                                        NPC.Conditions.Condition cond = response.conditions[c];

                                        asset.WriteLine(ExportCondition(cond, $"Response_{k}_", c));

                                        if (!string.IsNullOrEmpty(cond.Localization))
                                            local.WriteLine($"Response_{k}_Condition_{c} {cond.Localization}");
                                    }
                                }
                                if (response.rewards.Count() > 0)
                                {
                                    asset.WriteLine($"Response_{k}_Rewards {response.rewards.Count()}");
                                    int rwrdCnt = response.rewards.Count();
                                    for (int c = 0; c < rwrdCnt; c++)
                                    {
                                        Reward rew = response.rewards[c];

                                        asset.WriteLine(ExportReward(rew, $"Response_{k}_", c));

                                        if (!string.IsNullOrEmpty(rew.Localization))
                                            local.WriteLine($"Response_{k}_Reward_{c} {rew.Localization}");
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < dialogue.Messages.Count; k++)
                        {
                            for (int c = 0; c < dialogue.Messages[k].pages.Count; c++)
                            {
                                local.WriteLine($"Message_{k}_Page_{c} {dialogue.Messages[k].pages[c]}");
                            }
                        }
                        for (int k = 0; k < dialogue.Responses.Count; k++)
                        {
                            local.WriteLine($"Response_{k} {dialogue.Responses[k].mainText}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"Can't export dialogue {dialogue.ID}", ex: ex);
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Export_Dialogue_Error", dialogue.ID));
                }
            }
        }
        private static void Export_Vendors(IEnumerable<NPCVendor> vendors)
        {
            foreach (NPCVendor vendor in vendors)
            {
                try
                {
                    string aPath = Path.Combine(dir, "Vendors", GetVendorFolderName(vendor));

                    if (!Directory.Exists(aPath))
                        Directory.CreateDirectory(aPath);

                    using (StreamWriter asset = new StreamWriter(Path.Combine(aPath, "Asset.dat"), false, Encoding.UTF8))
                    using (StreamWriter local = new StreamWriter(Path.Combine(aPath, "English.dat"), false, Encoding.UTF8))
                    {
                        asset.WriteLine(WaterText);
                        local.WriteLine(WaterText);
                        if (AppConfig.Instance.generateGuids)
                        {
                            asset.WriteLine($"GUID {vendor.GUID}");
                        }

                        asset.WriteLine($"Type Vendor");
                        asset.WriteLine($"ID {vendor.ID}");

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

                                if (buy[k].rewards?.Count > 0)
                                {
                                    asset.WriteLine($"Buying_{k}_Rewards {buy[k].rewards.Count}");
                                    for (int c = 0; c < buy[k].rewards.Count; c++)
                                    {
                                        asset.WriteLine(ExportReward(buy[k].rewards[c], $"Buying_{k}_", c));
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

                                if (sell[k].rewards?.Count > 0)
                                {
                                    asset.WriteLine($"Selling_{k}_Rewards {sell[k].rewards.Count}");
                                    for (int c = 0; c < sell[k].rewards.Count; c++)
                                    {
                                        asset.WriteLine(ExportReward(sell[k].rewards[c], $"Selling_{k}_", c));
                                    }
                                }
                            }
                        }
                        if (vendor.disableSorting)
                        {
                            asset.WriteLine("Disable_Sorting");
                        }

                        if (vendor.currency?.Length > 0)
                        {
                            asset.WriteLine("Currency " + vendor.currency);
                        }

                        local.WriteLine($"Name {vendor.Title}");
                        local.WriteLine($"Description {vendor.vendorDescription}");
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"Can't export vendor {vendor.ID}", ex: ex);
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Export_Vendor_Error", vendor.ID));
                }
            }
        }
        private static void Export_Quests(IEnumerable<NPCQuest> quests)
        {
            foreach (NPCQuest quest in quests)
            {
                try
                {
                    string aPath = Path.Combine(dir, "Quests", GetQuestFolderName(quest));

                    if (!Directory.Exists(aPath))
                        Directory.CreateDirectory(aPath);

                    using (StreamWriter asset = new StreamWriter(Path.Combine(aPath, "Asset.dat"), false, Encoding.UTF8))
                    using (StreamWriter local = new StreamWriter(Path.Combine(aPath, "English.dat"), false, Encoding.UTF8))
                    {
                        asset.WriteLine(WaterText);
                        local.WriteLine(WaterText);
                        if (AppConfig.Instance.generateGuids)
                        {
                            asset.WriteLine($"GUID {quest.GUID}");
                        }

                        asset.WriteLine($"Type Quest");
                        asset.WriteLine($"ID {quest.ID}");

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

                        local.WriteLine($"Name {quest.Title}");
                        local.WriteLine($"Description {quest.description}");
                        for (int k = 0; k < quest.conditions?.Count; k++)
                        {
                            if (quest.conditions[k].Localization?.Length > 0)
                            {
                                local.WriteLine($"Condition_{k} {quest.conditions[k].Localization}");
                            }
                        }
                        for (int k = 0; k < quest.rewards?.Count; k++)
                        {
                            if (quest.rewards[k].Localization?.Length > 0)
                            {
                                local.WriteLine($"Reward_{k} {quest.rewards[k].Localization}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"Can't export quest {quest.ID}", ex: ex);
                    App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Export_Quest_Error", quest.ID));
                }
            }
        }
        private static void Export_Currencies(IEnumerable<CurrencyAsset> currencies)
        {
            foreach (var cur in currencies)
            {
                try
                {
                    string aPath = Path.Combine(dir, "Currencies");

                    if (!Directory.Exists(aPath))
                        Directory.CreateDirectory(aPath);

                    using (StreamWriter asset = new StreamWriter(Path.Combine(aPath, GetCurrencyFileName(cur)), false, Encoding.UTF8))
                    {
                        char q = '\"';

                        asset.WriteLine($"{q}Metadata{q}");
                        asset.WriteLine("{");
                        asset.WriteLine($"\t{q}GUID{q} {q}{cur.GUID}{q}");
                        asset.WriteLine($"\t{q}Type{q} {q}SDG.Unturned.ItemCurrencyAsset, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null{q}");
                        asset.WriteLine("}");
                        asset.WriteLine($"{q}Asset{q}");
                        asset.WriteLine("{");
                        asset.WriteLine($"\t{q}ValueFormat{q} {q}{cur.ValueFormat}{q}");
                        asset.WriteLine($"\t{q}Entries{q}");
                        asset.WriteLine($"\t[");

                        void writeEntry(CurrencyEntry entry)
                        {
                            asset.WriteLine("\t\t{");

                            asset.WriteLine($"\t\t\t{q}Item{q}");
                            asset.WriteLine("\t\t\t{");
                            asset.WriteLine($"\t\t\t\t{q}GUID{q} {q}{entry.ItemGUID}{q}");
                            asset.WriteLine("\t\t\t}");
                            asset.WriteLine($"\t\t\t{q}Value{q} {q}{entry.Value}{q}");

                            asset.WriteLine("\t\t}");
                        }

                        foreach (var e in cur.Entries)
                            writeEntry(e);

                        asset.WriteLine($"\t]");
                        asset.WriteLine("}");
                    }
                }
                catch
                {

                }
            }
        }
        internal static string ExportCondition(NPC.Conditions.Condition condition, string prefix, int conditionIndex, bool skipLocalization = true)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"{prefix}Condition_{conditionIndex}_Type {condition.Type}");
            foreach (PropertyInfo prop in condition.GetType().GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                {
                    continue;
                }

                string propName = prop.Name;
                SkipFieldAttribute skipPropA = prop.GetCustomAttribute<SkipFieldAttribute>();
                if ((skipLocalization && propName == "Localization") || skipPropA != null)
                {
                    continue;
                }

                object propValue = prop.GetValue(condition);
                NoValueAttribute noValueA = prop.GetCustomAttribute<NoValueAttribute>();
                OptionalAttribute optionalA = prop.GetCustomAttribute<OptionalAttribute>();
                if (skipPropA != null)
                {
                    continue;
                }

                if (noValueA != null)
                {
                    if (propValue.Equals(true))
                    {
                        propValue = "";
                    }
                    else
                    {
                        continue;
                    }
                }

                if (prop.PropertyType.IsNullable())
                {
                    if (propValue == null)
                    {
                        if (optionalA != null)
                        {
                            if (optionalA.DefaultValue == null)
                                continue;

                            propValue = optionalA.DefaultValue;
                        }
                    }
                }

                result.AppendLine($"{prefix}Condition_{conditionIndex}_{propName} {propValue}");
            }
            return result.ToString();
        }
        internal static string ExportReward(Reward reward, string prefix, int rewardIndex, bool skipLocalization = true)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"{prefix}Reward_{rewardIndex}_Type {reward.Type}");
            foreach (PropertyInfo prop in reward.GetType().GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                {
                    continue;
                }

                string propName = prop.Name;
                SkipFieldAttribute skipPropA = prop.GetCustomAttribute<SkipFieldAttribute>();
                if ((skipLocalization && propName == "Localization") || skipPropA != null)
                {
                    continue;
                }

                object propValue = prop.GetValue(reward);
                NoValueAttribute noValueA = prop.GetCustomAttribute<NoValueAttribute>();
                OptionalAttribute optionalA = prop.GetCustomAttribute<OptionalAttribute>();
                if (skipPropA != null)
                {
                    continue;
                }

                if (noValueA != null)
                {
                    if (propValue.Equals(true))
                    {
                        propValue = "";
                    }
                    else
                    {
                        continue;
                    }
                }

                if (prop.PropertyType.IsNullable())
                {
                    if (propValue == null)
                    {
                        if (optionalA != null)
                        {
                            if (optionalA.DefaultValue == null)
                                continue;

                            propValue = optionalA.DefaultValue;
                        }
                    }
                }

                result.AppendLine($"{prefix}Reward_{rewardIndex}_{propName} {propValue}");
            }
            return result.ToString();
        }

        private static EExportSchema ExportSchema => Configuration.AppConfig.Instance.exportSchema;
        private static string GetCharacterFolderName(NPCCharacter character)
        {
            switch (ExportSchema)
            {
                case EExportSchema.GUID_All_The_Way:
                    return $"{ps(character.GUID)}_{character.ID}";

                case EExportSchema.Verbose_Comment:
                    return $"{ps(character.Comment, character.EditorName, character.GUID)}_{character.ID}";

                case EExportSchema.Verbose_No_Comment:
                    return $"{ps(character.EditorName, character.GUID)}_{character.ID}";

                case EExportSchema.Default:
                default:
                    return $"{ps(character.EditorName)}_{character.ID}";
            }
        }
        private static string GetDialogueFolderName(NPCDialogue dialogue)
        {
            switch (ExportSchema)
            {
                case EExportSchema.Verbose_Comment:
                    return $"{ps(dialogue.Comment, dialogue.GUID)}_{dialogue.ID}";

                case EExportSchema.Verbose_No_Comment:
                case EExportSchema.GUID_All_The_Way:
                case EExportSchema.Default:
                default:
                    return $"{ps(dialogue.GUID)}_{dialogue.ID}";
            }
        }
        private static string GetVendorFolderName(NPCVendor vendor)
        {
            switch (ExportSchema)
            {
                case EExportSchema.Verbose_Comment:
                    return $"{ps(vendor.Comment, vendor.Title, vendor.GUID)}_{vendor.ID}";

                case EExportSchema.Verbose_No_Comment:
                    return $"{ps(vendor.Title, vendor.GUID)}_{vendor.ID}";

                case EExportSchema.GUID_All_The_Way:
                case EExportSchema.Default:
                default:
                    return $"{ps(vendor.GUID)}_{vendor.ID}";
            }
        }
        private static string GetQuestFolderName(NPCQuest quest)
        {
            switch (ExportSchema)
            {
                case EExportSchema.Verbose_Comment:
                    return $"{ps(quest.Comment, quest.Title, quest.GUID)}_{quest.ID}";

                case EExportSchema.Verbose_No_Comment:
                    return $"{ps(quest.Title, quest.GUID)}_{quest.ID}";

                case EExportSchema.GUID_All_The_Way:
                case EExportSchema.Default:
                default:
                    return $"{ps(quest.GUID)}_{quest.ID}";
            }
        }
        private static string GetCurrencyFileName(CurrencyAsset currency)
        {
            switch (ExportSchema)
            {
                default:
                    return $"{ps(currency.GUID)}.asset";
            }
        }

        private static string ps(params string[] options)
        {
            if (!(options is null))
            {
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];

                    if (string.IsNullOrWhiteSpace(option))
                        continue;

                    return MakeValidFileName(option);
                }
            }

            return Guid.NewGuid().ToString("N");
        }
        private static string MakeValidFileName(string fileName)
        {
            var invalids = System.IO.Path.GetInvalidFileNameChars();
            var newName = string.Join("_", fileName.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');

            return newName;
        }
    }
}
