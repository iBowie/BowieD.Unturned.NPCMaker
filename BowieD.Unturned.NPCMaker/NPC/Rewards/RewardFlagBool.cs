﻿using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardFlagBool : Reward
    {
        public override RewardType Type => RewardType.Flag_Bool;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Flag_Bool"]} [{ID}] -> {Value}";
        [AssetPicker(typeof(FlagDescriptionProjectAsset), "Control_SelectAsset_Project_Flag", MahApps.Metro.IconPacks.PackIconMaterialKind.Flag)]
        public ushort ID { get; set; }
        public bool Value { get; set; }

        public override void Give(Simulation simulation)
        {
            simulation.Flags[ID] = (short)(Value ? 1 : 0);
        }
    }
}
