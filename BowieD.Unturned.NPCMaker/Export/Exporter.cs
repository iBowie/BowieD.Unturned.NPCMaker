using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Export
{
    public static class Exporter
    {
        private const string WaterText = "// Made in NPC Maker by BowieD";
        private const string NoValue = "UNDEFINED";
        public static bool ExportCharacter(NPCCharacter character, string directory)
        {
            try
            {
                string workDir = $"{directory}Characters{Path.DirectorySeparatorChar}{character.guid}_{character.id}{Path.DirectorySeparatorChar}";
                Directory.CreateDirectory(workDir);
                using (StreamWriter assetWriter = new StreamWriter(workDir + "Asset.dat", false, Encoding.UTF8))
                using (StreamWriter localWriter = new StreamWriter(workDir + "English.dat", false, Encoding.UTF8))
                {
                    assetWriter.WriteLine(WaterText);
                    if (AppConfig.Instance.generateGuids)
                        assetWriter.WriteLine($"GUID {character.guid}");
                    assetWriter.WriteLine($"ID {character.id}");
                    assetWriter.WriteLine("Type NPC");
                    if (character.clothing.top > 0)
                        assetWriter.WriteLine($"Shirt {character.clothing.top}");
                    if (character.clothing.bottom > 0)
                        assetWriter.WriteLine($"Pants {character.clothing.bottom}");
                    if (character.clothing.mask > 0)
                        assetWriter.WriteLine($"Mask {character.clothing.mask}");
                    if (character.clothing.vest > 0)
                        assetWriter.WriteLine($"Vest {character.clothing.vest}");
                    if (character.clothing.backpack > 0)
                        assetWriter.WriteLine($"Backpack {character.clothing.backpack}");
                    if (character.clothing.hat > 0)
                        assetWriter.WriteLine($"Hat {character.clothing.hat}");
                    if (character.clothing.glasses > 0)
                        assetWriter.WriteLine($"Glasses {character.clothing.glasses}");

                    if (character.christmasClothing.top > 0)
                        assetWriter.WriteLine($"Christmas_Shirt {character.christmasClothing.top}");
                    if (character.christmasClothing.bottom > 0)
                        assetWriter.WriteLine($"Christmas_Pants {character.christmasClothing.bottom}");
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

                    if (character.halloweenClothing.top > 0)
                        assetWriter.WriteLine($"Halloween_Shirt {character.halloweenClothing.top}");
                    if (character.halloweenClothing.bottom > 0)
                        assetWriter.WriteLine($"Halloween_Pants {character.halloweenClothing.bottom}");
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
                    if (character.equipped != Equip_Type.None)
                        assetWriter.WriteLine($"Equipped {character.equipped.ToString()}");
                    assetWriter.WriteLine($"Face {character.face}");
                    assetWriter.WriteLine($"Beard {character.beard}");
                    assetWriter.WriteLine($"Hair {character.haircut}");
                    assetWriter.WriteLine($"Color_Skin {character.skinColor.ToHEX()}");
                    assetWriter.WriteLine($"Color_Hair {character.hairColor.ToHEX()}");
                    assetWriter.WriteLine($"Pose {character.pose.ToString()}");
                    if (character.leftHanded)
                        assetWriter.WriteLine("Backward");
                    if (character.startDialogueId > 0)
                        assetWriter.WriteLine($"Dialogue {character.startDialogueId}");

                    localWriter.WriteLine(WaterText);
                    localWriter.WriteLine($"Name {character.editorName}");
                    localWriter.WriteLine($"Character {character.displayName}");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
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
                        Text = LocUtil.LocalizeInterface("export_Done_Goto")
                    }
                };
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $@"results\{save.guid}"))
                {
                    Action<object, RoutedEventArgs> action = new Action<object, RoutedEventArgs>((sender, e) => { Process.Start(AppDomain.CurrentDomain.BaseDirectory + $@"results\{save.guid}"); });
                    button.Click += new RoutedEventHandler(action);
                    App.NotificationManager.Notify(LocUtil.LocalizeInterface("export_Done"), buttons: button);
                }
            }
            catch (Exception ex)
            {
                App.NotificationManager.Notify($"Export Error: {ex.Message}");
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
                        asset.WriteLine($"// Generated by NPC Maker {MainWindow.Version} by BowieD");
                        local.WriteLine($"// Generated by NPC Maker {MainWindow.Version} by BowieD");
                        if (AppConfig.Instance.generateGuids)
                            asset.WriteLine($"GUID {character.guid}");
                        asset.WriteLine($"ID {character.id}");
                        asset.WriteLine($"Type NPC");
                        if (character.clothing.top > 0)
                            asset.WriteLine($"Shirt {character.clothing.top}");
                        if (character.clothing.bottom > 0)
                            asset.WriteLine($"Pants {character.clothing.bottom}");
                        if (character.clothing.mask > 0)
                            asset.WriteLine($"Mask {character.clothing.mask}");
                        if (character.clothing.vest > 0)
                            asset.WriteLine($"Vest {character.clothing.vest}");
                        if (character.clothing.backpack > 0)
                            asset.WriteLine($"Backpack {character.clothing.backpack}");
                        if (character.clothing.hat > 0)
                            asset.WriteLine($"Hat {character.clothing.hat}");
                        if (character.clothing.glasses > 0)
                            asset.WriteLine($"Glasses {character.clothing.glasses}");

                        if (character.christmasClothing.top > 0)
                            asset.WriteLine($"Christmas_Shirt {character.christmasClothing.top}");
                        if (character.christmasClothing.bottom > 0)
                            asset.WriteLine($"Christmas_Pants {character.christmasClothing.bottom}");
                        if (character.christmasClothing.mask > 0)
                            asset.WriteLine($"Christmas_Mask {character.christmasClothing.mask}");
                        if (character.christmasClothing.vest > 0)
                            asset.WriteLine($"Christmas_Vest {character.christmasClothing.vest}");
                        if (character.christmasClothing.backpack > 0)
                            asset.WriteLine($"Christmas_Backpack {character.christmasClothing.backpack}");
                        if (character.christmasClothing.hat > 0)
                            asset.WriteLine($"Christmas_Hat {character.christmasClothing.hat}");
                        if (character.christmasClothing.glasses > 0)
                            asset.WriteLine($"Christmas_Glasses {character.christmasClothing.glasses}");

                        if (character.halloweenClothing.top > 0)
                            asset.WriteLine($"Halloween_Shirt {character.halloweenClothing.top}");
                        if (character.halloweenClothing.bottom > 0)
                            asset.WriteLine($"Halloween_Pants {character.halloweenClothing.bottom}");
                        if (character.halloweenClothing.mask > 0)
                            asset.WriteLine($"Halloween_Mask {character.halloweenClothing.mask}");
                        if (character.halloweenClothing.vest > 0)
                            asset.WriteLine($"Halloween_Vest {character.halloweenClothing.vest}");
                        if (character.halloweenClothing.backpack > 0)
                            asset.WriteLine($"Halloween_Backpack {character.halloweenClothing.backpack}");
                        if (character.halloweenClothing.hat > 0)
                            asset.WriteLine($"Halloween_Hat {character.halloweenClothing.hat}");
                        if (character.halloweenClothing.glasses > 0)
                            asset.WriteLine($"Halloween_Glasses {character.halloweenClothing.glasses}");

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
                                asset.WriteLine(cond.GetFullFilePresentation("", -1, k));
                            }
                        }

                        local.WriteLine($"Name {character.editorName}");
                        local.WriteLine($"Character {character.displayName}");
                    }
                }
                catch (Exception ex) { App.NotificationManager.Notify($"Can't export character {character.id}. Exception: {ex.Message}"); }
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
                        asset.WriteLine($"// Generated by NPC Maker {MainWindow.Version} by BowieD");
                        local.WriteLine($"// Generated by NPC Maker {MainWindow.Version} by BowieD");
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
                                List<NPCResponse> visibleResponses = dialogue.responses.Where(d => d.VisibleInAll || d.visibleIn[k] == 1).ToList();
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
                                        asset.WriteLine(message.conditions[c].GetFullFilePresentation("Message_", k, c));
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
                                        if (response.visibleIn[c] == 1)
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
                                        asset.WriteLine(response.conditions[c].GetFullFilePresentation("Response_", k, c));
                                    }
                                }
                                if (response.rewards.Count() > 0)
                                {
                                    asset.WriteLine($"Response_{k}_Rewards {response.rewards.Count()}");
                                    int rwrdCnt = response.rewards.Count();
                                    for (int c = 0; c < rwrdCnt; c++)
                                    {
                                        asset.WriteLine(response.rewards[c].GetFullFilePresentation("Response_", k, c));
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
                catch (Exception ex) { App.NotificationManager.Notify($"Can't export dialogue {dialogue.id}. Exception: {ex.Message}"); }
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
                        asset.WriteLine($"// Generated by NPC Maker {MainWindow.Version} by BowieD");
                        local.WriteLine($"// Generated by NPC Maker {MainWindow.Version} by BowieD");
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
                                asset.WriteLine($"Buying_{k}_Amount {buy[k].amount}");
                                if (buy[k].conditions?.Count > 0)
                                {
                                    asset.WriteLine($"Buying_{k}_Conditions {buy[k].conditions.Count}");
                                    for (int c = 0; c < buy[k].conditions.Count; c++)
                                    {
                                        asset.WriteLine(buy[k].conditions[c].GetFullFilePresentation("Buying", k, c));
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
                                else
                                {
                                    asset.WriteLine($"Selling_{k}_Amount {sell[k].amount}");
                                }
                                asset.WriteLine($"Selling_{k}_ID {sell[k].id}");
                                asset.WriteLine($"Selling_{k}_Cost {sell[k].cost}");
                                if (sell[k].conditions?.Count > 0)
                                {
                                    asset.WriteLine($"Selling_{k}_Conditions {sell[k].conditions.Count}");
                                    for (int c = 0; c < sell[k].conditions.Count; c++)
                                    {
                                        asset.WriteLine(sell[k].conditions[c].GetFullFilePresentation("Selling", k, c));
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
                catch (Exception ex) { App.NotificationManager.Notify($"Can't export vendor {vendor.id}. Exception: {ex.Message}"); }
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
                        asset.WriteLine($"// Generated by NPC Maker {MainWindow.Version} by BowieD");
                        local.WriteLine($"// Generated by NPC Maker {MainWindow.Version} by BowieD");
                        if (AppConfig.Instance.generateGuids)
                            asset.WriteLine($"GUID {quest.guid}");
                        asset.WriteLine($"Type Quest");
                        asset.WriteLine($"ID {quest.id}");

                        if (quest.conditions?.Count > 0)
                        {
                            asset.WriteLine($"Conditions {quest.conditions.Count}");
                            for (int k = 0; k < quest.conditions.Count; k++)
                            {
                                asset.WriteLine(quest.conditions[k].GetFullFilePresentation("", k, k, false));
                            }
                        }

                        if (quest.rewards?.Count > 0)
                        {
                            asset.WriteLine($"Rewards {quest.rewards.Count}");
                            for (int k = 0; k < quest.rewards.Count; k++)
                            {
                                asset.WriteLine(quest.rewards[k].GetFullFilePresentation("", k, k, false));
                            }
                        }

                        local.WriteLine($"Name {quest.title}");
                        local.WriteLine($"Description {quest.description}");
                        for (int k = 0; k < quest.conditions?.Count; k++)
                        {
                            local.WriteLine($"Condition_{k} {quest.conditions[k].Localization}");
                        }
                    }
                }
                catch (Exception ex) { App.NotificationManager.Notify($"Can't export quest {quest.id}. Exception: {ex.Message}"); }
            }
        }
    }
}
