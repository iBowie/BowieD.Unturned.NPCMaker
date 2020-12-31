using MahApps.Metro.IconPacks;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Shared.Attributes
{
    public class AssetPickerAttribute : Attribute
    {
        public Type AssetType { get; }
        public string Key { get; }
        public PackIconMaterialKind Icon { get; }
        public AssetPickerAttribute(Type assetType, string key, PackIconMaterialKind icon)
        {
            AssetType = assetType;
            this.Key = key;
            this.Icon = icon;
        }
    }
}
