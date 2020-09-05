using BowieD.Unturned.NPCMaker.Templating.Conditions.Attributes;

namespace BowieD.Unturned.NPCMaker.Templating.Conditions
{
    [TemplateCondition("false")]
    public class TemplateCondition_AlwaysFalse : ITemplateCondition
    {
        public TemplateCondition_AlwaysFalse()
        {
        }

        public bool IsMet(Template template)
        {
            return false;
        }
    }
}
