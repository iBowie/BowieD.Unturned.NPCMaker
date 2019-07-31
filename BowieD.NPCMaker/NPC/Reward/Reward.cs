using System.Collections.Generic;

namespace BowieD.NPCMaker.NPC.Reward
{
    public abstract class Reward
    {
        public abstract string RewardType { get; }
        public Dictionary<ELanguage, string> localization;
    }
}
