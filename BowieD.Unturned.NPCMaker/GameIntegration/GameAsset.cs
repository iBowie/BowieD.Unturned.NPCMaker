using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameAsset
    {
        public GameAsset(string name, ushort id, Guid guid, string type, EGameAssetOrigin origin)
        {
            this.name = name;
            this.id = id;
            this.guid = guid;
            this.type = type;
            this.origin = origin;
        }

        public string name;
        public ushort id;
        public Guid guid;
        public string type;
        public EGameAssetOrigin origin;
    }
}
