using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameItemTacticalAsset : GameItemCaliberAsset
    {
        public GameItemTacticalAsset(DataReader data, DataReader local, string dirName, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(data, local, dirName, name, id, guid, type, origin)
        {

        }
    }
}
