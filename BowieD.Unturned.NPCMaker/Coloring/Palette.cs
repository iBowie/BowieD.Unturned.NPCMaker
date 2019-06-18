namespace BowieD.Unturned.NPCMaker.Coloring
{
    public abstract class Palette
    {
        public static Palette Convert<T>(Palette from) where T : Palette, new()
        {
            Palette p = new T();
            p.FromRGB(from.ToRGB());
            return p;
        }
        public abstract Palette FromRGB((byte R, byte G, byte B) rgb);
        public abstract (byte R, byte G, byte B) ToRGB();
        public abstract new string ToString();
    }
}
