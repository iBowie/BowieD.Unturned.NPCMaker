using System.Globalization;

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
    }
}
