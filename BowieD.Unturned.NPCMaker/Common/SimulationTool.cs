using BowieD.Unturned.NPCMaker.NPC;
using System;

namespace BowieD.Unturned.NPCMaker.Common
{
    public static class SimulationTool
    {
        public static bool Compare<T>(T a, T b, Logic_Type logic) where T : IComparable
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
        public static short Operate(short a, short b, Operation_Type operation)
        {
            switch (operation)
            {
                case Operation_Type.Addition:
                    return (short)(a + b);
                case Operation_Type.Assign:
                    return b;
                case Operation_Type.Division:
                    return (short)(a / b);
                case Operation_Type.Multiplication:
                    return (short)(a * b);
                case Operation_Type.Subtraction:
                    return (short)(a - b);
                default: throw new Exception("Invalid operation");
            }
        }
        public static short Modify(short a, short b, Modification_Type modification)
        {
            switch (modification)
            {
                case Modification_Type.Assign:
                    return b;
                case Modification_Type.Decrement:
                    return (short)(a - b);
                case Modification_Type.Increment:
                    return (short)(a + b);
                default: throw new Exception("Invalid modification");
            }
        }
    }
}
