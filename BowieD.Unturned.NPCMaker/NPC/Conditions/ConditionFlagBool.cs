using BowieD.Unturned.NPCMaker.Localization;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionFlagBool : Condition
    {
        public override Condition_Type Type => Condition_Type.Flag_Bool;
        public ushort ID { get; set; }
        public bool Value { get; set; }
        [ConditionNoValue]
        public bool Reset { get; set; }
        [ConditionNoValue]
        public bool Allow_Unset { get; set; }
        public Logic_Type Logic { get; set; }
        public override string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocalizationManager.Current.Condition["Type_Flag_Bool"]} [{ID}] = {Value}");
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        sb.Append("= ");
                        break;
                    case Logic_Type.Not_Equal:
                        sb.Append("!= ");
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
                }
                return sb.ToString();
            }
        }
    }
}
