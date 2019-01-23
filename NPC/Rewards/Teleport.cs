using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class Teleport : Reward
    {
        public Teleport()
        {
            Type = RewardType.Teleport;
        }

        public ushort SpawnpointID { get; set; }

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
