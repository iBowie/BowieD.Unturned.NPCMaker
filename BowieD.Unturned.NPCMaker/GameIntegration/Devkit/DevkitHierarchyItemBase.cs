using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Devkit
{
    public abstract class DevkitHierarchyItemBase : IDevkitHierarchyItem
    {
        public DevkitHierarchyItemBase(string fileName, EGameAssetOrigin origin)
        {
            this.OriginFileName = fileName;
            this.Origin = origin;
        }

        public EGameAssetOrigin Origin { get; }

        public virtual string Name => string.Empty;
        public virtual ushort ID => 0;
        public virtual EIDDef IDDef => EIDDef.FILEORIGIN_DIR_SHORT;
        public virtual Guid GUID => Guid.Empty;

        public string OriginFileName { get; }

        public abstract void read(IFileReader reader);
    }
}
