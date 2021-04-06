using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCClothing : IAXData
    {
        [XmlElement("hat")]
        public ushort Hat
        {
            get => hat;
            set => hat = value;
        }
        [XmlElement("mask")]
        public ushort Mask
        {
            get => mask;
            set => mask = value;
        }
        [XmlElement("top")]
        public ushort Shirt
        {
            get => top;
            set => top = value;
        }
        [XmlElement("bottom")]
        public ushort Pants
        {
            get => bottom;
            set => bottom = value;
        }
        [XmlElement("backpack")]
        public ushort Backpack
        {
            get => backpack;
            set => backpack = value;
        }
        [XmlElement("vest")]
        public ushort Vest
        {
            get => vest;
            set => vest = value;
        }
        [XmlElement("glasses")]
        public ushort Glasses
        {
            get => glasses;
            set => glasses = value;
        }
        private ushort
            hat,
            mask,
            top,
            bottom,
            backpack,
            vest,
            glasses;

        public NPCClothing()
        {

        }
        public bool IsEmpty => Hat == 0 && Backpack == 0 && Mask == 0 && Vest == 0 && Shirt == 0 && Glasses == 0 && Pants == 0;

        public bool IsHairVisible
        {
            get
            {
                if (Hat > 0 && GameAssetManager.TryGetAsset<GameItemHatAsset>(Hat, out var hasset))
                {
                    if (!hasset.hairVisible)
                    {
                        return false;
                    }
                }

                if (Backpack > 0 && GameAssetManager.TryGetAsset<GameItemBackpackAsset>(Backpack, out var basset))
                {
                    if (!basset.hairVisible)
                    {
                        return false;
                    }
                }

                if (Mask > 0 && GameAssetManager.TryGetAsset<GameItemMaskAsset>(Mask, out var masset))
                {
                    if (!masset.hairVisible)
                    {
                        return false;
                    }
                }

                if (Vest > 0 && GameAssetManager.TryGetAsset<GameItemVestAsset>(Vest, out var vasset))
                {
                    if (!vasset.hairVisible)
                    {
                        return false;
                    }
                }

                if (Shirt > 0 && GameAssetManager.TryGetAsset<GameItemShirtAsset>(Shirt, out var sasset))
                {
                    if (!sasset.hairVisible)
                    {
                        return false;
                    }
                }

                if (Glasses > 0 && GameAssetManager.TryGetAsset<GameItemGlassesAsset>(Glasses, out var gasset))
                {
                    if (!gasset.hairVisible)
                    {
                        return false;
                    }
                }

                if (Pants > 0 && GameAssetManager.TryGetAsset<GameItemPantsAsset>(Pants, out var passet))
                {
                    if (!passet.hairVisible)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        public bool IsBeardVisible
        {
            get
            {
                if (Hat > 0 && GameAssetManager.TryGetAsset<GameItemHatAsset>(Hat, out var hasset))
                {
                    if (!hasset.beardVisible)
                    {
                        return false;
                    }
                }

                if (Backpack > 0 && GameAssetManager.TryGetAsset<GameItemBackpackAsset>(Backpack, out var basset))
                {
                    if (!basset.beardVisible)
                    {
                        return false;
                    }
                }

                if (Mask > 0 && GameAssetManager.TryGetAsset<GameItemMaskAsset>(Mask, out var masset))
                {
                    if (!masset.beardVisible)
                    {
                        return false;
                    }
                }

                if (Vest > 0 && GameAssetManager.TryGetAsset<GameItemVestAsset>(Vest, out var vasset))
                {
                    if (!vasset.beardVisible)
                    {
                        return false;
                    }
                }

                if (Shirt > 0 && GameAssetManager.TryGetAsset<GameItemShirtAsset>(Shirt, out var sasset))
                {
                    if (!sasset.beardVisible)
                    {
                        return false;
                    }
                }

                if (Glasses > 0 && GameAssetManager.TryGetAsset<GameItemGlassesAsset>(Glasses, out var gasset))
                {
                    if (!gasset.beardVisible)
                    {
                        return false;
                    }
                }

                if (Pants > 0 && GameAssetManager.TryGetAsset<GameItemPantsAsset>(Pants, out var passet))
                {
                    if (!passet.beardVisible)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void Load(XmlNode node, int version)
        {
            hat = node["hat"].ToUInt16();
            mask = node["mask"].ToUInt16();
            top = node["top"].ToUInt16();
            bottom = node["bottom"].ToUInt16();
            backpack = node["backpack"].ToUInt16();
            vest = node["vest"].ToUInt16();
            glasses = node["glasses"].ToUInt16();
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("hat", node).WriteUInt16(hat);
            document.CreateNodeC("mask", node).WriteUInt16(mask);
            document.CreateNodeC("top", node).WriteUInt16(top);
            document.CreateNodeC("bottom", node).WriteUInt16(bottom);
            document.CreateNodeC("backpack", node).WriteUInt16(backpack);
            document.CreateNodeC("vest", node).WriteUInt16(vest);
            document.CreateNodeC("glasses", node).WriteUInt16(glasses);
        }
    }
}
