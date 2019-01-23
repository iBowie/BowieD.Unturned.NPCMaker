using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class Vehicle : Reward
    {
        public Vehicle()
        {
            Type = RewardType.Vehicle;
        }

        public ushort Id { get; set; }
        public ushort SpawnPointID { get; set; }

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
