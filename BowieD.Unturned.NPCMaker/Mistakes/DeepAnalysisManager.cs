using BowieD.Unturned.NPCMaker.Common.Utility;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.Mistakes
{
    public class DeepAnalysisManager
    {
        public DeepAnalysisManager()
        {
            MainWindow.Instance.deepAnalysis_Button.Click += DeepAnalysisButton_Click;
        }
        public async void DeepAnalysisButton_Click(object sender, RoutedEventArgs e)
        {
            MistakesManager.FindMistakes();
            foreach (NPCDialogue dialogue in MainWindow.CurrentProject.data.dialogues)
            {
                if (GameAssetManager.TryGetAsset<GameDialogueAsset>(dialogue.ID, out _))
                {
                    MainWindow.Instance.lstMistakes.Items.Add(new Mistake()
                    {
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_dialogue", dialogue.ID),
                        Importance = IMPORTANCE.WARNING
                    });
                }
                await Task.Yield();
            }
            foreach (NPCVendor vendor in MainWindow.CurrentProject.data.vendors)
            {
                if (GameAssetManager.TryGetAsset<GameVendorAsset>(vendor.ID, out _))
                {
                    MainWindow.Instance.lstMistakes.Items.Add(new Mistake()
                    {
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_vendor", vendor.ID),
                        Importance = IMPORTANCE.WARNING
                    });
                }
                foreach (VendorItem it in vendor.items)
                {
                    if (it.type == ItemType.VEHICLE)
                    {
                        if (GameAssetManager.TryGetAsset<GameVehicleAsset>(it.id, out _))
                        {
                            MainWindow.Instance.lstMistakes.Items.Add(new Mistake()
                            {
                                MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_vehicle", it.id),
                                Importance = IMPORTANCE.WARNING
                            });
                            continue;
                        }
                    }
                    if (it.type == ItemType.ITEM)
                    {
                        if (GameAssetManager.TryGetAsset<GameItemAsset>(it.id, out _))
                        {
                            MainWindow.Instance.lstMistakes.Items.Add(new Mistake()
                            {
                                MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_item", it.id),
                                Importance = IMPORTANCE.WARNING
                            });
                            continue;
                        }
                    }
                }
                await Task.Yield();
            }
            foreach (NPCQuest quest in MainWindow.CurrentProject.data.quests)
            {
                if (GameAssetManager.TryGetAsset<GameQuestAsset>(quest.ID, out _))
                {
                    MainWindow.Instance.lstMistakes.Items.Add(new Mistake()
                    {
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_quest", quest.ID),
                        Importance = IMPORTANCE.WARNING
                    });
                }
                await Task.Yield();
            }
            foreach (NPCCharacter character in MainWindow.CurrentProject.data.characters)
            {
                if (character.ID > 0)
                {
                    if (GameAssetManager.TryGetAsset<GameNPCAsset>(character.ID, out _))
                    {
                        MainWindow.Instance.lstMistakes.Items.Add(new Mistake()
                        {
                            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_char", character.ID),
                            Importance = IMPORTANCE.WARNING
                        });
                    }
                }
            }
            MainWindow.Instance.blockActionsOverlay.Visibility = Visibility.Collapsed;
            if (MainWindow.Instance.lstMistakes.Items.Count == 0)
            {
                MainWindow.Instance.lstMistakes.Visibility = Visibility.Collapsed;
                MainWindow.Instance.noErrorsLabel.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.Instance.lstMistakes.Visibility = Visibility.Visible;
                MainWindow.Instance.noErrorsLabel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
