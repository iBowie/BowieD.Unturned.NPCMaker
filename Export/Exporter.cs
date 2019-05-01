﻿using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Export
{
    public static class Exporter
    {
        public static void ExportNPC(NPCSave save)
        {
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $@"\results\{save.guid}"))
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + $@"\results\{save.guid}", true);
            dir = AppDomain.CurrentDomain.BaseDirectory + $@"\results\{save.guid}\";
            Export_Characters(save.characters);
            Export_Dialogues(save.dialogues);
            //Export_Objects(save.objects);
            Export_Quests(save.quests);
            Export_Vendors(save.vendors);
            Button button = new Button
            {
                Content = new TextBlock
                {
                    Text = MainWindow.Localize("export_Done_Goto")
                }
            };
            Action<object, RoutedEventArgs> action = new Action<object, RoutedEventArgs>((sender, e) => { Process.Start(AppDomain.CurrentDomain.BaseDirectory + $@"results\{save.guid}"); });
            button.Click += new RoutedEventHandler(action);
            MainWindow.NotificationManager.Notify(MainWindow.Localize("export_Done"), buttons: button);
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
                        if (Config.Configuration.Properties.generateGuids)
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
                        asset.WriteLine($"Color_Skin {character.skinColor.HEX}");
                        asset.WriteLine($"Color_Hair {character.hairColor.HEX}");
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
                catch (Exception ex) { MainWindow.NotificationManager.Notify($"Can't export character {character.id}. Exception: {ex.Message}"); }
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
                        if (Config.Configuration.Properties.generateGuids)
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
                                        asset.WriteLine(response.rewards[c].GetFilePresentation("Response_", k, c));
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
                catch (Exception ex) { MainWindow.NotificationManager.Notify($"Can't export dialogue {dialogue.id}. Exception: {ex.Message}"); }
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
                        if (Config.Configuration.Properties.generateGuids)
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

                        local.WriteLine($"Name {vendor.vendorTitle}");
                        local.WriteLine($"Description {vendor.vendorDescription}");
                    }
                }
                catch (Exception ex) { MainWindow.NotificationManager.Notify($"Can't export vendor {vendor.id}. Exception: {ex.Message}"); }
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
                        if (Config.Configuration.Properties.generateGuids)
                            asset.WriteLine($"GUID {quest.guid}");
                        asset.WriteLine($"Type Quest");
                        asset.WriteLine($"ID {quest.id}");

                        if (quest.conditions?.Count > 0)
                        {
                            asset.WriteLine($"Conditions {quest.conditions.Count}");
                            for (int k = 0; k < quest.conditions.Count; k++)
                            {
                                asset.WriteLine(quest.conditions[k].GetFullFilePresentation("", k, k));
                            }
                        }

                        if (quest.rewards?.Count > 0)
                        {
                            asset.WriteLine($"Rewards {quest.rewards.Count}");
                            for (int k = 0; k < quest.rewards.Count; k++)
                            {
                                asset.WriteLine(quest.rewards[k].GetFilePresentation("", k, k));
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
                catch (Exception ex) { MainWindow.NotificationManager.Notify($"Can't export quest {quest.id}. Exception: {ex.Message}"); }
            }
        }
        //private static void Export_Objects(IEnumerable<NPCObject> objects)
        //{
        //    if (objects.Count() > 0)
        //    {
        //        Action<NPCObject, StreamWriter> writeHint = new Action<NPCObject, StreamWriter>((NPCObject obj, StreamWriter stream) =>
        //        {
        //            if (obj.hint != Object_Hint.None)
        //            {
        //                switch (obj.hint)
        //                {
        //                    case Object_Hint.Door:
        //                        stream.WriteLine($"Interactability_Hint Door");
        //                        break;
        //                    case Object_Hint.Fire:
        //                        stream.WriteLine($"Interactability_Hint Fire");
        //                        break;
        //                    case Object_Hint.Generator:
        //                        stream.WriteLine($"Interactability_Hint Generator");
        //                        break;
        //                    case Object_Hint.Switch:
        //                        stream.WriteLine($"Interactability_Hint Switch");
        //                        break;
        //                    case Object_Hint.Use:
        //                        stream.WriteLine($"Interactability_Hint Use");
        //                        break;
        //                }
        //            }
        //        });
        //        Action<NPCObject, StreamWriter> writeEditor = new Action<NPCObject, StreamWriter>((NPCObject obj, StreamWriter stream) =>
        //        {
        //            if (obj.interactInEditor)
        //                stream.WriteLine($"Interactability_Editor Toggle");
        //        });
        //        foreach (NPCObject obj in objects)
        //        {
        //            try
        //            {
        //                if (obj.assetFilePath == null || !File.Exists(obj.assetFilePath))
        //                {
        //                    Logger.Log($"Can't find asset file for {obj.name}_{obj.ID}");
        //                    continue;
        //                }
        //                Directory.CreateDirectory(dir + $@"Objects\{obj.name}_{obj.ID}");
        //                File.Copy(obj.assetFilePath, dir + $@"Objects\{obj.name}_{obj.ID}\{obj.name}_{obj.ID}.unity3d");
        //                using (StreamWriter asset = new StreamWriter(dir + $@"Objects\{obj.name}_{obj.ID}\{obj.name}_{obj.ID}.dat", false, Encoding.UTF8))
        //                using (StreamWriter local = new StreamWriter(dir + $@"Objects\{obj.name}_{obj.ID}\English.dat", false, Encoding.UTF8))
        //                {
        //                    asset.WriteLine($"// Generated by NPC Maker {MainWindow.Version} by BowieD");
        //                    local.WriteLine($"// Generated by NPC Maker {MainWindow.Version} by BowieD");
        //                    local.WriteLine($"Name {obj.name}");

        //                    asset.WriteLine($"GUID {obj.guid}");
        //                    switch (obj.type)
        //                    {
        //                        case Object_Type.SMALL:
        //                            asset.WriteLine($"Type Small");
        //                            break;
        //                        case Object_Type.MEDIUM:
        //                            asset.WriteLine($"Type Medium");
        //                            break;
        //                        case Object_Type.LARGE:
        //                            asset.WriteLine($"Type Large");
        //                            break;
        //                    }
        //                    asset.WriteLine($"ID {obj.ID}");
        //                    if (obj.conditions.Count > 0)
        //                    {
        //                        int condIndex = 0;
        //                        foreach (var cond in obj.conditions)
        //                        {
        //                            string fileP = cond.GetFullFilePresentation("", -1, condIndex++);
        //                            asset.WriteLine(fileP);
        //                        }
        //                    }

        //                    if (obj.interactability != Object_Interactability.None)
        //                    {
        //                        switch (obj.interactability)
        //                        {
        //                            case Object_Interactability.Binary_State:
        //                                asset.WriteLine($"Interactability Binary_State");
        //                                asset.WriteLine($"Interactability_Delay {obj.interactDelay.ToString(CultureInfo.InvariantCulture)}");
        //                                asset.WriteLine($"Interactability_Reset {obj.interactReset}");
        //                                writeHint.Invoke(obj, asset);
        //                                writeEditor.Invoke(obj, asset);
        //                                break;
        //                            case Object_Interactability.Dropper:
        //                                asset.WriteLine($"Interactability Dropper");
        //                                asset.WriteLine($"Interactability_Delay {obj.interactDelay}");
        //                                writeHint.Invoke(obj, asset);
        //                                asset.WriteLine($"Interactability_Reward_ID {obj.interactDrop}");
        //                                break;
        //                            case Object_Interactability.Fuel:
        //                            case Object_Interactability.Water:
        //                                if (obj.interactability == Object_Interactability.Fuel)
        //                                    asset.WriteLine($"Interactability Fuel");
        //                                else
        //                                    asset.WriteLine($"Interactability Water");
        //                                asset.WriteLine($"Interactability_Reset {obj.interactReset}");
        //                                asset.WriteLine($"Interactability_Resource {obj.interactResource}");
        //                                break;
        //                            case Object_Interactability.Note:
        //                                asset.WriteLine($"Interactability Note");
        //                                writeHint.Invoke(obj, asset);
        //                                asset.WriteLine($"Interactability_Text_Lines {obj.noteLines.Count}");
        //                                int lineIndex = 0;
        //                                foreach (string s in obj.noteLines)
        //                                {
        //                                    local.WriteLine($"Interactability_Text_Line_{lineIndex++} {s}");
        //                                }
        //                                break;
        //                            case Object_Interactability.Quest:
        //                                asset.WriteLine($"Interactability Quest");
        //                                asset.WriteLine($"Interactability_Effect {obj.interactEffect}");
        //                                if (obj.rewards.Count > 0)
        //                                {
        //                                    asset.WriteLine($"Interactability_Rewards {obj.rewards.Count}");
        //                                    int rewardIndex = 0;
        //                                    foreach (Reward reward in obj.rewards)
        //                                    {
        //                                        string fileP = reward.GetFilePresentation("Interactability", -1, rewardIndex++);
        //                                        asset.WriteLine(fileP);
        //                                    }
        //                                }
        //                                break;
        //                            case Object_Interactability.Rubble:
        //                                asset.WriteLine($"Interactability Rubble");
        //                                asset.WriteLine($"Interactability_Reset {obj.interactReset}");
        //                                asset.WriteLine($"Interactability_Health {obj.interactHealth}");
        //                                asset.WriteLine($"Interactability_Effect {obj.interactEffect}");
        //                                break;
        //                        }
        //                    }

        //                    if (obj.IsRubble)
        //                    {
        //                        asset.WriteLine($"Rubble Destroy");
        //                        asset.WriteLine($"Rubble_Reset {obj.RubbleReset}");
        //                        asset.WriteLine($"Rubble_Health {obj.RubbleHealth}");
        //                        asset.WriteLine($"Rubble_Effect {obj.RubbleEffect}");
        //                    }
        //                }
        //            }
        //            catch (Exception ex) { }
        //        }
        //    }
        //}
    }
}