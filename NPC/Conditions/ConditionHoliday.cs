namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    #endregion
    public sealed class ConditionHoliday : Condition
    {
        public ENPCHoliday Value;
        public override Condition_Type Type => Condition_Type.Holiday;
        public override string DisplayName
        {
            get
            {
                return MainWindow.Localize($"{MainWindow.Localize("Condition_Type_ConditionHoliday")} {MainWindow.Localize($"Condition_Holiday_Enum_{Value.ToString()}")}");
            }
        }
    }
}
