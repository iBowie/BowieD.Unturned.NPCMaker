using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public static class GameAssetManager
    {
        private static readonly List<GameAsset> _assets = new List<GameAsset>();
        
        public static IEnumerable<T> GetAssets<T>() where T : GameAsset
        {
            foreach (var ga in _assets)
            {
                if (ga is T gat)
                {
                    yield return gat;
                }
            }
        }
        public static bool TryGetAsset<T>(Func<T, bool> func, out T result) where T : GameAsset
        {
            foreach (var ga in _assets)
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
        public static async Task Import(string directory, Action<int, int> fileLoadedCallback = null)
        {
            Queue<FileInfo> files = new Queue<FileInfo>();
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
                files.Enqueue(file);
            }

            int index = -1;
            int total = files.Count;

            foreach (var fi in files)
            {
                index++;
                try
                {
                    var res = await TryReadLegacyAssetFile(fi.FullName);
                    if (res.Item1)
                    {
                        _assets.Add(res.Item2);
                    }
                }
                catch (Exception ex)
                {
                    await App.Logger.LogException($"Could not import asset '{fi.FullName}'", ex: ex);
                }
                fileLoadedCallback?.Invoke(index, total);
            }
        }
        public static void Purge()
        {
            _assets.Clear();
        }

        private static async Task<Tuple<bool, GameAsset>> TryReadLegacyAssetFile(string fileName)
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

                name = local.ReadString("Name", dir);
            }
            else
            {
                name = dir;
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
                case "item": case "hat": case "pants":
                case "shirt": case "mask": case "backpack":
                case "vest": case "glasses": case "gun": case "sight":
                case "tactical": case "grip": case "barrel":
                case "magazine": case "food":
                case "water": case "medical":
                case "melee": case "fuel":
                case "tool": case "barricade":
                case "storage": case "beacon":
                case "farm": case "trap":
                case "structure": case "supply":
                case "throwable": case "grower":
                case "optic": case "refill":
                case "fisher": case "cloud":
                case "map": case "key":
                case "box": case "arrest_start":
                case "arrest_end": case "tank":
                case "generator": case "detonator":
                case "charge": case "library":
                case "filter": case "sentry":
                case "vehicle_repair_tool": case "tire":
                case "compass": case "oil_pump":
                    return new Tuple<bool, GameAsset>(true, new GameItemAsset(dr, new DirectoryInfo(dir).Name, name, id, guid, vt));
                case "npc":
                    return new Tuple<bool, GameAsset>(true, new GameNPCAsset(dr, local, name, id, guid, vt));
                case "dialogue":
                    return new Tuple<bool, GameAsset>(true, new GameDialogueAsset(dr, local, name, id, guid, vt));
                case "quest":
                    return new Tuple<bool, GameAsset>(true, new GameQuestAsset(dr, local, name, id, guid, vt));
                case "vendor":
                    return new Tuple<bool, GameAsset>(true, new GameVendorAsset(dr, local, name, id, guid, vt));
                case "vehicle":
                    return new Tuple<bool, GameAsset>(true, new GameVehicleAsset(dr, name, id, guid, vt));
                default:
                    return new Tuple<bool, GameAsset>(true, new GameAsset(name, id, guid, vt));
            }
        }
    }
}
