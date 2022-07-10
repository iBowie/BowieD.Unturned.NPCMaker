using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Mistakes.Character;
using BowieD.Unturned.NPCMaker.Mistakes.Currencies;
using BowieD.Unturned.NPCMaker.Mistakes.Dialogue;
using BowieD.Unturned.NPCMaker.Mistakes.Quest;
using BowieD.Unturned.NPCMaker.Mistakes.Vendor;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Mistakes
{
    public static class MistakesManager
    {
        public static void FindMistakes()
        {
            if (CheckMistakes == null)
            {
                CheckMistakes = new HashSet<Mistake>();

                void register<T>() where T : Mistake, new()
                {
                    CheckMistakes.Add(new T());
                }

                // characters
                register<NE_0000>();
                register<NE_0001>();
                register<NE_0002>();
                register<NE_0003>();
                register<NE_0004>();
                register<NE_0005>();
                // dialogues
                register<NE_1000>();
                register<NE_1001>();
                register<NE_1002>();
                register<NE_1003>();
                register<NE_1004>();
                register<NE_1005>();
                register<NE_1006>();
                register<NE_1007>();
                register<NE_1008>();
                register<NE_1009>();
                register<NE_1010>();
                // vendors
                register<NE_2000>();
                register<NE_2001>();
                register<NE_2002>();
                register<NE_2003>();
                register<NE_2004>();
                // quests
                register<NE_3000>();
                register<NE_3001>();
                register<NE_3002>();
                register<NE_3003>();
                register<NE_3004>();
                // currencies
                register<NE_4000>();
                register<NE_4001>();
                register<NE_4002>();
                register<NE_4003>();
                register<NE_4004>();
            }
            MainWindow.Instance.MainWindowViewModel.SaveAll();
            MainWindow.Instance.lstMistakes.Items.Clear();
            FoundMistakes.Clear();
            foreach (Mistake m in CheckMistakes)
            {
                IEnumerable<Mistake> mistakes = m.CheckMistake();
                foreach (Mistake fm in mistakes)
                {
                    string descKey = $"{fm.MistakeName}_Desc";
                    if (fm.MistakeDesc == null)
                    {
                        if (LocalizationManager.Current.Mistakes.TryGetValue(descKey, out string desc))
                        {
                            fm.MistakeDesc = desc;
                        }
                    }

                    string solutionKey = $"{fm.MistakeName}_Solution";
                    if (fm.MistakeSolution == null)
                    {
                        if (LocalizationManager.Current.Mistakes.TryGetValue(solutionKey, out string solution))
                        {
                            fm.MistakeSolution = solution;
                        }
                    }

                    FoundMistakes.Add(fm);
                }
            }

            foreach (NPCDialogue dialogue in MainWindow.CurrentProject.data.dialogues)
            {
                if (GameAssetManager.TryGetAsset<GameAsset>((asset) =>
                {
                    if (asset is GameDialogueAsset gda)
                    {
                        if (gda.dialogue != null && gda.dialogue == dialogue)
                            return false;

                        return gda.id == dialogue.ID;
                    }
                    else
                    {
                        return asset.Category == EGameAssetCategory.NPC && asset.id == dialogue.ID;
                    }
                }, out _))
                {
                    Mistake newMistake = new Mistake()
                    {
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_dialogue", dialogue.ID),
                        Importance = IMPORTANCE.WARNING
                    };
                    FoundMistakes.Add(newMistake);
                }
            }

            foreach (NPCVendor vendor in MainWindow.CurrentProject.data.vendors)
            {
                if (GameAssetManager.TryGetAsset<GameAsset>((asset) =>
                {
                    if (asset is GameVendorAsset gva)
                    {
                        if (gva.vendor != null && gva.vendor == vendor)
                            return false;

                        return gva.id == vendor.ID;
                    }
                    else
                    {
                        return asset.Category == EGameAssetCategory.NPC && asset.id == vendor.ID;
                    }
                }, out _))
                {
                    Mistake newMistake = new Mistake()
                    {
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_vendor", vendor.ID),
                        Importance = IMPORTANCE.WARNING
                    };
                    FoundMistakes.Add(newMistake);
                }
                foreach (VendorItem it in vendor.items)
                {
                    if (it.type == ItemType.VEHICLE)
                    {
                        if (GameAssetManager.HasImportedAssets && !GameAssetManager.TryGetAsset<GameVehicleAsset>(it.id, out _))
                        {
                            Mistake newMistake = new Mistake()
                            {
                                MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_vehicle", it.id),
                                Importance = IMPORTANCE.WARNING
                            };
                            FoundMistakes.Add(newMistake);
                            continue;
                        }
                    }
                    if (it.type == ItemType.ITEM)
                    {
                        if (GameAssetManager.HasImportedAssets && !GameAssetManager.TryGetAsset<GameItemAsset>(it.id, out _))
                        {
                            Mistake newMistake = new Mistake()
                            {
                                MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_item", it.id),
                                Importance = IMPORTANCE.WARNING
                            };
                            FoundMistakes.Add(newMistake);
                            continue;
                        }
                    }
                }
            }

            foreach (NPCQuest quest in MainWindow.CurrentProject.data.quests)
            {
                if (GameAssetManager.TryGetAsset<GameAsset>((asset) =>
                {
                    if (asset is GameQuestAsset gqa)
                    {
                        if (gqa.quest != null && gqa.quest == quest)
                            return false;

                        return gqa.id == quest.ID;
                    }
                    else
                    {
                        return asset.Category == EGameAssetCategory.NPC && asset.id == quest.ID;
                    }
                }, out _))
                {
                    Mistake newMistake = new Mistake()
                    {
                        MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_quest", quest.ID),
                        Importance = IMPORTANCE.WARNING
                    };
                    FoundMistakes.Add(newMistake);
                }
            }

            foreach (NPCCharacter character in MainWindow.CurrentProject.data.characters)
            {
                if (character.ID > 0)
                {
                    if (GameAssetManager.TryGetAsset<GameAsset>((asset) =>
                    {
                        if (asset is GameNPCAsset gva)
                        {
                            if (gva.character != null && gva.character == character)
                                return false;

                            return gva.id == character.ID;
                        }
                        else
                        {
                            return asset.Category == EGameAssetCategory.OBJECT && asset.id == character.ID;
                        }
                    }, out _))
                    {
                        Mistake newMistake = new Mistake()
                        {
                            MistakeDesc = LocalizationManager.Current.Mistakes.Translate("deep_char", character.ID),
                            Importance = IMPORTANCE.WARNING
                        };
                        FoundMistakes.Add(newMistake);
                    }
                }
            }

            FoundMistakes.RemoveWhere(d =>
            {
                if (d.Importance == IMPORTANCE.CRITICAL)
                    return false;

                if (AppConfig.Instance.disabledErrors.Contains(d.MistakeName))
                    return true;

                return false;
            });

            foreach (var fm in FoundMistakes)
            {
                MainWindow.Instance.lstMistakes.Items.Add(fm);
            }

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
        private static HashSet<Mistake> CheckMistakes;
        public static int Advices_Count => FoundMistakes.Count(d => d.Importance == IMPORTANCE.ADVICE);
        public static int Warnings_Count => FoundMistakes.Count(d => d.Importance == IMPORTANCE.WARNING);
        public static int Criticals_Count => FoundMistakes.Count(d => d.Importance == IMPORTANCE.CRITICAL);
        public static HashSet<Mistake> FoundMistakes { get; } = new HashSet<Mistake>();
    }
}
