using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public static class GameAssetManager
    {
        private static readonly List<GameAsset> _assets = new List<GameAsset>();

        public static bool HasImportedAssets
        {
            get
            {
                return HasImportedVanilla || HasImportedWorkshop;
            }
            set
            {
                if (value)
                    throw new InvalidOperationException("Not expected to do this");

                HasImportedVanilla = false;
                HasImportedWorkshop = false;
            }
        }

        public static bool HasImportedVanilla { get; set; }
        public static bool HasImportedWorkshop { get; set; }

        public static IEnumerable<IHasIcon> GetAllAssetsWithIcons()
        {
            foreach (var asset in GetAllAssets<GameAsset>())
            {
                if (asset is IHasIcon hasIcon)
                {
                    yield return hasIcon;
                }
            }
        }
        public static IEnumerable<GameAsset> GetAllAssets(Type type)
        {
            if (typeof(GameAsset).IsAssignableFrom(type))
            {
                foreach (var asset in GetAllAssets<GameAsset>())
                {
                    if (type.IsAssignableFrom(asset.GetType()))
                    {
                        yield return asset;
                    }
                }
            }
            else
                throw new ArgumentException("Expected derived class from GameAsset");
        }
        public static IEnumerable<T> GetAllAssets<T>() where T : GameAsset
        {
            foreach (var ga in _assets)
            {
                if (ga is T gat)
                {
                    yield return gat;
                }
            }

            if (typeof(T).IsAssignableFrom(typeof(GameNPCAsset)))
            {
                foreach (var ch in MainWindow.CurrentProject.data.characters)
                {
                    yield return new GameNPCAsset(ch, EGameAssetOrigin.Project) as T;
                }
            }

            if (typeof(T).IsAssignableFrom(typeof(GameDialogueAsset)))
            {
                foreach (var ch in MainWindow.CurrentProject.data.dialogues)
                {
                    yield return new GameDialogueAsset(ch, EGameAssetOrigin.Project) as T;
                }
            }

            if (typeof(T).IsAssignableFrom(typeof(GameVendorAsset)))
            {
                foreach (var ch in MainWindow.CurrentProject.data.vendors)
                {
                    yield return new GameVendorAsset(ch, EGameAssetOrigin.Project) as T;
                }
            }

            if (typeof(T).IsAssignableFrom(typeof(GameQuestAsset)))
            {
                foreach (var ch in MainWindow.CurrentProject.data.quests)
                {
                    yield return new GameQuestAsset(ch, EGameAssetOrigin.Project) as T;
                }
            }

            if (typeof(T).IsAssignableFrom(typeof(GameCurrencyAsset)))
            {
                foreach (var ch in MainWindow.CurrentProject.data.currencies)
                {
                    yield return new GameCurrencyAsset(ch, EGameAssetOrigin.Project) as T;
                }
            }
        }
        public static bool TryGetAsset<T>(Func<T, bool> func, out T result) where T : GameAsset
        {
            foreach (var ga in GetAllAssets<T>())
            {
                if (ga is T gat)
                {
                    if (func.Invoke(gat))
                    {
                        result = gat;
                        return true;
                    }
                }
            }

            result = default;
            return false;
        }
        public static bool TryGetAsset<T>(Guid guid, out T result) where T : GameAsset
        {
            return TryGetAsset((k) => k.guid == guid, out result);
        }
        public static bool TryGetAsset<T>(ushort id, out T result) where T : GameAsset
        {
            return TryGetAsset((k) => k.id == id, out result);
        }
        public static bool TryGetAsset<T>(Guid guid, EGameAssetOrigin origin, out T result) where T : GameAsset
        {
            return TryGetAsset((k) => k.guid == guid && origin.HasFlag(k.origin), out result);
        }
        public static bool TryGetAsset<T>(ushort id, EGameAssetOrigin origin, out T result) where T : GameAsset
        {
            return TryGetAsset((k) => k.id == id && origin.HasFlag(k.origin), out result);
        }
        public static async Task Import(string directory, EGameAssetOrigin origin, Action<int, int> fileLoadedCallback = null, CancellationTokenSource tokenSource = null)
        {
            Queue<ScannedFileInfo> files = new Queue<ScannedFileInfo>();

            HashSet<string> ignoreFileNames = new HashSet<string>();
            foreach (object langValue in Enum.GetValues(typeof(ELanguage)))
            {
                ignoreFileNames.Add(langValue + ".dat");
            }

            foreach (FileInfo file in new DirectoryInfo(directory).GetFiles("*.dat", SearchOption.AllDirectories))
            {
                if (ignoreFileNames.Contains(file.Name))
                {
                    continue;
                }
                files.Enqueue(new LegacyFileInfo(file));
            }

            foreach (FileInfo file in new DirectoryInfo(directory).GetFiles("*.asset", SearchOption.AllDirectories))
            {
                files.Enqueue(new AssetFileInfo(file));
            }

            int index = -1;
            int total = files.Count;

            foreach (var fi in files)
            {
                if (tokenSource != null && tokenSource.IsCancellationRequested)
                {
                    await App.Logger.Log("Cancelled import", Logging.ELogLevel.TRACE);
                    break;
                }

                index++;
                try
                {
                    if (fi is LegacyFileInfo)
                    {
                        var res = await TryReadLegacyAssetFile(fi.Info.FullName, origin);
                        if (res.Item1)
                        {
                            _assets.Add(res.Item2);
                        }
                    }
                    else if (fi is AssetFileInfo)
                    {
                        var res = TryReadAssetFile(fi.Info.FullName, origin);
                        if (res.Item1)
                        {
                            _assets.Add(res.Item2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    await App.Logger.LogException($"Could not import asset '{fi.Info.FullName}'", ex: ex);
                }
                fileLoadedCallback?.Invoke(index, total);
            }
        }

        public static bool TryFindUnusedID(EGameAssetCategory category, out ushort id)
        {
            for (ushort res = 2000; res < ushort.MaxValue; res++)
            {
                if (TryGetAsset<GameAsset>((asset) => asset.id == res && asset.Category == category, out _))
                {
                    continue;
                }

                id = res;
                return true;
            }

            id = 0;
            return false;
        }

        private abstract class ScannedFileInfo
        {
            public ScannedFileInfo(FileInfo info)
            {
                this.Info = info;
            }

            public FileInfo Info { get; }
        }
        private class LegacyFileInfo : ScannedFileInfo
        {
            public LegacyFileInfo(FileInfo info) : base(info)
            {
            }
        }
        private class AssetFileInfo : ScannedFileInfo
        {
            public AssetFileInfo(FileInfo info) : base(info)
            {
            }
        }

        public static void Purge()
        {
            _assets.Clear();
            HasImportedAssets = false;
        }

        private static Tuple<bool, GameAsset> TryReadAssetFile(string fileName, EGameAssetOrigin origin)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                IFileReader formattedFileReader = null;
                try
                {
                    formattedFileReader = new KVTableReader(sr);
                }
                catch
                {
                    return new Tuple<bool, GameAsset>(false, null);
                }
                IFileReader formattedFileReader2 = formattedFileReader.readObject("Metadata");
                Guid gUID = formattedFileReader2.readValue<Guid>("GUID");
                Type type = formattedFileReader2.readValue<Type>("Type");
                if (type == null)
                {
                    return new Tuple<bool, GameAsset>(false, null);
                }
                try
                {
                    GameAsset asset = Activator.CreateInstance(type, gUID, origin) as GameAsset;
                    if (asset != null)
                    {
                        asset.guid = gUID;
                        formattedFileReader.readKey("Asset");
                        asset.read(formattedFileReader);
                        return new Tuple<bool, GameAsset>(true, asset);
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"Could not create instance of {type.Name}", ex: ex);
                }
            }

            return new Tuple<bool, GameAsset>(false, null);
        }
        private static async Task<Tuple<bool, GameAsset>> TryReadLegacyAssetFile(string fileName, EGameAssetOrigin origin)
        {
            string drContent;

            using (StreamReader sr = new StreamReader(fileName))
            {
                drContent = await sr.ReadToEndAsync();
            }

            DataReader dr = new DataReader(drContent);

            DataReader local = null;

            var t = dr.ReadString("Type");

            if (string.IsNullOrEmpty(t))
            {
                return new Tuple<bool, GameAsset>(false, null);
            }

            string dir = Path.GetDirectoryName(fileName);
            string shortDir = new DirectoryInfo(dir).Name;

            string name;

            string localPath = Path.Combine(dir, "English.dat");
            if (File.Exists(localPath))
            {
                string locContent;

                using (StreamReader sr = new StreamReader(localPath))
                {
                    locContent = await sr.ReadToEndAsync();
                }

                local = new DataReader(locContent);

                name = local.ReadString("Name", shortDir);
            }
            else
            {
                name = shortDir;
            }

            if (!dr.Has("ID") || !dr.Has("GUID"))
            {
                return new Tuple<bool, GameAsset>(false, null);
            }

            ushort id = dr.ReadUInt16("ID");
            Guid guid = dr.ReadGUID("GUID");

            var vt = t.ToLowerInvariant();

            switch (vt)
            {
                case "item":
                case "food":
                case "water":
                case "medical":
                case "melee":
                case "fuel":
                case "tool":
                case "barricade":
                case "storage":
                case "beacon":
                case "farm":
                case "trap":
                case "structure":
                case "supply":
                case "throwable":
                case "grower":
                case "optic":
                case "refill":
                case "fisher":
                case "cloud":
                case "map":
                case "key":
                case "box":
                case "arrest_start":
                case "arrest_end":
                case "tank":
                case "generator":
                case "detonator":
                case "charge":
                case "library":
                case "filter":
                case "sentry":
                case "vehicle_repair_tool":
                case "tire":
                case "compass":
                case "oil_pump":
                    return new Tuple<bool, GameAsset>(true, new GameItemAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "hat":
                    return new Tuple<bool, GameAsset>(true, new GameItemHatAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "pants":
                    return new Tuple<bool, GameAsset>(true, new GameItemPantsAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "sight":
                    return new Tuple<bool, GameAsset>(true, new GameItemSightAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "barrel":
                    return new Tuple<bool, GameAsset>(true, new GameItemBarrelAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "tactical":
                    return new Tuple<bool, GameAsset>(true, new GameItemTacticalAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "magazine":
                    return new Tuple<bool, GameAsset>(true, new GameItemMagazineAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "grip":
                    return new Tuple<bool, GameAsset>(true, new GameItemGripAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "shirt":
                    return new Tuple<bool, GameAsset>(true, new GameItemShirtAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "glasses":
                    return new Tuple<bool, GameAsset>(true, new GameItemGlassesAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "mask":
                    return new Tuple<bool, GameAsset>(true, new GameItemMaskAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "backpack":
                    return new Tuple<bool, GameAsset>(true, new GameItemBackpackAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "vest":
                    return new Tuple<bool, GameAsset>(true, new GameItemVestAsset(dr, local, shortDir, name, id, guid, vt, origin));
                case "npc":
                    return new Tuple<bool, GameAsset>(true, new GameNPCAsset(dr, local, name, id, guid, vt, origin));
                case "dialogue":
                    return new Tuple<bool, GameAsset>(true, new GameDialogueAsset(dr, local, name, id, guid, vt, origin));
                case "quest":
                    return new Tuple<bool, GameAsset>(true, new GameQuestAsset(dr, local, name, id, guid, vt, origin));
                case "vendor":
                    return new Tuple<bool, GameAsset>(true, new GameVendorAsset(dr, local, name, id, guid, vt, origin));
                case "vehicle":
                    return new Tuple<bool, GameAsset>(true, new GameVehicleAsset(dr, name, id, guid, vt, origin));
                case "animal":
                    return new Tuple<bool, GameAsset>(true, new GameAnimalAsset(dr, name, id, guid, vt, origin));
                case "large":
                case "medium":
                case "small":
                    return new Tuple<bool, GameAsset>(true, new GameObjectAsset(dr, name, id, guid, vt, origin));
                case "spawn":
                    return new Tuple<bool, GameAsset>(true, new GameSpawnAsset(dr, name, id, guid, vt, origin));
                case "resource":
                    return new Tuple<bool, GameAsset>(true, new GameResourceAsset(dr, name, id, guid, vt, origin));
                case "gun":
                    return new Tuple<bool, GameAsset>(true, new GameItemGunAsset(dr, local, shortDir, name, id, guid, vt, origin));
                default:
                    return new Tuple<bool, GameAsset>(true, new GameAsset(name, id, guid, vt, origin));
            }
        }
    }
}
