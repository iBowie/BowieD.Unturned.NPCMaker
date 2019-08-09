using System;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Coloring
{
#pragma warning disable CS0660
#pragma warning disable CS0661
    public struct Color
    {
        public Color(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }
        public Color(string hex)
        {
            if (hex.StartsWith("#"))
            {
                this.R = byte.Parse(hex[1] + "" + hex[2], System.Globalization.NumberStyles.HexNumber);
                this.G = byte.Parse(hex[3] + "" + hex[4], System.Globalization.NumberStyles.HexNumber);
                this.B = byte.Parse(hex[5] + "" + hex[6], System.Globalization.NumberStyles.HexNumber);
            }
            else
                throw new ArgumentException("Should start with #");
        }
        public byte R;
        public byte G;
        public byte B;

        public static bool operator ==(Color a, Color b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }
        public static bool operator !=(Color a, Color b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B;
        }
        public static implicit operator System.Windows.Media.Color(Coloring.Color a)
        {
            return System.Windows.Media.Color.FromRgb(a.R, a.G, a.B);
        }
        public static implicit operator Coloring.Color(System.Windows.Media.Color a)
        {
            return new Color(a.R, a.G, a.B);
        }
        public override string ToString()
        {
            return $"({R};{G};{B})";
        }
        public string ToHEX()
        {
            return $"#{R:X2}{G:X2}{B:X2}";
        }
    }
#pragma warning restore CS0661
#pragma warning restore CS0660
    public static class ColorConverter
    {
        public static Color HSVtoColor(double H, double S, double V)
        {
            int hi = Convert.ToInt32(Math.Floor(H / 60)) % 6;
            double f = H / 60 - Math.Floor(H / 60);

            V *= 255;
            byte v = (byte)Convert.ToInt32(V);
            byte p = (byte)Convert.ToInt32(V * (1 - S));
            byte q = (byte)Convert.ToInt32(V * (1 - f * S));
            byte t = (byte)Convert.ToInt32(V * (1 - (1 - f) * S));

            if (hi == 0)
                return new Color(v, t, p);
            else if (hi == 1)
                return new Color(q, v, p);
            else if (hi == 2)
                return new Color(p, v, t);
            else if (hi == 3)
                return new Color(p, q, v);
            else if (hi == 4)
                return new Color(t, p, v);
            else
                return new Color(v, p, q);
        }
        public static (double H, double S, double V) ColorToHSV(Color color)
        {
            double RR = color.R / 255d;
            double GG = color.G / 255d;
            double BB = color.B / 255d;
            double Cmax = new double[] { RR, GG, BB }.Max();
            double Cmin = new double[] { RR, GG, BB }.Min();
            double delta = Cmax - Cmin;
            double H = 0;
            if (delta == 0)
                H = 0;
            else if (Cmax == RR)
                H = 60d * (((GG - BB) / delta) % 6d);
            else if (Cmax == GG)
                H = 60d * (((BB - RR) / delta) + 2d);
            else if (Cmax == BB)
                H = 60d * (((RR - GG) / delta) + 4d);
            double S = 0;
            if (Cmax != 0)
                S = delta / Cmax;
            double V = Cmax;
            S = Math.Round(S, 2);
            V = Math.Round(V, 2);
            return (H, S, V);
        }
        public static Color HSLtoColor(double H, double S, double L)
        {
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
            return new Color((byte)((preResult.R + m) * 255d), (byte)((preResult.G + m) * 255d), (byte)((preResult.B + m) * 255d));
        }
        public static (double H, double S, double L) ColorToHSL(Color color)
        {
            double RR = color.R / 255d;
            double GG = color.G / 255d;
            double BB = color.B / 255d;
            double MAX = new double[] { RR, GG, BB }.Max();
            double MIN = new double[] { RR, GG, BB }.Min();
            double H = 0, S, L;
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
            return (H, S, L);
        }
        public static Color CMYKtoColor(double C, double M, double Y, double K)
        {
            return new Color((byte)(255 * (1 - C) * (1 - K)), (byte)(255 * (1 - M) * (1 - K)), (byte)(255 * (1 - Y) * (1 - K)));
        }
        public static (double C, double M, double Y, double K) ColorToCMYK(Color color)
        {
            double RR = color.R / 255d;
            double GG = color.G / 255d;
            double BB = color.B / 255d;

            double K = 1d - new double[] { RR, GG, BB }.Max();
            double C = (1d - RR - K) / (1d - K);
            double M = (1d - GG - K) / (1d - K);
            double Y = (1d - BB - K) / (1d - K);
            // did not mention that on the website
            C = double.IsNaN(C) ? 0 : C;
            M = double.IsNaN(M) ? 0 : M;
            Y = double.IsNaN(Y) ? 0 : Y;
            return (C, M, Y, K);
        }
    }
}
