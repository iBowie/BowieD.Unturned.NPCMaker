using BowieD.NPCMaker.NPC.Condition.Attributes;

namespace BowieD.NPCMaker.NPC.Condition
{
    public sealed class ConditionCompareFlags : Condition
    {
        public override string ConditionType => "Compare_Flags";
        [ConditionField("A_ID")]
        public ushort A_ID;
        [ConditionField("B_ID")]
        public ushort B_ID;
        [ConditionField("Allow_A_Unset"), ConditionFlag]
        public bool Allow_A_Unset;
        [ConditionField("Allow_B_Unset"), ConditionFlag]
        public bool Allow_B_Unset;
        [ConditionField("Logic")]
        public ENPCLogicType Logic;
        [ConditionField("Reset"), ConditionFlag]
        public bool Reset;
    }
}
