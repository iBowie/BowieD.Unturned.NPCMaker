using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes
{
    public class RewardOptionalAttribute : Attribute
    {
        public object DefaultValue { get; private set; }
        public object SkipValue { get; private set; }
        public RewardOptionalAttribute(object defaultValue, object skipValue)
        {
            DefaultValue = defaultValue;
            SkipValue = skipValue;
        }
        public bool ConditionApplied(object currentValue)
        {
            if (SkipValue.Equals(currentValue))
            {
                return true;
            }

            return false;
        }
    }
}
