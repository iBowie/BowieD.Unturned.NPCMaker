using BowieD.Unturned.NPCMaker.Parsing;
using BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.UnityTypes;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Devkit
{
    public class DevkitHierarchyWorldItem : DevkitHierarchyItemBase
    {
        public DevkitHierarchyWorldItem(string fileName, EGameAssetOrigin origin) : base(fileName, origin)
        {
        }

        public Vector3 Position { get; protected set; }

        public override void read(IFileReader reader)
        {
            reader = reader.readObject();
            if (reader != null)
                readHierarchyItem(reader);
        }
        protected virtual void readHierarchyItem(IFileReader reader) 
        {
            Position = reader.readValue<Vector3>("Position");
        }
    }
}
