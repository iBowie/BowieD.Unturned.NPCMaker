using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Coloring
{
#pragma warning restore CS0661
#pragma warning restore CS0660
    public static class ColorConverter
    {
        static readonly BrushConverter converter = new BrushConverter();
        public static Dictionary<string, string> UnturnedColors { get; } = new Dictionary<string, string>()
        {
            { "common", "#ffffff" },
            { "gold", "#d2bf22" },
            { "uncommon", "#1f871f" },
            { "rare", "#4b64fa" },
            { "epic", "#964bfa" },
            { "legendary", "#c832fa" },
            { "mythical", "#fa3219" },
            { "red", "#bf1f1f" },
            { "green", "#1f871f" },
            { "blue", "#3298c8" },
            { "orange", "#ab8019" },
            { "yellow", "#dcb413" },
            { "purple", "#6a466d" }
        };
        public static Dictionary<string, string> UnityColors { get; } = new Dictionary<string, string>()
        {
            { "black", "#000000" },
            { "blue", "#0000FF" },
            { "cyan", "#00FFFF" },
            { "gray", "#7F7F7F" },
            { "grey", "#7F7F7F" },
            { "magenta", "#FF00FF" },
            { "green", "#00FF00" },
            { "red", "#FF0000" },
            { "white", "#FFFFFF" },
            { "yellow", "#FFEB04" }
        };
        public static Brush ParseColor(string code)
        {
            if (code == null)
                return Brushes.Black;

            string parse;

            if (UnturnedColors.TryGetValue(code, out string ucolor))
                parse = ucolor;
            else if (UnityColors.TryGetValue(code, out string uncolor))
                parse = uncolor;
            else
                parse = code;

            try
            {
                return converter.ConvertFromString(parse) as Brush;
            }
            catch
            {
                return Brushes.Transparent;
            }
        }
        public static string BrushToHEX(Brush brush)
        {
            if (brush is SolidColorBrush scb)
            {
                var wcl = scb.Color;

                Color cl = new Color(wcl.R, wcl.G, wcl.B);

                return cl.ToHEX();
            }
            else
            {
                throw new ArgumentException("Expected SolidColorBrush");
            }
        }

        public static Color HSVtoColor(double H, double S, double V)
        {
            int hi = Convert.ToInt32(Math.Floor(H / 60)) % 6;
            double f = H / 60 - Math.Floor(H / 60);

            V *= 255;
            byte v = (byte)Convert.ToInt32(V);
            byte p = (byte)Convert.ToInt32(V * (1 - S));
            byte q = (byte)Convert.ToInt32(V * (1 - f * S));
            byte t = (byte)Convert.ToInt32(V * (1 - (1 - f) * S));

            switch (hi)
            {
                case 0:
                    return new Color(v, t, p);
                case 1:
                    return new Color(q, v, p);
                case 2:
                    return new Color(p, v, t);
                case 3:
                    return new Color(p, q, v);
                case 4:
                    return new Color(t, p, v);
                default:
                    return new Color(v, p, q);
            }
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
            {
                H = 0;
            }
            else if (Cmax == RR)
            {
                H = 60d * (((GG - BB) / delta) % 6d);
            }
            else if (Cmax == GG)
            {
                H = 60d * (((BB - RR) / delta) + 2d);
            }
            else if (Cmax == BB)
            {
                H = 60d * (((RR - GG) / delta) + 4d);
            }

            double S = 0;
            if (Cmax != 0)
            {
                S = delta / Cmax;
            }

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
            {
                preResult = (C, X, 0);
            }
            else if (H < 120)
            {
                preResult = (X, C, 0);
            }
            else if (H < 180)
            {
                preResult = (0, C, X);
            }
            else if (H < 240)
            {
                preResult = (0, X, C);
            }
            else if (H < 300)
            {
                preResult = (X, 0, C);
            }
            else
            {
                preResult = (C, 0, X);
            }

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
