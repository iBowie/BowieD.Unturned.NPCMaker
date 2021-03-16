using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardCurrency : Reward
    {
        public override RewardType Type => RewardType.Currency;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Currency"]} x{Value}";
        [AssetPicker(typeof(GameCurrencyAsset), "Control_SelectAsset_Currency", MahApps.Metro.IconPacks.PackIconMaterialKind.CurrencyUsd)]
        public string GUID { get; set; }
        public uint Value { get; set; }

        public override void Give(Simulation simulation)
        {
            if (simulation.Currencies.ContainsKey(GUID))
            {
                simulation.Currencies[GUID] += Value;
            }
            else
            {
                simulation.Currencies.Add(GUID, Value);
            }
        }
        public override string FormatReward(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                if (GameAssetManager.TryGetAsset<GameCurrencyAsset>(Guid.Parse(GUID), out var asset) && !string.IsNullOrEmpty(asset.valueFormat))
                {
                    text = asset.valueFormat;
                }
                else
                {
                    text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Reward_Currency");
                }
            }

            return string.Format(text, Value);
        }
    }
}
