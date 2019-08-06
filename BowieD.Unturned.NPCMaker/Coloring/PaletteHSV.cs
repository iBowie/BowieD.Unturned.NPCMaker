using System;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Coloring
{
    /// <summary>
    /// HSV - Hue Saturation Value
    /// </summary>
    public sealed class PaletteHSV : Palette
    {
        public (double H, double S, double V) HSV
        {
            get
            {
                return (H, S, V);
            }
            set
            {
                H = value.H;
                S = value.S;
                V = value.V;
            }
        }

        /// <summary>
        /// [0..360]
        /// </summary>
        public double H
        {
            get
            {
                return h;
            }
            set
            {
                double newValue = value % 360;
                while (newValue < 0)
                    newValue += 360;
                h = newValue;
            }
        }
        /// <summary>
        /// [0..1]
        /// </summary>
        public double S, V;
        private double h;
        public override Palette FromRGB((byte R, byte G, byte B) rgb)
        {
            double RR = rgb.R / 255d;
            double GG = rgb.G / 255d;
            double BB = rgb.B / 255d;
            double Cmax = new double[] { RR, GG, BB }.Max();
            double Cmin = new double[] { RR, GG, BB }.Min();
            double delta = Cmax - Cmin;
            H = 0;
            if (delta == 0)
                H = 0;
            else if (Cmax == RR)
                H = 60d * (((GG - BB) / delta) % 6d);
            else if (Cmax == GG)
                H = 60d * (((BB - RR) / delta) + 2d);
            else if (Cmax == BB)
                H = 60d * (((RR - GG) / delta) + 4d);
            S = 0;
            if (Cmax != 0)
                S = delta / Cmax;
            V = Cmax;
            S = Math.Round(S, 2);
            V = Math.Round(V, 2);
            return this;
        }
        public override (byte R, byte G, byte B) ToRGB()
        {
            int hi = System.Convert.ToInt32(Math.Floor(H / 60)) % 6;
            double f = H / 60 - Math.Floor(H / 60);

            V = V * 255;
            byte v = (byte)System.Convert.ToInt32(V);
            byte p = (byte)System.Convert.ToInt32(V * (1 - S));
            byte q = (byte)System.Convert.ToInt32(V * (1 - f * S));
            byte t = (byte)System.Convert.ToInt32(V * (1 - (1 - f) * S));

            if (hi == 0)
                return (v, t, p);
            else if (hi == 1)
                return (q, v, p);
            else if (hi == 2)
                return (p, v, t);
            else if (hi == 3)
                return (p, q, v);
            else if (hi == 4)
                return (t, p, v);
            else
                return (v, p, q);
        }
        public override string ToString()
        {
            return $"({H};{S};{V})";
        }
    }
}
