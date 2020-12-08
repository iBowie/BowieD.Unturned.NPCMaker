using System;

namespace BowieD.Unturned.NPCMaker.NPC.Shared.Attributes
{
    public class OptionalAttribute : Attribute
    {
        public object DefaultValue { get; private set; }
        public OptionalAttribute(object defaultValue)
        {
            this.DefaultValue = defaultValue;
        }
    }
}
