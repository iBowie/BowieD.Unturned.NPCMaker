using System;

namespace BowieD.Unturned.NPCMaker
{
    public static class MathUtil
    {
        public static bool ApproximateEquals(this float value, float anotherValue, float threshold = float.Epsilon)
        {
            return Math.Abs(value - anotherValue) <= threshold;
        }
    }
}
