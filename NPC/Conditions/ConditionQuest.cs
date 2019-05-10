namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    #endregion
    public sealed class ConditionQuest : Condition
    {
        public override Condition_Type Type => Condition_Type.Quest;
        public ushort ID;
        public Quest_Status Status;
        public Logic_Type Logic;
        [ConditionName("ConditionReset_Quest_Title")]
        [ConditionTooltip("ConditionReset_Quest_Tooltip")]
        [ConditionNoValue]
        public bool Reset;
        public override string DisplayName
        {
            get
            {
                string outp = MainWindow.Localize("Condition_Type_ConditionQuest") + $" [{ID}] ";
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
                outp += MainWindow.Localize($"Condition_Status_Enum_{Status.ToString()}");
                return outp;
            }
        }
    }
}
