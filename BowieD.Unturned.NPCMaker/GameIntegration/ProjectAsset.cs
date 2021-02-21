using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class ProjectAsset : GameAsset
    {
        public ProjectAsset() : base(Guid.Empty, EGameAssetOrigin.Project)
        {
        }

        public ProjectAsset(string name, ushort id, string type) : base(name, id, Guid.Empty, type, EGameAssetOrigin.Project)
        {
        }
    }
}
