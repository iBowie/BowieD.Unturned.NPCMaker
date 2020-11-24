namespace BowieD.Unturned.NPCMaker
{
    public static class Random
    {
        private static readonly System.Random random = new System.Random();
        public static int NextInt32(int min, int max)
        {
            return random.Next(min, max);
        }

        public static int NextInt32(int max)
        {
            return random.Next(max);
        }

        public static int NextInt32()
        {
            return random.Next();
        }

        public static byte NextByte(byte min, byte max)
        {
            return (byte)random.Next(min, max);
        }

        public static byte NextByte(byte max)
        {
            return (byte)random.Next(0, max);
        }

        public static byte NextByte()
        {
            return (byte)random.Next(0, 256);
        }

        public static float Value => (float)random.NextDouble();
    }
}
