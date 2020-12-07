using System;
using System.Globalization;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Coloring
{
#pragma warning disable CS0660
#pragma warning disable CS0661
    [Serializable]
    public struct Color
    {
        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
        public Color(string hex)
        {
            if (hex.StartsWith("#"))
            {
                R = byte.Parse(hex[1] + "" + hex[2], System.Globalization.NumberStyles.HexNumber);
                G = byte.Parse(hex[3] + "" + hex[4], System.Globalization.NumberStyles.HexNumber);
                B = byte.Parse(hex[5] + "" + hex[6], System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                throw new ArgumentException("Should start with #");
            }
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
        public static implicit operator System.Windows.Media.Brush(Coloring.Color a)
        {
            return new SolidColorBrush(a);
        }
        public override string ToString()
        {
            return $"({R};{G};{B})";
        }
        public string ToHEX()
        {
            return $"#{R:X2}{G:X2}{B:X2}";
        }
        public static bool IsHEX(string input)
        {
            if (input.Length == 7 && input[0] == '#')
            {
                return byte.TryParse(input[1] + "" + input[2], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte s1)
                    && byte.TryParse(input[3] + "" + input[4], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte s2)
                    && byte.TryParse(input[5] + "" + input[6], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte s3);
            }
            return false;
        }
    }
}
