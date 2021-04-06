using BowieD.Unturned.NPCMaker.Common;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Shared.Attributes
{
    internal class ContextAttribute : Attribute
    {
        public ContextAttribute(ContextHelper.EContextOption options)
        {
            this.Options = options;
        }
        public ContextHelper.EContextOption Options { get; }
    }
}
