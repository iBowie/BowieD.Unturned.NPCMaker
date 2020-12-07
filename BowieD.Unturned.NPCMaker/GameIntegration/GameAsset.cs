using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Collections.Generic;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameAsset : IFileReadable, IHasTextToolTip
    {
        public GameAsset(string name, ushort id, Guid guid, string type, EGameAssetOrigin origin)
        {
            this.name = name;
            this.id = id;
            this.guid = guid;
            this.type = type;
            this.origin = origin;
        }
        public GameAsset(Guid guid, EGameAssetOrigin origin)
        {
            this.name = guid.ToString("N");
            this.id = 0;
            this.type = string.Empty;
            this.guid = guid;
            this.origin = origin;
        }

        public string name;
        public ushort id;
        public Guid guid;
        public string type;
        public EGameAssetOrigin origin;

        public virtual EGameAssetCategory Category => EGameAssetCategory.NONE;

        public virtual void read(IFileReader reader)
        {
            if (reader != null)
            {
                reader = reader.readObject();
                readAsset(reader);
            }
        }
        protected virtual void readAsset(IFileReader reader)
        {
            id = reader.readValue<ushort>("ID");
        }

        public virtual IEnumerable<string> GetToolTipLines()
        {
            string originLoc = LocalizationManager.Current.Interface[$"AssetPicker_Filter_Origin_{origin}"];

            yield return LocalizationManager.Current.Interface.Translate("AssetPicker_ToolTip_GameAsset_Origin", originLoc);
        }
    }
}
