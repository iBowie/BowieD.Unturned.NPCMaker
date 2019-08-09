using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class ConditionNameAttribute : Attribute
    {
        public string Text { get; private set; }
        public ConditionNameAttribute(string translationKey)
        {
            Text = LocUtil.LocalizeCondition(translationKey);
        }
    }
}
