using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameAsset : IFileReadable
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
    }
}
