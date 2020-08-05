using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes
{
    public class RewardOptionalAttribute : Attribute
    {
        public object DefaultValue { get; private set; }
        public RewardOptionalAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}
