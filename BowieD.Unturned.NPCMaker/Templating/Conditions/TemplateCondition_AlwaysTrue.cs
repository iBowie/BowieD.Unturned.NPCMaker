using BowieD.Unturned.NPCMaker.Templating.Conditions.Attributes;

namespace BowieD.Unturned.NPCMaker.Templating.Conditions
{
    [TemplateCondition("true")]
    public class TemplateCondition_AlwaysTrue : ITemplateCondition
    {
        public TemplateCondition_AlwaysTrue()
        {
        }

        public bool IsMet(Template template)
        {
            return true;
        }
    }
}
