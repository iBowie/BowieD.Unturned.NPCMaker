using System;

namespace BowieD.Unturned.NPCMaker.NPC.Shared.Attributes
{
    public class PriorityAttribute : Attribute
    {
        public PriorityAttribute(int priority)
        {
            this.Priority = priority;
        }

        public int Priority { get; }
    }
}
