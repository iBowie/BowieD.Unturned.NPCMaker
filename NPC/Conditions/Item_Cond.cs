using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Item_Cond : Condition
    {
        public Item_Cond()
        {
            Type = Condition_Type.Item;
        }

        public ushort Id { get; set; }
        public uint Amount { get; set; }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Item");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_ID {this.Id}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Amount {this.Amount}");
            return output;
        }
    }
}
