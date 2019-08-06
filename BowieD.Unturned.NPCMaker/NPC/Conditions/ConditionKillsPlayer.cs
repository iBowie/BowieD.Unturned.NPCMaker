using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsPlayer : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Player;
        public override string DisplayName
        {
            get
            {
                return $"[{ID}] {LocUtil.LocalizeCondition("Condition_Type_ConditionKillsPlayer")} >= {Value}";
            }
        }
        public ushort ID;
        [ConditionName("Condition_Amount")]
        public short Value;
        [ConditionNoValue]
        public bool Reset;
    }
}
