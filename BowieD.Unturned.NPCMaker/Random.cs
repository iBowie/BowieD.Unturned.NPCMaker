namespace BowieD.Unturned.NPCMaker
{
    public static class Random
    {
        private static readonly System.Random random = new System.Random();
        public static int NextInt32(int min, int max) => random.Next(min, max);
        public static int NextInt32(int max) => random.Next(max);
        public static int NextInt32() => random.Next();
        public static byte NextByte(byte min, byte max) => (byte)random.Next(min, max);
        public static byte NextByte(byte max) => (byte)random.Next(0, max);
        public static byte NextByte() => (byte)random.Next(0, 256);
    }
}
