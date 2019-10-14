using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCClothing
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
    }
}
