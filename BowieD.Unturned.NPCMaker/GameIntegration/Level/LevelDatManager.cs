using System.Collections.Generic;
using System.IO;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Level
{
    public static class LevelDatManager
    {
        private static Dictionary<string, LevelInfo> _cache = new Dictionary<string, LevelInfo>();

        public static LevelInfo GetInfo(string mapDirectory)
        {
            try
            {
                if (!_cache.ContainsKey(mapDirectory))
                {

                    using (FileStream fs = new FileStream(Path.Combine(mapDirectory, "Level.dat"), FileMode.Open, FileAccess.Read))
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        _ = br.ReadByte(); // version
                        _ = br.ReadUInt64(); // author
                        ELevelSize size = (ELevelSize)br.ReadByte();

                        _cache[mapDirectory] = new LevelInfo()
                        {
                            Size = size
                        };
                    }
                }

                return _cache[mapDirectory];
            }
            catch
            {
                return null;
            }
        }
    }

    public class LevelInfo
    {
        public ELevelSize Size { get; set; }
    }

    public enum ELevelSize : byte
    {
        TINY = 0,
        SMALL = 1,
        MEDIUM = 2,
        LARGE = 3,
        INSANE = 4
    }
}
