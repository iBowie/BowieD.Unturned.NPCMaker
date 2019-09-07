using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes
{
    public class RewardTooltipAttribute : Attribute
    {
        public readonly string Text;
        public RewardTooltipAttribute(string key)
        {
            Text = LocalizationManager.Current.Reward[key];
        }
    }
}
