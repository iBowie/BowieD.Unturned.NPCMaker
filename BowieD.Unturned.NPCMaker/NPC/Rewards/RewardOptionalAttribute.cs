using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public class RewardOptionalAttribute : Attribute
    {
        public object defaultValue { get; private set; }
        public object skipValue { get; private set; }
        public RewardOptionalAttribute(object defaultValue, object skipValue)
        {
            this.defaultValue = defaultValue;
            this.skipValue = skipValue;
        }
        public bool ConditionApplied(object currentValue)
        {
            if (skipValue.Equals(currentValue))
                return true;
            return false;
        }
    }
}
