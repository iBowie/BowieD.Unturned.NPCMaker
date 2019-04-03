using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker
{
    [XmlInclude(typeof(MetroTheme))]
    public abstract class ITheme
    {
        public abstract void Apply();
        public abstract byte R { get; set; }
        public abstract byte G { get; set; }
        public abstract byte B { get; set; }
        public abstract string Name { get; set; }
    }
}