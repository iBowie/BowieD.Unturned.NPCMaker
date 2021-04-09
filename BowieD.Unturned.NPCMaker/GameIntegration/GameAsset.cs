using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public interface IAssetPickable
    {
        ushort ID { get; }
        Guid GUID { get; }
        string Name { get; }
        EGameAssetOrigin Origin { get; }

        EIDDef IDDef {get;}
    }
    public interface IHasOriginFile : IAssetPickable
    {
        string OriginFileName { get; }
    }
    public enum EIDDef
    {
        NONE = 0,
        ID = 1 << 0,
        GUID = 1 << 1,
        FILEORIGIN = 1 << 2,
        FILEORIGIN_DIR = 1 << 3,
        FILEORIGIN_DIR_SHORT = 1 << 4
    }
    public class GameAsset : IFileReadable, IHasTextToolTip, IAssetPickable
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
        public GameAsset(EGameAssetOrigin origin)
        {
            this.name = guid.ToString("N");
            this.id = 0;
            this.type = string.Empty;
            this.guid = Guid.NewGuid();
            this.origin = origin;
        }

        public string name;
        public ushort id;
        public Guid guid;
        [XmlIgnore]
        public string type;
        [XmlIgnore]
        public EGameAssetOrigin origin;

        public virtual ushort ID => id;
        public virtual Guid GUID => guid;
        public virtual string Name => name;
        public virtual EIDDef IDDef => EIDDef.ID;
        public virtual EGameAssetOrigin Origin => origin;

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
