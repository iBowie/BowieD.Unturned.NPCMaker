using MahApps.Metro.IconPacks;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class ConditionAssetPickerAttribute : Attribute
    {
        public Type AssetType { get; }
        public string Key { get; }
        public PackIconMaterialKind Icon { get; }
        public ConditionAssetPickerAttribute(Type assetType, string key, PackIconMaterialKind icon)
        {
            AssetType = assetType;
            this.Key = key;
            this.Icon = icon;
        }
    }
}
