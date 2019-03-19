using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class Quest : Reward
    {
        public Quest()
        {
            Type = RewardType.Quest;
        }

        public ushort Id { get; set; }

        public override int Elements => 1;
        public override void Init(Universal_RewardEditor ure)
        {
            ure.AddLabel(MainWindow.Localize("rewardEditor_ID"));
            ure.AddTextBox(5);
        }
        public override void Init(Universal_RewardEditor ure, Reward start)
        {
            Init(ure);
            if (start != null)
            {
                ure.SetMainValue(1, (start as Quest).Id);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Quest()
            {
                Id = ushort.Parse(input[0].ToString())
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Reward_{conditionIndex}_Type Quest");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_ID {this.Id}");
            return output;
        }

        public override string ToString()
        {
            return $"{(string)MainWindow.Instance.TryFindResource("reward_Type_Quest")} : {Id}";
        }
    }
}
