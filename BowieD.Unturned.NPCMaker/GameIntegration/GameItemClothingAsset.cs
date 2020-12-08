using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameItemClothingAsset : GameItemAsset
    {
        public GameItemClothingAsset(DataReader data, string dirName, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(data, dirName, name, id, guid, type, origin)
        {
            if (isPro)
            {
                armor = 1f;
                explosionArmor = 1f;
            }
            else
            {
                armor = data.ReadSingle("Armor", 1f);
                explosionArmor = data.ReadSingle("Armor_Explosion", armor);
                proofWater = data.Has("Proof_Water");
                proofFire = data.Has("Proof_Fire");
            }

            hairVisible = data.ReadBoolean("Hair_Visible", true);
            beardVisible = data.ReadBoolean("Beard_Visible", true);
        }

        public float armor, explosionArmor;
        public bool proofWater, proofFire, hairVisible, beardVisible;

        public override IEnumerable<string> GetToolTipLines()
        {
            foreach (var l in base.GetToolTipLines())
                yield return l;

            yield return LocalizationManager.Current.Interface.Translate("AssetPicker_ToolTip_GameItemClothingAsset_Armor", armor);
            yield return LocalizationManager.Current.Interface.Translate("AssetPicker_ToolTip_GameItemClothingAsset_Armor_Explosion", explosionArmor);
            if (proofWater)
                yield return LocalizationManager.Current.Interface["AssetPicker_ToolTip_GameItemClothingAsset_Proof_Water"];
            if (proofFire)
                yield return LocalizationManager.Current.Interface["AssetPicker_ToolTip_GameItemClothingAsset_Proof_Fire"];
            if (!hairVisible)
                yield return LocalizationManager.Current.Interface["AssetPicker_ToolTip_GameItemClothingAsset_Hair_Invisible"];
            if (!beardVisible)
                yield return LocalizationManager.Current.Interface["AssetPicker_ToolTip_GameItemClothingAsset_Beard_Invisible"];
        }
    }
}
