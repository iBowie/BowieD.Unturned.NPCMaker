using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;

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

        public static string ReplacePlaceholders(NPCCharacter character, Simulation simulation, string raw)
        {
            string result = raw;

            if (result.Contains("<br>"))
                result = result.Replace("<br>", Environment.NewLine);

            if (character != null && result.Contains("<name_npc>"))
                result = result.Replace("<name_npc>", ReplacePlaceholders(character, simulation, character.displayName));

            if (simulation != null && result.Contains("<name_char>"))
                result = result.Replace("<name_char>", simulation.Name);

            if (result.Contains("<pause>"))
                result = result.Replace("<pause>", "");

            return result;
        }
    }
}
