using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Time_Of_Day_Cond : Condition
    {
        public Time_Of_Day_Cond()
        {
            Type = Condition_Type.Time_Of_Day;
        }

        public Logic_Type Logic { get; set; }
        public ulong Second { get; set; }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Time_Of_Day");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Logic {this.Logic}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Second {this.Second}");
            return output;
        }
    }
}
