﻿using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameItemGearAsset : GameItemClothingAsset
    {
        public GameItemGearAsset(DataReader data, string dirName, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(data, dirName, name, id, guid, type, origin)
        {

        }
    }
}
