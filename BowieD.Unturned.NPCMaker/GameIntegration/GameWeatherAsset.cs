using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameWeatherAsset : GameAsset
    {
        public GameWeatherAsset(string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {

        }
        public GameWeatherAsset(Guid guid, EGameAssetOrigin origin) : base(guid, origin)
        {

        }

        public override EIDDef IDDef => EIDDef.GUID;
    }
}
