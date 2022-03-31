using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCClothing : IAXData
    {
        public NPCClothing()
        {
            Hat = new GUIDIDBridge(0);
            Mask = new GUIDIDBridge(0);
            Shirt = new GUIDIDBridge(0);
            Pants = new GUIDIDBridge(0);
            Vest = new GUIDIDBridge(0);
            Glasses = new GUIDIDBridge(0);
            Backpack = new GUIDIDBridge(0);
        }

        [XmlElement("hat")]
        public GUIDIDBridge Hat
        {
            get => hat;
            set => hat = value;
        }
        [XmlElement("mask")]
        public GUIDIDBridge Mask
        {
            get => mask;
            set => mask = value;
        }
        [XmlElement("top")]
        public GUIDIDBridge Shirt
        {
            get => top;
            set => top = value;
        }
        [XmlElement("bottom")]
        public GUIDIDBridge Pants
        {
            get => bottom;
            set => bottom = value;
        }
        [XmlElement("backpack")]
        public GUIDIDBridge Backpack
        {
            get => backpack;
            set => backpack = value;
        }
        [XmlElement("vest")]
        public GUIDIDBridge Vest
        {
            get => vest;
            set => vest = value;
        }
        [XmlElement("glasses")]
        public GUIDIDBridge Glasses
        {
            get => glasses;
            set => glasses = value;
        }
        private GUIDIDBridge
            hat,
            mask,
            top,
            bottom,
            backpack,
            vest,
            glasses;

        public bool IsEmpty => Hat.IsEmpty && Backpack.IsEmpty && Mask.IsEmpty && Vest.IsEmpty && Shirt.IsEmpty && Glasses.IsEmpty && Pants.IsEmpty;

        public bool IsHairVisible
        {
            get
            {
                if (!Hat.IsEmpty && GameAssetManager.TryGetAsset<GameItemHatAsset>(Hat, out var hasset))
                {
                    if (!hasset.hairVisible)
                    {
                        return false;
                    }
                }

                if (!Backpack.IsEmpty && GameAssetManager.TryGetAsset<GameItemBackpackAsset>(Backpack, out var basset))
                {
                    if (!basset.hairVisible)
                    {
                        return false;
                    }
                }

                if (!Mask.IsEmpty && GameAssetManager.TryGetAsset<GameItemMaskAsset>(Mask, out var masset))
                {
                    if (!masset.hairVisible)
                    {
                        return false;
                    }
                }

                if (!Vest.IsEmpty && GameAssetManager.TryGetAsset<GameItemVestAsset>(Vest, out var vasset))
                {
                    if (!vasset.hairVisible)
                    {
                        return false;
                    }
                }

                if (!Shirt.IsEmpty && GameAssetManager.TryGetAsset<GameItemShirtAsset>(Shirt, out var sasset))
                {
                    if (!sasset.hairVisible)
                    {
                        return false;
                    }
                }

                if (!Glasses.IsEmpty && GameAssetManager.TryGetAsset<GameItemGlassesAsset>(Glasses, out var gasset))
                {
                    if (!gasset.hairVisible)
                    {
                        return false;
                    }
                }

                if (!Pants.IsEmpty && GameAssetManager.TryGetAsset<GameItemPantsAsset>(Pants, out var passet))
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
                if (!Hat.IsEmpty && GameAssetManager.TryGetAsset<GameItemHatAsset>(Hat, out var hasset))
                {
                    if (!hasset.beardVisible)
                    {
                        return false;
                    }
                }

                if (!Backpack.IsEmpty && GameAssetManager.TryGetAsset<GameItemBackpackAsset>(Backpack, out var basset))
                {
                    if (!basset.beardVisible)
                    {
                        return false;
                    }
                }

                if (!Mask.IsEmpty && GameAssetManager.TryGetAsset<GameItemMaskAsset>(Mask, out var masset))
                {
                    if (!masset.beardVisible)
                    {
                        return false;
                    }
                }

                if (!Vest.IsEmpty && GameAssetManager.TryGetAsset<GameItemVestAsset>(Vest, out var vasset))
                {
                    if (!vasset.beardVisible)
                    {
                        return false;
                    }
                }

                if (!Shirt.IsEmpty && GameAssetManager.TryGetAsset<GameItemShirtAsset>(Shirt, out var sasset))
                {
                    if (!sasset.beardVisible)
                    {
                        return false;
                    }
                }

                if (!Glasses.IsEmpty && GameAssetManager.TryGetAsset<GameItemGlassesAsset>(Glasses, out var gasset))
                {
                    if (!gasset.beardVisible)
                    {
                        return false;
                    }
                }

                if (!Pants.IsEmpty && GameAssetManager.TryGetAsset<GameItemPantsAsset>(Pants, out var passet))
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
            if (version >= 11)
            {
                hat = node["hat"].ToGuidIDBridge();
                mask = node["mask"].ToGuidIDBridge();
                top = node["top"].ToGuidIDBridge();
                bottom = node["bottom"].ToGuidIDBridge();
                backpack = node["backpack"].ToGuidIDBridge();
                vest = node["vest"].ToGuidIDBridge();
                glasses = node["glasses"].ToGuidIDBridge();
            }
            else
            {
                hat = (GUIDIDBridge)node["hat"].ToUInt16();
                mask = (GUIDIDBridge)node["mask"].ToUInt16();
                top = (GUIDIDBridge)node["top"].ToUInt16();
                bottom = (GUIDIDBridge)node["bottom"].ToUInt16();
                backpack = (GUIDIDBridge)node["backpack"].ToUInt16();
                vest = (GUIDIDBridge)node["vest"].ToUInt16();
                glasses = (GUIDIDBridge)node["glasses"].ToUInt16();
            }
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("hat", node).WriteGuidIDBridge(hat);
            document.CreateNodeC("mask", node).WriteGuidIDBridge(mask);
            document.CreateNodeC("top", node).WriteGuidIDBridge(top);
            document.CreateNodeC("bottom", node).WriteGuidIDBridge(bottom);
            document.CreateNodeC("backpack", node).WriteGuidIDBridge(backpack);
            document.CreateNodeC("vest", node).WriteGuidIDBridge(vest);
            document.CreateNodeC("glasses", node).WriteGuidIDBridge(glasses);
        }
    }
}
