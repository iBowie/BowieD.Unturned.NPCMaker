using BowieD.Unturned.NPCMaker.Localization;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionTimeOfDay : Condition
    {
        public int Second;
        public Logic_Type Logic;
        [ConditionNoValue]
        public bool Reset;
        public override Condition_Type Type => Condition_Type.Time_Of_Day;
        public override string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocUtil.LocalizeCondition("Condition_Type_ConditionTimeOfDay")} ");
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
                sb.Append(Second);
                return sb.ToString();
            }
        }
    }
}
