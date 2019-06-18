using System;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Coloring
{
    public sealed class PaletteHSL : Palette
    {
        public (double H, double S, double L) HSL
        {
            get
            {
                return (H, S, L);
            }
            set
            {
                H = value.H;
                S = value.S;
                L = value.L;
            }
        }

        private double h;
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
        public double S, L;
        public override Palette FromRGB((byte R, byte G, byte B) rgb)
        {
            // https://ru.wikipedia.org/wiki/HSL
            double RR = rgb.R / 255d;
            double GG = rgb.G / 255d;
            double BB = rgb.B / 255d;
            double MAX = new double[] { RR, GG, BB }.Max();
            double MIN = new double[] { RR, GG, BB }.Min();
            if (MAX != MIN)
            {
                if (MAX == RR)
                {
                    if (GG >= BB)
                    {
                        H = 60 * ((GG - BB) / (MAX - MIN)) + 0;
                    }
                    else
                    {
                        H = 60 * ((GG - BB) / (MAX - MIN)) + 360;
                    }
                }
                else if (MAX == GG)
                {
                    H = 60 * ((BB - RR) / (MAX - MIN)) + 120;
                }
                else
                {
                    H = 60 * ((RR - GG) / (MAX - MIN)) + 240;
                }
            }
            L = (MAX + MIN) * 0.5;
            if (L == 0 || MAX == MIN)
            {
                S = 0;
            }
            else if (L <= 0.5)
            {
                S = ((MAX - MIN) / (MAX + MIN));
            }
            else if (L < 1)
            {
                S = ((MAX - MIN) / (2d - (MAX + MIN)));
            }
            else
            {
                S = ((MAX - MIN) / (1d - System.Math.Abs(1d - (MAX + MIN))));
            }
            S = Math.Round(S, 2);
            L = Math.Round(L, 2);
            return this;
        }

        public override (byte R, byte G, byte B) ToRGB()
        {
            // https://www.rapidtables.com/convert/color/hsl-to-rgb.html
            double C = (1d - Math.Abs(2d * L - 1d)) * S;
            double X = C * (1d - Math.Abs((H / 60d) % 2d - 1d));
            double m = L - C / 2d;
            (double R, double G, double B) preResult;
            if (H < 60)
                preResult = (C, X, 0);
            else if (H < 120)
                preResult = (X, C, 0);
            else if (H < 180)
                preResult = (0, C, X);
            else if (H < 240)
                preResult = (0, X, C);
            else if (H < 300)
                preResult = (X, 0, C);
            else
                preResult = (C, 0, X);
            return ((byte)((preResult.R + m) * 255d), (byte)((preResult.G + m) * 255d), (byte)((preResult.B + m) * 255d));
        }

        public override string ToString()
        {
            return $"({H};{S};{L})";
        }
    }
}
