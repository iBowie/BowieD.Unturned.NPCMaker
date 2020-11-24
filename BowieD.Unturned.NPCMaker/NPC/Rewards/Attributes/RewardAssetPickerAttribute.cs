using MahApps.Metro.IconPacks;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes
{
    public class RewardAssetPickerAttribute : Attribute
    {
        public Type AssetType { get; }
        public string Key { get; }
        public PackIconMaterialKind Icon { get; }
        public RewardAssetPickerAttribute(Type assetType, string key, PackIconMaterialKind icon)
        {
            AssetType = assetType;
            this.Key = key;
            this.Icon = icon;
        }
    }
}
