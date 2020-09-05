using System;

namespace BowieD.Unturned.NPCMaker.Templating.Conditions.Attributes
{
    public sealed class TemplateConditionAttribute : Attribute
    {
        public TemplateConditionAttribute(string id)
        {
            this.ID = id;
        }
        public string ID { get; }
    }
}
