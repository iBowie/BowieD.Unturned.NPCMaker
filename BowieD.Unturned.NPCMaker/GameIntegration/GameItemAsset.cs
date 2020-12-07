using BowieD.Unturned.NPCMaker.Common.Utility;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Localization;
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
        }

        public readonly bool isPro;

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
        }
    }
}
