using System;
using System.Linq;
using BowieD.Unturned.NPCMaker.BetterForms;

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

        public override int Elements => 3;
        public override void Init(Universal_RewardEditor ure)
        {
            ure.AddLabel(MainWindow.Localize("rewardEditor_FlagA_ID"));
            ure.AddTextBox(5);
            ure.AddLabel(MainWindow.Localize("rewardEditor_FlagB_ID"));
            ure.AddTextBox(5);
            ure.AddLabel(MainWindow.Localize("rewardEditor_Operation"));
            ure.AddComboBox(Enum.GetValues(typeof(Operation_Type)).Cast<Operation_Type>(), "Operation_{0}");
        }
        public override void Init(Universal_RewardEditor ure, Reward start)
        {
            Init(ure);
            if (start != null)
            {
                ure.SetMainValue(1, (start as Flag_Math).FlagA);
                ure.SetMainValue(3, (start as Flag_Math).FlagB);
                ure.SetMainValue(5, (start as Flag_Math).Operation);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Flag_Math()
            {
                FlagA = ushort.Parse(input[0].ToString()),
                FlagB = ushort.Parse(input[1].ToString()),
                Operation = (Operation_Type)input[2]
            } as T;
        }

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
