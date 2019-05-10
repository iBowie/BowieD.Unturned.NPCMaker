using System;
using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class Item_Random : Reward
    {
        public Item_Random()
        {
            Type = RewardType.Item_Random;
        }

        public ushort SpawnID { get; set; }
        public uint Amount { get; set; }

        public override int Elements => 2;
        public override void Init(Universal_RewardEditor ure)
        {
            ure.AddLabel(LocUtil.LocalizeReward("rewardEditor_SpawnID"));
            ure.AddTextBox(5);
            ure.AddLabel(LocUtil.LocalizeReward("rewardEditor_Amount"));
            ure.AddTextBox(6);
        }
        public override void Init(Universal_RewardEditor ure, Reward start)
        {
            Init(ure);
            if (start != null)
            {
                ure.SetMainValue(1, (start as Item_Random).SpawnID);
                ure.SetMainValue(3, (start as Item_Random).Amount);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Item_Random()
            {
                SpawnID = ushort.Parse(input[0].ToString()),
                Amount = uint.Parse(input[1].ToString())
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Reward_{conditionIndex}_Type Item_Random");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_ID {this.SpawnID}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_Amount {this.Amount}");
            return output;
        }

        public override string ToString()
        {
            return $"{(string)MainWindow.Instance.TryFindResource("reward_Type_Item_Random")} {SpawnID} x{Amount}";
        }
    }
}
