using System;
using System.Linq;
using BowieD.Unturned.NPCMaker.BetterForms;

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

        public override int Elements => 3;
        public override void Init(Universal_RewardEditor ure)
        {
            ure.AddLabel(MainWindow.Localize("rewardEditor_FlagID"));
            ure.AddTextBox(5);
            ure.AddLabel(MainWindow.Localize("rewardEditor_Value"));
            ure.AddTextBox(5);
            ure.AddLabel(MainWindow.Localize("rewardEditor_Modification"));
            ure.AddComboBox(Enum.GetValues(typeof(Modification_Type)).Cast<Modification_Type>(), "Modification_{0}");
        }
        public override void Init(Universal_RewardEditor ure, Reward start)
        {
            Init(ure);
            if (start != null)
            {
                ure.SetMainValue(1, (start as Flag_Short).Id);
                ure.SetMainValue(3, (start as Flag_Short).Value);
                ure.SetMainValue(5, (start as Flag_Short).Modification);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Flag_Short()
            {
                Id = ushort.Parse(input[0].ToString()),
                Value = short.Parse(input[1].ToString()),
                Modification = (Modification_Type)input[2]
            } as T;
        }

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
