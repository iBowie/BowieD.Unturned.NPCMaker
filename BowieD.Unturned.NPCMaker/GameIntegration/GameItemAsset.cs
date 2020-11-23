using BowieD.Unturned.NPCMaker.Common.Utility;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.IO;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameItemAsset : GameAsset
    {
        public static readonly Uri DefaultImagePath = new Uri("pack://application:,,,/Resources/Icons/unknown.png");

        public GameItemAsset(DataReader data, string dirName, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {
            this.dirName = dirName;
        }

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
    }
}
