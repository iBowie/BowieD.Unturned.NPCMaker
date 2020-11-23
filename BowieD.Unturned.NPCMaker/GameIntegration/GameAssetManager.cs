using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Collections.Generic;
using System.IO;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public static class GameAssetManager
    {
        private static readonly List<GameAsset> _assets = new List<GameAsset>();
        
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
        public static void Import(string directory)
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

            foreach (var fi in files)
            {
                if (TryReadFile(fi.FullName, out var asset))
                {
                    _assets.Add(asset);
                }
            }
        }
        public static void Purge()
        {
            _assets.Clear();
        }

        private static bool TryReadFile(string fileName, out GameAsset result)
        {
            DataReader dr = new DataReader(File.ReadAllText(fileName));

            var t = dr.ReadString("Type");

            if (string.IsNullOrEmpty(t))
            {
                result = null;
                return false;
            }

            string dir = Path.GetDirectoryName(fileName);

            string name;

            string localPath = Path.Combine(dir, "English.dat");
            if (File.Exists(localPath))
            {
                DataReader local = new DataReader(File.ReadAllText(localPath));

                name = local.ReadString("Name", dir);
            }
            else
            {
                name = dir;
            }

            if (!dr.Has("ID") || !dr.Has("GUID"))
            {
                result = null;
                return false;
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
                    result = new GameItemAsset(dr, name, id, guid, vt);
                    return true;
                case "npc":
                    result = new GameNPCAsset(dr, name, id, guid, vt);
                    return true;
                case "dialogue":
                    result = new GameDialogueAsset(dr, name, id, guid, vt);
                    return true;
                case "quest":
                    result = new GameQuestAsset(dr, name, id, guid, vt);
                    return true;
                case "vendor":
                    result = new GameVendorAsset(dr, name, id, guid, vt);
                    return true;
                case "vehicle":
                    result = new GameVehicleAsset(dr, name, id, guid, vt);
                    return true;
                default:
                    result = new GameAsset(dr, name, id, guid, vt);
                    return true;
            }
        }
    }
}
