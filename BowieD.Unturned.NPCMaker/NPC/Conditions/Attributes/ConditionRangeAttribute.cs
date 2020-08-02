using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class ConditionRangeAttribute : Attribute
    {
        public object Minimum { get; }
        public object Maximum { get; }
        public ConditionRangeAttribute(object minimum, object maximum)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }
    }
}
