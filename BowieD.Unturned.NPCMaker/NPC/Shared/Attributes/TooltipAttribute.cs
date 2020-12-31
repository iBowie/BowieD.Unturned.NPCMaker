using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Shared.Attributes
{
    public class TooltipAttribute : Attribute
    {
        public string Text { get; private set; }
        public TooltipAttribute(string translationKey, string dict)
        {
            var d = LocalizationManager.Current.GetDictionary(dict);

            Text = d[translationKey];
        }
    }
}
