using System.Collections.Generic;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker
{
    [XmlInclude(typeof(MetroTheme))]
    public abstract class Theme
    {
        public abstract void Apply();
        public abstract byte R { get; set; }
        public abstract byte G { get; set; }
        public abstract byte B { get; set; }
        public abstract string Name { get; set; }
        public abstract void Remove();
        public static Theme CurrentTheme { get; protected set; }
    }
}