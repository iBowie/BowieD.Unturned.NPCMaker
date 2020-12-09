using BowieD.Unturned.NPCMaker.Common.Utility;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Collections.Generic;
using System.IO;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameItemAsset : GameAsset, IHasIcon
    {
        public static readonly Uri DefaultImagePath = new Uri("pack://application:,,,/Resources/Icons/unknown.png");

        public GameItemAsset(DataReader data, string dirName, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {
            this.dirName = dirName;

            isPro = data.Has("Pro");
            rarity = data.ReadEnum("Rarity", EGameItemRarity.Common);
            sizeX = data.ReadByte("Size_X", 0);
            if (sizeX < 1)
                sizeX = 1;
            sizeY = data.ReadByte("Size_Y", 0);
            if (sizeY < 1)
                sizeY = 1;

            useable = data.ReadString("Useable");
            canPlayerEquip = data.ReadBoolean("Can_Player_Equip", !string.IsNullOrEmpty(useable));

            slot = data.ReadEnum("Slot", Equip_Type.None);
        }

        public readonly bool isPro;
        public readonly EGameItemRarity rarity;
        public readonly byte sizeX, sizeY;
        public readonly bool canPlayerEquip;
        public readonly string useable;
        public readonly Equip_Type slot;

        private string dirName;

        public Uri ImagePath
        {
            get
            {
                var fallback = DefaultImagePath;

                var uDir = AppConfig.Instance.unturnedDir;
                if (string.IsNullOrEmpty(uDir) || !Directory.Exists(uDir) || !PathUtility.IsUnturnedPath(uDir))
                {
                    return fallback;
                }
                else
                {
                    string fName = Path.Combine(uDir, "Extras", "Icons", $"{dirName}_{id}.png");

                    if (File.Exists(fName))
                    {
                        return new Uri(Path.GetFullPath(fName));
                    }
                    else
                    {
                        return fallback;
                    }
                }
            }
        }

        public override EGameAssetCategory Category => EGameAssetCategory.ITEM;

        public override IEnumerable<string> GetToolTipLines()
        {
            foreach (var b in base.GetToolTipLines())
                yield return b;

            if (isPro)
                yield return LocalizationManager.Current.Interface["AssetPicker_ToolTip_GameItemAsset_Pro"];

            yield return LocalizationManager.Current.Interface.Translate("AssetPicker_ToolTip_GameItemAsset_Rarity", LocalizationManager.Current.Interface[$"AssetPicker_ToolTip_GameItemAsset_Rarity_{rarity}"]);

            yield return LocalizationManager.Current.Interface.Translate("AssetPicker_ToolTip_GameItemAsset_SizeX", sizeX);
            yield return LocalizationManager.Current.Interface.Translate("AssetPicker_ToolTip_GameItemAsset_SizeY", sizeY);
        }
    }
}
