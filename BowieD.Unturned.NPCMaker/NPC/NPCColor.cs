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
        }
        public static NPCColor FromHEX(string HEX)
        {
            if (HEX.StartsWith("#") && HEX.Length > 1)
                HEX = HEX.Substring(1);
            if (HEX.Length < 6)
                throw new ArgumentException("Not HEX");
            if (byte.TryParse($"{HEX[0]}{HEX[1]}", NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte R) &&
                byte.TryParse($"{HEX[2]}{HEX[3]}", NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte G) &&
                byte.TryParse($"{HEX[4]}{HEX[5]}", NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte B))
            {
                return new NPCColor(R, G, B);
            }
            else
                throw new ArgumentException("Not HEX");
        }
        public static NPCColor FromBrush(SolidColorBrush b)
        {
            var clr = b.Color;
            return new NPCColor(clr.R, clr.G, clr.B);
        }
        [XmlIgnore]
        public (double H, double S, double V) HSV
        {
            get
            {
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
                return (H, S, V);
            }
        }
        [XmlIgnore]
        public string HEX
        {
            get => $"#{R:X2}{G:X2}{B:X2}";
        }
        public static bool CanParseHex(string text)
        {
            return brushConverter.IsValid(text.StartsWith("#") ? text.Substring(1) : text)
                || brushConverter.IsValid(text.StartsWith("#") ? text : "#" + text);
        }

        private static BrushConverter brushConverter = new BrushConverter();
        [XmlIgnore]
        public Brush Brush => brushConverter.ConvertFromString(HEX) as Brush;
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
        public override int GetHashCode()
        {
            return HEX.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
