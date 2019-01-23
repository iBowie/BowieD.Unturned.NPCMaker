using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class Flag_Short_Random : Reward
    {
        public Flag_Short_Random()
        {
            Type = RewardType.Flag_Short_Random;
        }

        public ushort Id { get; set; }
        public short MinValue { get; set; }
        public short MaxValue { get; set; }
        public Modification_Type Modification { get; set; }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Reward_{conditionIndex}_Type Flag_Short_Random");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_ID {this.Id}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_Min_Value {this.MinValue}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_Max_Value {this.MaxValue}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_Modification {this.Modification}");
            return output;
        }

        public override string ToString()
        {
            return $"{(string)MainWindow.Instance.TryFindResource("reward_Type_Flag_Short_Random")} : {(string)MainWindow.Instance.TryFindResource($"Modification_{Modification}")} ({MinValue}-{MaxValue}) ({Id})";
        }
    }
}
