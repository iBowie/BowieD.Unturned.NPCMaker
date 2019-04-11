using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class Teleport : Reward
    {
        public Teleport()
        {
            Type = RewardType.Teleport;
        }

        public string SpawnpointID { get; set; }

        public override int Elements => 1;
        public override void Init(Universal_RewardEditor ure)
        {
            ure.AddLabel(MainWindow.Localize("rewardEditor_SpawnpointID"));
            ure.AddTextBox(int.MaxValue);
        }
        public override void Init(Universal_RewardEditor ure, Reward start)
        {
            Init(ure);
            if (start != null)
            {
                ure.SetMainValue(1, (start as Teleport).SpawnpointID);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Teleport()
            {
                SpawnpointID = input[0].ToString()
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Reward_{conditionIndex}_Type Teleport");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_Spawnpoint {this.SpawnpointID}");
            return output;
        }

        public override string ToString()
        {
            return $"{(string)MainWindow.Instance.TryFindResource("reward_Type_Teleport")} {SpawnpointID}";
        }
    }
}
