using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class Flag_Math : Reward
    {
        public Flag_Math()
        {
            Type = RewardType.Flag_Math;
        }

        public ushort FlagA { get; set; }
        public ushort FlagB { get; set; }
        public Operation_Type Operation { get; set; }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Reward_{conditionIndex}_Type Flag_Math");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_A_ID {this.FlagA}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_B_ID {this.FlagB}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_Operation {this.Operation}");
            return output;
        }

        public override string ToString()
        {
            return $"{(string)MainWindow.Instance.TryFindResource("reward_Type_Flag_Math")} : {(string)MainWindow.Instance.TryFindResource($"Operation_{Operation}")} ({FlagA} -> {FlagB})";
        }
    }
}
