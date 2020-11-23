using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes
{
    public class RewardAssetPickerAttribute : Attribute
    {
        public Type AssetType { get; private set; }
        public RewardAssetPickerAttribute(Type assetType)
        {
            AssetType = assetType;
        }
    }
}
