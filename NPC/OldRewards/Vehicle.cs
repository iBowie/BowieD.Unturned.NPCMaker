using System;
using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class Vehicle : Reward
    {
        public Vehicle()
        {
            Type = RewardType.Vehicle;
        }

        public ushort Id { get; set; }
        public string SpawnPointID { get; set; }

        public override int Elements => 2;
        public override void Init(Universal_RewardEditor ure)
        {
            ure.AddLabel(LocUtil.LocalizeReward("rewardEditor_ID"));
            ure.AddTextBox(5);
            ure.AddLabel(LocUtil.LocalizeReward("rewardEditor_SpawnpointID"));
            ure.AddTextBox(int.MaxValue);
        }
        public override void Init(Universal_RewardEditor ure, Reward start)
        {
            Init(ure);
            if (start != null)
            {
                ure.SetMainValue(1, (start as Vehicle).Id);
                ure.SetMainValue(3, (start as Vehicle).SpawnPointID);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Vehicle()
            {
                Id = ushort.Parse(input[0].ToString()),
                SpawnPointID = input[1].ToString()
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Reward_{conditionIndex}_Type Vehicle");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_ID {this.Id}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Reward_{conditionIndex}_Spawnpoint {this.SpawnPointID}");
            return output;
        }

        public override string ToString()
        {
            return $"{(string)MainWindow.Instance.TryFindResource("reward_Type_Vehicle")} {Id} : {SpawnPointID}";
        }
    }
}
