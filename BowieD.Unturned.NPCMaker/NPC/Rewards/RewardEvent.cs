using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardEvent : Reward
    {
        public override RewardType Type => RewardType.Event;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Event"]} [{ID}]";

        public string ID { get; set; }
    }
}
