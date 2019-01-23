using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class Reputation : Reward
    {
        public Reputation()
        {
            Type = RewardType.Reputation;
        }

        public int Value { get; set; }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Reward_{conditionIndex}_Type Reputation");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_Value {this.Value}");
            return output;
        }

        public override string ToString()
        {
            return $"{(string)MainWindow.Instance.TryFindResource("reward_Type_Reputation")} x{Value}";
        }
    }
}
