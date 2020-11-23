using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameVendorAsset : GameAsset
    {
        public GameVendorAsset(NPCVendor vendor, EGameAssetOrigin origin) : base(vendor.Title, vendor.ID, Guid.Parse(vendor.GUID), "Vendor", origin)
        {
            this.vendor = vendor;
        }
        public GameVendorAsset(DataReader data, DataReader local, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {
            vendor = new Parsing.ParseTool(data, local).ParseVendor();
        }

        public NPCVendor vendor;
    }
}
