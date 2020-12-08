using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameItemCaliberAsset : GameItemAsset
    {
        public GameItemCaliberAsset(DataReader data, string dirName, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(data, dirName, name, id, guid, type, origin)
        {
            calibers = new ushort[data.ReadByte("Calibers", 0)];

            for (int i = 0; i < calibers.Length; i++)
            {
                calibers[i] = data.ReadUInt16($"Caliber_{i}", 0);
            }
        }

        public readonly ushort[] calibers;
    }
}
