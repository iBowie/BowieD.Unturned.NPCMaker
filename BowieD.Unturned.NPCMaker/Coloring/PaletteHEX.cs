using System.Globalization;

namespace BowieD.Unturned.NPCMaker.Coloring
{
    /// <summary>
    /// HEX - Hexadecimal
    /// </summary>
    public sealed class PaletteHEX : Palette
    {
        /// <summary>
        /// #H1H2H3
        /// </summary>
        public string HEX
        {
            get
            {
                return $"#{H1}{H2}{H3}";
            }
            set
            {
                H1 = "" + value[1] + value[2];
                H2 = "" + value[3] + value[4];
                H3 = "" + value[5] + value[6];
            }
        }
        /// <summary>
        /// [00..FF]
        /// </summary>
        private string H1 = "00", H2 = "00", H3 = "00";
        public override Palette FromRGB((byte R, byte G, byte B) rgb)
        {
            H1 = $"{rgb.R:X2}";
            H2 = $"{rgb.G:X2}";
            H3 = $"{rgb.B:X2}";
            return this;
        }

        public override (byte R, byte G, byte B) ToRGB()
        {
            return (byte.Parse(H1, NumberStyles.HexNumber), byte.Parse(H2, NumberStyles.HexNumber), byte.Parse(H3, NumberStyles.HexNumber));
        }

        public override string ToString()
        {
            return HEX.StartsWith("#") ? HEX : "#" + HEX;
        }
    }
}
