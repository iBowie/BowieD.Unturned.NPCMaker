using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class ConditionOptionalAttribute : Attribute
    {
        public object defaultValue { get; private set; }
        public ConditionOptionalAttribute(object defaultValue)
        {
            this.defaultValue = defaultValue;
        }
    }
}
