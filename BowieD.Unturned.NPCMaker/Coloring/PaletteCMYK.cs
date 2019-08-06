using System.Linq;

namespace BowieD.Unturned.NPCMaker.Coloring
{
    /// <summary>
    /// CMYK - Cyan Magenta Yellow blacK
    /// </summary>
    public sealed class PaletteCMYK : Palette
    {
        public (double C, double M, double Y, double K) CMYK
        {
            get
            {
                return (C, M, Y, K);
            }
            set
            {
                C = value.C;
                M = value.M;
                Y = value.Y;
                K = value.K;
            }
        }

        /// <summary>
        /// [0..1]
        /// </summary>
        public double C, M, Y, K;
        public override Palette FromRGB((byte R, byte G, byte B) rgb)
        {
            // https://www.rapidtables.com/convert/color/rgb-to-cmyk.html
            double RR = rgb.R / 255d;
            double GG = rgb.G / 255d;
            double BB = rgb.B / 255d;

            K = 1d - new double[] { RR, GG, BB }.Max();
            C = (1d - RR - K) / (1d - K);
            M = (1d - GG - K) / (1d - K);
            Y = (1d - BB - K) / (1d - K);
            // did not mention that on the website
            C = double.IsNaN(C) ? 0 : C;
            M = double.IsNaN(M) ? 0 : M;
            Y = double.IsNaN(Y) ? 0 : Y;
            return this;
        }

        public override (byte R, byte G, byte B) ToRGB()
        {
            // https://www.rapidtables.com/convert/color/cmyk-to-rgb.html
            return ((byte)(255 * (1 - C) * (1 - K)), (byte)(255 * (1 - M) * (1 - K)), (byte)(255 * (1 - Y) * (1 - K)));
        }

        public override string ToString()
        {
            return $"({C};{M};{Y};{K})";
        }
    }
}
