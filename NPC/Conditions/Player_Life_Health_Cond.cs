using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Player_Life_Health_Cond : Condition
    {
        public Player_Life_Health_Cond()
        {
            Type = Condition_Type.Player_Life_Health;
        }

        public Logic_Type Logic { get; set; }
        public ushort Value { get; set; }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Player_Life_Health");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Logic {this.Logic}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Value}");
            return output;
        }
    }
}
