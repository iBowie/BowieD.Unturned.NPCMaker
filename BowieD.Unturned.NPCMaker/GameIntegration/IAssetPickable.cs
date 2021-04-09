using System;

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
}
