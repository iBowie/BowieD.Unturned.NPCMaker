namespace BowieD.Unturned.NPCMaker.Coloring
{
    /// <summary>
    /// RGB - Red Green Blue
    /// </summary>
    public sealed class PaletteRGB : Palette
    {
        public (byte R, byte G, byte B) RGB
        {
            get
            {
                return ToRGB();
            }
            set
            {
                FromRGB(value);
            }
        }

        /// <summary>
        /// [0..255]
        /// </summary>
        public byte R, G, B;
        public override Palette FromRGB((byte R, byte G, byte B) rgb)
        {
            this.R = rgb.R;
            this.G = rgb.G;
            this.B = rgb.B;
            return this;
        }
        public override (byte R, byte G, byte B) ToRGB()
        {
            return (R, G, B);
        }
        public override string ToString()
        {
            return $"({R};{G};{B})";
        }
    }
}
