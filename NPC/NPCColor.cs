using System;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public struct NPCColor
    {
        public byte R;
        public byte G;
        public byte B;

        public NPCColor(byte R, byte G, byte B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
        public static NPCColor FromHSV(double H, double S, double V)
        {
            #region OPTION_1
            //double C = V * S;
            //double X = C * (1d - Math.Abs((H / 60d) % 2d - 1d));
            //double m = V - C;
            //double RR = 0, GG = 0, BB = 0;
            //if (H < 60d)
            //{
            //    RR = C;
            //    GG = X;
            //    BB = 0;
            //}
            //else if (H < 120d)
            //{
            //    RR = X;
            //    GG = C;
            //    BB = 0;
            //}
            //else if (H < 180d)
            //{
            //    RR = 0;
            //    GG = C;
            //    BB = X;
            //}
            //else if (H < 240d)
            //{
            //    RR = 0;
            //    GG = X;
            //    BB = C;
            //}
            //else if (H < 300d)
            //{
            //    RR = X;
            //    GG = 0;
            //    BB = C;
            //}
            //else
            //{
            //    RR = C;
            //    GG = 0;
            //    BB = X;
            //}
            //return new NPCColor((byte)((RR + m) * 255), (byte)((GG + m) * 255), (byte)((BB + m) * 255));
            #endregion
            #region OPTION_2
            // working properly
            int hi = Convert.ToInt32(Math.Floor(H / 60)) % 6;
            double f = H / 60 - Math.Floor(H / 60);

            V = V * 255;
            byte v = (byte)Convert.ToInt32(V);
            byte p = (byte)Convert.ToInt32(V * (1 - S));
            byte q = (byte)Convert.ToInt32(V * (1 - f * S));
            byte t = (byte)Convert.ToInt32(V * (1 - (1 - f) * S));

            if (hi == 0)
                return new NPCColor(v, t, p);
            else if (hi == 1)
                return new NPCColor(q, v, p);
            else if (hi == 2)
                return new NPCColor(p, v, t);
            else if (hi == 3)
                return new NPCColor(p, q, v);
            else if (hi == 4)
                return new NPCColor(t, p, v);
            else
                return new NPCColor(v, p, q);
            #endregion
        }
        [XmlIgnore]
        public Tuple<double, double, double> HSV
        {
            get
            {
                #region OPTION_1
                double RR = R / 255d;
                double GG = G / 255d;
                double BB = B / 255d;
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
                return new Tuple<double, double, double>(H, S, V);
                #endregion
            }
        }
        public string HEX
        {
            get => $"#{R:X2}{G:X2}{B:X2}";
            set
            {
                if (value.StartsWith("#") && value.Length > 1)
                    value = value.Substring(1);
                if (value.Length < 6)
                    return;
                try
                {
                    R = byte.Parse($"{value[0]}{value[1]}", NumberStyles.HexNumber);
                }
                catch { }
                try
                {
                    G = byte.Parse($"{value[2]}{value[3]}", NumberStyles.HexNumber);
                }
                catch { }
                try
                {
                    B = byte.Parse($"{value[4]}{value[5]}", NumberStyles.HexNumber);
                }
                catch { }
            }
        }
        public static bool CanParseHex(string text)
        {
            if ((text.StartsWith("#") && text.Length < 7) || (text.Length < 6))
                return false;
            int offset = 0;
            if (text.StartsWith("#"))
                offset = 1;
            return (byte.TryParse($"{text[0+offset]}{text[1+offset]}", NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte result1))
                && (byte.TryParse($"{text[2+offset]}{text[3+offset]}", NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte result2))
                && (byte.TryParse($"{text[4+offset]}{text[5+offset]}", NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte result3));
        }
        [XmlIgnore]
        public Brush Brush => new BrushConverter().ConvertFromString(HEX) as Brush;
        [XmlIgnore]
        public Color Color => Color.FromRgb(R, G, B);

        public static bool operator==(NPCColor a, NPCColor b)
        {
            return (a.R == b.R && a.G == b.G && a.B == b.B);
        }
        public static bool operator!=(NPCColor a, NPCColor b)
        {
            return !(a == b);
        }
    }
}
