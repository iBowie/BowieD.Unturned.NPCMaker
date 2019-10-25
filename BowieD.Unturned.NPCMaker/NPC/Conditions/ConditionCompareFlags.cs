using BowieD.Unturned.NPCMaker.Localization;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionCompareFlags : Condition
    {
        public override Condition_Type Type => Condition_Type.Compare_Flags;
        public ushort A_ID { get; set; }
        public ushort B_ID { get; set; }
        [ConditionNoValue]
        public bool Allow_A_Unset { get; set; }
        [ConditionNoValue]
        public bool Allow_B_Unset { get; set; }
        public Logic_Type Logic { get; set; }
        [ConditionNoValue]
        public bool Reset { get; set; }
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocalizationManager.Current.Condition["Type_Compare_Flags"]} ");
                sb.Append($"[{A_ID}] ");
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        sb.Append("== ");
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
