using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardEvent : Reward
    {
        public override RewardType Type => RewardType.Event;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardEvent")} [{ID}]";
            }
        }

        public string ID;
    }
}
