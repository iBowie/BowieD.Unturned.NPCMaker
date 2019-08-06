using System;

namespace BowieD.NPCMaker.Math
{
    public static class Mathf
    {
        public static T Clamp<T>(T minimum, T maximum, T value) where T : IComparable
        {
            if (value.CompareTo(minimum) < 0)
                value = minimum;
            else if (value.CompareTo(maximum) > 0)
                value = maximum;
            return value;
        }
    }
}
