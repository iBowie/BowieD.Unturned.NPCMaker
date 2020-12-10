using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameItemBagAsset : GameItemClothingAsset
    {
        public GameItemBagAsset(DataReader data, DataReader local, string dirName, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(data, local, dirName, name, id, guid, type, origin)
        {
            width = data.ReadByte("Width", 0);
            height = data.ReadByte("Height", 0);
        }

        public readonly byte width, height;

        public override IEnumerable<string> GetToolTipLines()
        {
            foreach (var l in base.GetToolTipLines())
                yield return l;

            if (!isPro)
            {
                yield return LocalizationManager.Current.Interface.Translate("AssetPicker_ToolTip_GameItemBagAsset_Width", width);
                yield return LocalizationManager.Current.Interface.Translate("AssetPicker_ToolTip_GameItemBagAsset_Height", height);
            }
        }
    }
}
