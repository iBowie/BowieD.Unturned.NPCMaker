using System;
using System.Linq;
using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.Localization;

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

        public override int Elements => 4;
        public override void Init(Universal_RewardEditor ure)
        {
            ure.AddLabel(LocUtil.LocalizeReward("rewardEditor_FlagID"));
            ure.AddTextBox(5);
            ure.AddLabel(LocUtil.LocalizeReward("rewardEditor_MinValue"));
            ure.AddTextBox(5);
            ure.AddLabel(LocUtil.LocalizeReward("rewardEditor_MaxValue"));
            ure.AddTextBox(5);
            ure.AddLabel(LocUtil.LocalizeReward("rewardEditor_Modification"));
            ure.AddComboBox(Enum.GetValues(typeof(Modification_Type)).Cast<Modification_Type>(), "Modification_{0}");
        }
        public override void Init(Universal_RewardEditor ure, Reward start)
        {
            Init(ure);
            if (start != null)
            {
                ure.SetMainValue(1, (start as Flag_Short_Random).Id);
                ure.SetMainValue(3, (start as Flag_Short_Random).MinValue);
                ure.SetMainValue(5, (start as Flag_Short_Random).MaxValue);
                ure.SetMainValue(7, (start as Flag_Short_Random).Modification);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Flag_Short_Random()
            {
                Id = ushort.Parse(input[0].ToString()),
                MinValue = short.Parse(input[1].ToString()),
                MaxValue = short.Parse(input[2].ToString()),
                Modification = (Modification_Type)input[3]
            } as T;
        }

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
