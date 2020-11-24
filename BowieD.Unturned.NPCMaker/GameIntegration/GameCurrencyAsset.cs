using BowieD.Unturned.NPCMaker.NPC.Currency;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameCurrencyAsset : GameAsset
    {
        public GameCurrencyAsset(string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {

        }
        public GameCurrencyAsset(CurrencyAsset asset, EGameAssetOrigin origin) : base(asset.GUID, 0, Guid.Parse(asset.GUID), "currency", origin)
        {
            valueFormat = asset.ValueFormat;
        }

        public string valueFormat;
    }
}
