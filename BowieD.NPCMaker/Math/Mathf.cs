using System;

namespace BowieD.NPCMaker.Math
{
    public static class Mathf
    {
        public static Int16 Clamp(Int16 minimum, Int16 maximum, Int16 value)
        {
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;
            return value;
        }
        public static Int32 Clamp(Int32 minimum, Int32 maximum, Int32 value)
        {
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;
            return value;
        }
        public static Int64 Clamp(Int64 minimum, Int64 maximum, Int64 value)
        {
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;
            return value;
        }
        public static UInt16 Clamp(UInt16 minimum, UInt16 maximum, UInt16 value)
        {
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;
            return value;
        }
        public static UInt32 Clamp(UInt32 minimum, UInt32 maximum, UInt32 value)
        {
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;
            return value;
        }
        public static UInt64 Clamp(UInt64 minimum, UInt64 maximum, UInt64 value)
        {
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;
            return value;
        }
        public static Byte Clamp(Byte minimum, Byte maximum, Byte value)
        {
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;
            return value;
        }
        public static Single Clamp(Single minimum, Single maximum, Single value)
        {
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;
            return value;
        }
        public static Double Clamp(Double minimum, Double maximum, Double value)
        {
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;
            return value;
        }
    }
}
