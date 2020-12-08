using System;

namespace BowieD.Unturned.NPCMaker.NPC.Shared.Attributes
{
    public class RangeAttribute : Attribute
    {
        public object Minimum { get; }
        public object Maximum { get; }
        public RangeAttribute(object minimum, object maximum)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }
    }
}
