using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionExperience : Condition
    {
        public override Condition_Type Type => Condition_Type.Experience;
        public override string DisplayName
        {
            get
            {
                string outp = LocUtil.LocalizeCondition("Condition_Type_ConditionExperience") + " ";
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        outp += "= ";
                        break;
                    case Logic_Type.Greater_Than:
                        outp += "> ";
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        outp += ">= ";
                        break;
                    case Logic_Type.Less_Than:
                        outp += "< ";
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        outp += "<= ";
                        break;
                    case Logic_Type.Not_Equal:
                        outp += "!= ";
                        break;
                }
                outp += Value;
                return outp;
            }
        }

        public Logic_Type Logic;
        [ConditionName("Condition_Amount")]
        public uint Value;
        [ConditionName("Condition_Reset_Experience_Title")]
        [ConditionNoValue]
        public bool Reset;
    }
}
