using BowieD.Unturned.NPCMaker.NPC;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Filtering
{
    public sealed class AssetFilter_Equippable : AssetFilter
    {
        public AssetFilter_Equippable(string dict, string translationKey, Equip_Type slot) : base(dict, translationKey)
        {
            IsEnabled = true;

            this.slot = slot;
        }

        private readonly Equip_Type slot;

        public override bool ShouldDisplay(GameAsset asset)
        {
            if (asset is GameItemAsset gia)
            {
                if (gia.canPlayerEquip)
                {
                    switch (slot)
                    {
                        case Equip_Type.Primary:
                            {
                                if (gia.slot == Equip_Type.Primary || gia.slot == Equip_Type.Secondary || gia.slot == Equip_Type.Any)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case Equip_Type.Secondary:
                            {
                                if (gia.slot == Equip_Type.Secondary || gia.slot == Equip_Type.Any)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case Equip_Type.Tertiary:
                            {
                                if (gia.slot == Equip_Type.None || gia.slot == Equip_Type.Tertiary || gia.slot == Equip_Type.Any)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        default:
                            return false;

                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
