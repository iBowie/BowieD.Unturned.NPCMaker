using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionQuest : Condition
    {
        public override Condition_Type Type => Condition_Type.Quest;
        public ushort ID { get; set; }
        public Quest_Status Status { get; set; }
        public Logic_Type Logic { get; set; }
        [ConditionTooltip("Quest_Reset_Tooltip")]
        [ConditionNoValue]
        public bool Reset { get; set; }
        public override string UIText
        {
            get
            {
                string outp = LocalizationManager.Current.Condition[$"Type_Quest"] + $" [{ID}] ";
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        outp += "= ";
                        break;
                    case Logic_Type.Not_Equal:
                        outp += "!= ";
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
                }
                outp += LocalizationManager.Current.Condition[$"Quest_Status_{Status}"];
                return outp;
            }
        }
    }
}
