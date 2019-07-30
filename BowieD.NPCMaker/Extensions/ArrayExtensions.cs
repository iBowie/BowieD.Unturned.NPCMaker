using System;
using System.Collections.Generic;

namespace BowieD.NPCMaker.Extensions
{
    public static class ArrayExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this Array array)
        {
            foreach (var k in array)
            {
                yield return (T)k;
            }
        }
    }
}
