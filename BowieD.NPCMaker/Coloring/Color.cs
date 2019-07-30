namespace BowieD.NPCMaker.Coloring
{
    public struct Color
    {
        public byte R;
        public byte G;
        public byte B;

        public static bool operator==(Color a, Color b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }
        public static bool operator!=(Color a, Color b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }
        public override string ToString()
        {
            return $"({R};{G};{B})";
        }
    }
}
