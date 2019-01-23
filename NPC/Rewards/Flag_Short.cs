using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class Flag_Short : Reward
    {
        public Flag_Short()
        {
            Type = RewardType.Flag_Short;
        }

        public ushort Id { get; set; }
        public short Value { get; set; }
        public Modification_Type Modification { get; set; }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Reward_{conditionIndex}_Type Flag_Short");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_ID {this.Id}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_Value {this.Value}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_Modification {this.Modification}");
            return output;
        }

        public override string ToString()
        {
            return $"{(string)MainWindow.Instance.TryFindResource("reward_Type_Flag_Short")} : {(string)MainWindow.Instance.TryFindResource($"Modification_{Modification}")} {Value} ({Id})";
        }
    }
}
