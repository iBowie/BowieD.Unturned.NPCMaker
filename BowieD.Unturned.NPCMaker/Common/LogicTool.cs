using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Common
{
    public static class LogicTool
    {
        public static bool Compare<T>(T a, T b, Logic_Type logic) where T : IComparable<T>
        {
            switch (logic)
            {
                case Logic_Type.Equal:
                    return a.CompareTo(b) == 0;
                case Logic_Type.Greater_Than:
                    return a.CompareTo(b) > 0;
                case Logic_Type.Greater_Than_Or_Equal_To:
                    return a.CompareTo(b) >= 0;
                case Logic_Type.Less_Than:
                    return a.CompareTo(b) < 0;
                case Logic_Type.Less_Than_Or_Equal_To:
                    return a.CompareTo(b) <= 0;
                case Logic_Type.Not_Equal:
                    return a.CompareTo(b) != 0;
                default: throw new Exception("Invalid logic");
            }
        }
    }
}
