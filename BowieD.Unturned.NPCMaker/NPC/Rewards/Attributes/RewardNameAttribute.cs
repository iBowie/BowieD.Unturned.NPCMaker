using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes
{
    public class RewardNameAttribute : Attribute
    {
        public readonly string Text;
        public RewardNameAttribute(string key)
        {
            Text = LocUtil.LocalizeReward(key);
        }
    }
}
