using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Kills_Players_Cond : Condition
    {
        public ushort ID { get; set; }
        public short Value { get; set; }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Kills_Players");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_ID {this.ID}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Value}");
            return output;
        }
    }
}
