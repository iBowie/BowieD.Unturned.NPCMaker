using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes
{
    public class RewardRangeAttribute : Attribute
    {
        public object Minimum { get; }
        public object Maximum { get; }

        public RewardRangeAttribute(object min, object max)
        {
            this.Minimum = min;
            this.Maximum = max;
        }
    }
}
