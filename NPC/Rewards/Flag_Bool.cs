using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class Flag_Bool : Reward
    {
        public Flag_Bool()
        {
            Type = RewardType.Flag_Bool;
        }

        public ushort Id { get; set; }
        public bool Value { get; set; }

        public override int Elements => 2;
        public override void Init(Universal_RewardEditor ure)
        {
            ure.AddLabel(MainWindow.Localize("rewardEditor_FlagID"));
            ure.AddTextBox(5);
            ure.AddLabel(MainWindow.Localize("rewardEditor_Value"));
            ure.AddCheckBox(false);
        }
        public override void Init(Universal_RewardEditor ure, Reward start)
        {
            Init(ure);
            if (start != null)
            {
                ure.SetMainValue(1, (start as Flag_Bool).Id);
                ure.SetMainValue(3, (start as Flag_Bool).Value);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Flag_Bool()
            {
                Id = ushort.Parse(input[0].ToString()),
                Value = (bool)input[1]
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Reward_{conditionIndex}_Type Flag_Bool");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_ID {this.Id}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_Value {this.Value}");
            return output;
        }

        public override string ToString()
        {
            return $"{(string)MainWindow.Instance.TryFindResource("reward_Type_Flag_Bool")} : {Value} ({Id})";
        }
    }
}
