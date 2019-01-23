using System;

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
