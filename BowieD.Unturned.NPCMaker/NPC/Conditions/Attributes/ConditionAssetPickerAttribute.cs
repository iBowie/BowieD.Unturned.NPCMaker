using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class ConditionAssetPickerAttribute : Attribute
    {
        public Type AssetType { get; private set; }
        public ConditionAssetPickerAttribute(Type assetType)
        {
            AssetType = assetType;
        }
    }
}
