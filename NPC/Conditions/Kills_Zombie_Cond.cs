using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Kills_Zombie_Cond : Condition
    {
        public Kills_Zombie_Cond()
        {
            Type = Condition_Type.Kills_Zombie;
        }

        public bool Spawn { get; set; }
        public ushort Id { get; set; }
        public ushort NavMesh { get; set; }
        public uint Amount { get; set; }
        public Zombie_Type Zombie_Type { get; set; }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Kills_Zombie");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Zombie {this.Zombie_Type}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Amount}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Nav {this.NavMesh}");
            return output;
        }
    }
}
