using System;

namespace BowieD.NPCMaker.NPC.Condition.Attributes
{
    public sealed class ConditionFieldAttribute : Attribute
    {
        public string NameOnExport { get; private set; }
        public ConditionFieldAttribute(string exportName)
        {
            NameOnExport = exportName;
        }
    }
}
