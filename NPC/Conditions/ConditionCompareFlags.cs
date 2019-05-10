using BowieD.Unturned.NPCMaker.Localization;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionCompareFlags : Condition
    {
        public override Condition_Type Type => Condition_Type.Compare_Flags;
        public ushort A_ID;
        public ushort B_ID;
        [ConditionNoValue]
        public bool Allow_A_Unset;
        [ConditionNoValue]
        public bool Allow_B_Unset;
        public Logic_Type Logic;
        [ConditionNoValue]
        public bool Reset;
        public override string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocUtil.LocalizeCondition("Condition_Type_ConditionCompareFlags")} ");
                sb.Append($"[{A_ID}] ");
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        sb.Append("= ");
                        break;
                    case Logic_Type.Greater_Than:
                        sb.Append("> ");
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        sb.Append(">= ");
                        break;
                    case Logic_Type.Less_Than:
                        sb.Append("< ");
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        sb.Append("<= ");
                        break;
                    case Logic_Type.Not_Equal:
                        sb.Append("!= ");
                        break;
                }
                sb.Append($"[{B_ID}]");
                return sb.ToString();
            }
        }
    }
}
