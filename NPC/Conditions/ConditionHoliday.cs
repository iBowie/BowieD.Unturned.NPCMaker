using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionHoliday : Condition
    {
        public ENPCHoliday Value;
        public override Condition_Type Type => Condition_Type.Holiday;
        public override string DisplayName
        {
            get
            {
                return LocUtil.LocalizeCondition($"{LocUtil.LocalizeCondition("Condition_Type_ConditionHoliday")} {LocUtil.LocalizeCondition($"Condition_Holiday_Enum_{Value.ToString()}")}");
            }
        }
    }
}
