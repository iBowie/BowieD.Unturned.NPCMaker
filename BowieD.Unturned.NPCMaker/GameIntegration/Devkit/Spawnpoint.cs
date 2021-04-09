using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Devkit
{
    public class Spawnpoint : DevkitHierarchyWorldItem
    {
        public string id;

        public Spawnpoint(string fileName, EGameAssetOrigin origin) : base(fileName, origin)
        {
        }

        public override string Name => id;

        protected override void readHierarchyItem(IFileReader reader)
        {
            base.readHierarchyItem(reader);

            id = reader.readValue<string>("ID");
        }
    }
    public class DevkitHierarchyWorldItem : DevkitHierarchyItemBase
    {
        public DevkitHierarchyWorldItem(string fileName, EGameAssetOrigin origin) : base(fileName, origin)
        {
        }

        public override void read(IFileReader reader)
        {
            reader = reader.readObject();
            if (reader != null)
                readHierarchyItem(reader);
        }
        protected virtual void readHierarchyItem(IFileReader reader) { }
    }
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
    public interface IDevkitHierarchyItem : IFileReadable, IAssetPickable, IHasOriginFile { }
}
