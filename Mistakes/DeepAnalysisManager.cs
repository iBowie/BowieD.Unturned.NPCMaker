using BowieD.Unturned.NPCMaker.Logging;
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
            bool skipCache = false;
            if (CachedUnturnedFiles?.Count() > 0)
            {
                var res = MessageBox.Show(MainWindow.Localize("mistakes_DA_UpdateCache"), "", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Cancel)
                {
                    MainWindow.Instance.blockActionsOverlay.Visibility = Visibility.Collapsed;
                    return;
                }
                if (res == MessageBoxResult.No)
                    skipCache = true;
            }
            if (!skipCache)
            {
                MainWindow.Instance.blockActionsOverlay.Visibility = Visibility.Visible;
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog
                {
                    ShowNewFolderButton = false
                };
                var res = fbd.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.Cancel || !File.Exists(fbd.SelectedPath + @"\Unturned.exe"))
                {
                    MainWindow.Instance.blockActionsOverlay.Visibility = Visibility.Collapsed;
                    return;
                }
                await CacheFiles(fbd.SelectedPath);
                MainWindow.Instance.blockActionsOverlay.Visibility = Visibility.Collapsed;
            }
            MistakesManager.FindMistakes();
            foreach (NPCDialogue dialogue in MainWindow.CurrentNPC.dialogues)
            {
                if (CachedUnturnedFiles != null && CachedUnturnedFiles.Any(d => d.Type == UnturnedFile.EAssetType.Dialogue && d.Id == dialogue.id))
                {
                    MainWindow.Instance.lstMistakes.Items.Add(new Mistakes.Generic(MainWindow.Localize("deep_dialogue", dialogue.id), "", IMPORTANCE.WARNING, true, false));
                }
                await Task.Yield();
            }
            foreach (NPCVendor vendor in MainWindow.CurrentNPC.vendors)
            {
                if (CachedUnturnedFiles != null && CachedUnturnedFiles.Any(d => d.Type == UnturnedFile.EAssetType.Vendor && d.Id == vendor.id))
                {
                    MainWindow.Instance.lstMistakes.Items.Add(new Mistakes.Generic(MainWindow.Localize("deep_vendor", vendor.id), "", IMPORTANCE.WARNING, true, false));
                }
                foreach (var it in vendor.items)
                {
                    if (it.type == ItemType.VEHICLE && !CachedUnturnedFiles.Any(d => d.Type == UnturnedFile.EAssetType.Vehicle && d.Id == it.id))
                    {
                        MainWindow.Instance.lstMistakes.Items.Add(new Mistakes.Generic(MainWindow.Localize("deep_vehicle", it.id), "", IMPORTANCE.WARNING, true, false));
                        continue;
                    }
                    if (it.type == ItemType.ITEM && !CachedUnturnedFiles.Any(d => d.Type == UnturnedFile.EAssetType.Item && d.Id == it.id))
                    {
                        MainWindow.Instance.lstMistakes.Items.Add(new Mistakes.Generic(MainWindow.Localize("deep_item", it.id), "", IMPORTANCE.WARNING, true, false));
                        continue;
                    }
                }
                await Task.Yield();
            }
            foreach (NPCQuest quest in MainWindow.CurrentNPC.quests)
            {
                if (CachedUnturnedFiles != null && CachedUnturnedFiles.Any(d => d.Type == UnturnedFile.EAssetType.Quest && d.Id == quest.id))
                {
                    MainWindow.Instance.lstMistakes.Items.Add(new Generic(MainWindow.Localize("deep_quest", quest.id), "", IMPORTANCE.WARNING, true, false));
                }
                await Task.Yield();
            }
            if (MainWindow.Instance.txtID.Value > 0)
            {
                ushort input = (ushort)MainWindow.Instance.txtID.Value;
                if (CachedUnturnedFiles != null && CachedUnturnedFiles.Any(d => d.Type == UnturnedFile.EAssetType.NPC && d.Id == input))
                {
                    MainWindow.Instance.lstMistakes.Items.Add(new Generic(MainWindow.Localize("deep_char", input), "", IMPORTANCE.WARNING, true, false));
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
        private static HashSet<UnturnedFile> CachedUnturnedFiles { get; set; }
        public class UnturnedFile
        {
            public string FileName { get; set; }
            [XmlIgnore]
            public string Path { get; set; }
            public ushort Id { get; set; }
            public EAssetType Type { get; set; }
            public enum EAssetType
            {
                None,
                Item,
                Dialogue,
                Vendor,
                Animal,
                NPC,
                Quest,
                Vehicle
            }
            public enum EItemType
            {
                HAT,
                PANTS,
                SHIRT,
                MASK,
                BACKPACK,
                VEST,
                GLASSES,
                GUN,
                SIGHT,
                TACTICAL,
                GRIP,
                BARREL,
                MAGAZINE,
                FOOD,
                WATER,
                MEDICAL,
                MELEE,
                FUEL,
                TOOL,
                BARRICADE,
                STORAGE,
                BEACON,
                FARM,
                TRAP,
                STRUCTURE,
                SUPPLY,
                THROWABLE,
                GROWER,
                OPTIC,
                REFILL,
                FISHER,
                CLOUD,
                MAP,
                KEY,
                BOX,
                ARREST_START,
                ARREST_END,
                TANK,
                GENERATOR,
                DETONATOR,
                CHARGE,
                LIBRARY,
                FILTER,
                SENTRY,
                VEHICLE_REPAIR_TOOL,
                TIRE,
                COMPASS,
                OIL_PUMP
            }
        }
        public static async Task<bool> CacheFiles(string directory)
        {
            HashSet<UnturnedFile> cache = new HashSet<UnturnedFile>();
            IEnumerable<FileInfo> validFiles = new DirectoryInfo(directory).GetFiles("*.dat", SearchOption.AllDirectories);
            Logger.Log($"Found {validFiles.Count()} assets!");
            long oldTotal = validFiles.Count();
            validFiles = validFiles.Where(d => d.Name != "English.dat" && d.Name != "Russian.dat");
            MainWindow.NotificationManager.Notify($"Skipped {oldTotal - validFiles.Count()} files.");
            long step = 1;
            MainWindow.Instance.progrBar.Maximum = validFiles.Count();
            foreach (FileInfo fi in validFiles)
            {
                step++;
                try
                {
                    bool typeFound = false;
                    UnturnedFile.EAssetType assetType = UnturnedFile.EAssetType.None;
                    bool idFound = false;
                    bool skip = false;
                    ushort id = 0;
                    UnturnedFile unturnedFile = new UnturnedFile() { FileName = fi.Name, Path = fi.FullName };
                    using (StreamReader sr = new StreamReader(fi.FullName))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (line?.ToLower().StartsWith("name ") == true || line?.ToLower().StartsWith("description ") == true)
                            {
                                skip = true;
                                break;
                            }
                            if (!typeFound && line?.ToLower().StartsWith("type ") == true && line.Split(' ')?.Length >= 2)
                            {
                                if (Enum.TryParse(line.Substring(line.IndexOf(' ') + 1), out UnturnedFile.EAssetType type))
                                {
                                    assetType = type;
                                    typeFound = true;
                                }
                                else if (Enum.TryParse(line.Substring(line.IndexOf(' ') + 1).ToUpper(), out UnturnedFile.EItemType itemType))
                                {
                                    assetType = UnturnedFile.EAssetType.Item;
                                    typeFound = true;
                                }
                            }
                            if (!idFound && line?.ToLower().StartsWith("id ") == true && line.Split(' ')?.Length == 2)
                            {
                                if (ushort.TryParse(line.Split(' ')[1], out ushort resultedId))
                                {
                                    id = resultedId;
                                    idFound = true;
                                }
                            }
                        }
                    }
                    if (idFound)
                        unturnedFile.Id = id;
                    if (typeFound)
                        unturnedFile.Type = assetType;
                    if (!skip && assetType != UnturnedFile.EAssetType.None)
                        cache.Add(unturnedFile);
                }
                catch { }
                await Task.Yield();
                if (step % 25 == 0)
                    await Task.Delay(1);
                MainWindow.Instance.progrBar.Value = step;
            }
            CachedUnturnedFiles = cache;
            Logger.Log($"Cached {cache.Count} files!");
            return true;
        }
    }
}
