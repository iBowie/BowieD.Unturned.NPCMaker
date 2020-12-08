using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameItemGunAsset : GameItemWeaponAsset
    {
        public GameItemGunAsset(DataReader data, string dirName, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(data, dirName, name, id, guid, type, origin)
        {
            hasSight = data.Has("Hook_Sight");
            hasTactical = data.Has("Hook_Tactical");
            hasGrip = data.Has("Hook_Grip");
            hasBarrel = data.Has("Hook_Barrel");
        }

        public bool hasSight, hasTactical, hasGrip, hasBarrel;
    }
}
