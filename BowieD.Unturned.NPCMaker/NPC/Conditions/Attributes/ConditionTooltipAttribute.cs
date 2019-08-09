using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class ConditionTooltipAttribute : Attribute
    {
        public string Text { get; private set; }
        public ConditionTooltipAttribute(string translationKey)
        {
            Text = LocUtil.LocalizeCondition(translationKey);
        }
    }
}
