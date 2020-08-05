using System;

namespace BowieD.Unturned.NPCMaker
{
    public static class MathUtil
    {
        public static bool ApproximateEquals(this float value, float anotherValue, float threshold = float.Epsilon)
        {
            return Math.Abs(value - anotherValue) <= threshold;
        }
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return min;
            if (value.CompareTo(max) > 0)
                return max;
            return value;
        }
    }
}
