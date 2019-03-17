using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Export_ExportWindow.xaml
    /// </summary>
    public partial class Export_ExportWindow : Window
    {
        public Export_ExportWindow(string baseDir)
        {
            InitializeComponent();
            dir = baseDir;
        }

        public void DoActions(NPCSave save)
        {
            base.Show();
            Start(save);
            TextBlock text = new TextBlock
            {
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
                Text = (string)TryFindResource("export_Done"),
                TextWrapping = TextWrapping.Wrap
            };
            TextBlock buttonText = new TextBlock
            {
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                Text = (string)TryFindResource("export_Done_Goto")
            };
            Action<object, RoutedEventArgs> action = new Action<object, RoutedEventArgs>((sender, e) => { Process.Start(AppDomain.CurrentDomain.BaseDirectory + "results"); });
            MainWindow.Instance.DoNotification(text, buttonText, action);
            this.Close();
        }

        public void Start(NPCSave save)
        {
            try
            {
                Prepare(save, dir);
                detailedExport.Text = ((string)TryFindResource("export_StepFormat")).Replace("%done%", "1").Replace("%total%", "4").Replace("%step%", (string)FindResource("export_Step_Character"));
                Export_Character(save);
                detailedExport.Text = ((string)TryFindResource("export_StepFormat")).Replace("%done%", "2").Replace("%total%", "4").Replace("%step%", (string)FindResource("export_Step_Dialogues"));
                Export_Dialogues(save);
                detailedExport.Text = ((string)TryFindResource("export_StepFormat")).Replace("%done%", "3").Replace("%total%", "4").Replace("%step%", (string)FindResource("export_Step_Quests"));
                Export_Quests(save);
                detailedExport.Text = ((string)TryFindResource("export_StepFormat")).Replace("%done%", "4").Replace("%total%", "4").Replace("%step%", (string)FindResource("export_Step_Vendors"));
                Export_Vendors(save);
            }
            catch (Exception ex) { MainWindow.Instance.DoNotification($"Export failed. Exception: {ex.Message}"); }
        }

        public string dir;

        public void Prepare(NPCSave save, string dir)
        {
            try
            {
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + $@"results\{save.editorName}", true);
            }
            catch (Exception ex) { Logger.Log("Can't delete directory with NPC.", Log_Level.Normal); Logger.Log(ex.Message, Log_Level.Errors); }
            Directory.CreateDirectory(dir + $@"Characters\{save.editorName}");
            if (save.dialogues?.Count() > 0)
                Directory.CreateDirectory(dir + $@"Dialogues\{save.editorName}");
            if (save.vendors?.Count() > 0)
                Directory.CreateDirectory(dir + $@"Vendors\{save.editorName}");
            if (save.quests?.Count() > 0)
                Directory.CreateDirectory(dir + $@"Quests\{save.editorName}");
        }

        public void Export_Character(NPCSave save)
        {
            try
            {
                using (StreamWriter asset = new StreamWriter(dir + $@"Characters\{save.editorName}\Asset.dat", false, Encoding.UTF8))
                using (StreamWriter local = new StreamWriter(dir + $@"Characters\{save.editorName}\English.dat", false, Encoding.UTF8))
                {

                    asset.WriteLine();
                    local.WriteLine();
                    if (Config.Configuration.Properties.generateGuids)
                        asset.WriteLine($"GUID {save.guid}");
                    asset.WriteLine($"ID {save.id}");
                    asset.WriteLine($"Type NPC");
                    if (save.top > 0)
                        asset.WriteLine($"Shirt {save.top}");
                    if (save.bottom > 0)
                        asset.WriteLine($"Pants {save.bottom}");
                    if (save.mask > 0)
                        asset.WriteLine($"Mask {save.mask}");
                    if (save.vest > 0)
                        asset.WriteLine($"Vest {save.vest}");
                    if (save.backpack > 0)
                        asset.WriteLine($"Backpack {save.backpack}");
                    if (save.hat > 0)
                        asset.WriteLine($"Hat {save.hat}");
                    if (save.glasses > 0)
                        asset.WriteLine($"Glasses {save.glasses}");
                    if (save.equipPrimary > 0)
                        asset.WriteLine($"Primary {save.equipPrimary}");
                    if (save.equipSecondary > 0)
                        asset.WriteLine($"Secondary {save.equipSecondary}");
                    if (save.equipTertiary > 0)
                        asset.WriteLine($"Tertiary {save.equipTertiary}");
                    if (save.equipped != Equip_Type.None)
                        asset.WriteLine($"Equipped {save.equipped.ToString()}");
                    asset.WriteLine($"Face {save.face}");
                    asset.WriteLine($"Beard {save.beard}");
                    asset.WriteLine($"Hair {save.haircut}");
                    asset.WriteLine($"Color_Skin {save.skinColor.HEX}");
                    asset.WriteLine($"Color_Hair {save.hairColor.HEX}");
                    asset.WriteLine($"Pose {save.pose.ToString()}");
                    if (save.leftHanded)
                        asset.WriteLine($"Backward");
                    if (save.startDialogueId > 0)
                        asset.WriteLine($"Dialogue {save.startDialogueId}");

                    if (save.visibilityConditions?.Count() > 0)
                    {
                        int condCount = save.visibilityConditions.Count();
                        asset.WriteLine($"Conditions {condCount}");
                        for (int k = 0; k < condCount; k++)
                        {
                            var cond = save.visibilityConditions.ElementAt(k);
                            asset.WriteLine(cond.GetFullFilePresentation("", -1, k));
                        }
                    }

                    local.WriteLine($"Name {save.editorName}");
                    local.WriteLine($"Character {save.displayName}");
                }
            }
            catch (Exception ex) { MainWindow.Instance.DoNotification($"Can't export character. Exception: {ex.Message}"); }
        }

        public void Export_Dialogues(NPCSave save)
        {
            if (save.dialogues?.Count() == 0)
                return;
            foreach (NPCDialogue dialogue in save.dialogues)
            {
                try
                {

                    Directory.CreateDirectory(dir + $@"Dialogues\{save.editorName}\{save.editorName}_{dialogue.id}");
                    using (StreamWriter asset = new StreamWriter(dir + $@"Dialogues\{save.editorName}\{save.editorName}_{dialogue.id}\Asset.dat", false, Encoding.UTF8))
                    using (StreamWriter local = new StreamWriter(dir + $@"Dialogues\{save.editorName}\{save.editorName}_{dialogue.id}\English.dat", false, Encoding.UTF8))
                    {

                        asset.WriteLine();
                        local.WriteLine();
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
                                List<NPCResponse> visibleResponses = dialogue.responses.Where(d => d.VisibleInAll || d.visibleIn.Contains(k)).ToList();
                                if (visibleResponses.Count() > 0)
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
                                    asset.WriteLine($"Response_{k}_Messages {response.visibleIn.Length}");
                                    for (int c = 0; c < response.visibleIn.Length; c++)
                                    {
                                        asset.WriteLine($"Response_{k}_Message_{c} {response.visibleIn[c]}");
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
                catch (Exception ex) { MainWindow.Instance.DoNotification($"Can't export dialogue {dialogue.id}. Exception: {ex.Message}"); }
            }
        }

        public void Export_Vendors(NPCSave save)
        {
            if (save.vendors?.Count() == 0)
                return;
            foreach (NPCVendor vendor in save.vendors)
            {
                try
                {
                    Directory.CreateDirectory(dir + $@"Vendors\{save.editorName}\{save.editorName}_{vendor.id}");
                    using (StreamWriter asset = new StreamWriter(dir + $@"Vendors\{save.editorName}\{save.editorName}_{vendor.id}\Asset.dat", false, Encoding.UTF8))
                    using (StreamWriter local = new StreamWriter(dir + $@"Vendors\{save.editorName}\{save.editorName}_{vendor.id}\English.dat", false, Encoding.UTF8))
                    {

                        asset.WriteLine();
                        local.WriteLine();
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
                catch (Exception ex) { MainWindow.Instance.DoNotification($"Can't export vendor {vendor.id}. Exception: {ex.Message}"); }
            }
        }

        public void Export_Quests(NPCSave save)
        {
            if (save.quests?.Count() == 0)
                return;
            foreach (NPCQuest quest in save.quests)
            {
                try
                {
                    Directory.CreateDirectory(dir + $@"Quests\{save.editorName}\{save.editorName}_{quest.id}");
                    using (StreamWriter asset = new StreamWriter(dir + $@"Quests\{save.editorName}\{save.editorName}_{quest.id}\Asset.dat", false, Encoding.UTF8))
                    using (StreamWriter local = new StreamWriter(dir + $@"Quests\{save.editorName}\{save.editorName}_{quest.id}\English.dat", false, Encoding.UTF8))
                    {

                        asset.WriteLine();
                        local.WriteLine();
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
                catch (Exception ex) { MainWindow.Instance.DoNotification($"Can't export quest {quest.id}. Exception: {ex.Message}"); }
            }
        }
    }
}
