using BowieD.Unturned.NPCMaker.Coloring;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public struct NPCColor
    {
        public byte R;
        public byte G;
        public byte B;

        public NPCColor((byte R, byte G, byte B) rgb) : this(rgb.R, rgb.G, rgb.B) { }
        public NPCColor(byte R, byte G, byte B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public static bool operator==(NPCColor a, NPCColor b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }
        public static bool operator!=(NPCColor a, NPCColor b)
        {
            return !(a == b);
        }
        public static implicit operator NPCColor(Palette palette)
        {
            return new NPCColor(palette.ToRGB());
        }
        public static explicit operator PaletteRGB(NPCColor color)
        {
            var p = new PaletteRGB();
            p.FromRGB((color.R, color.G, color.B));
            return p;
        }
    }
}
