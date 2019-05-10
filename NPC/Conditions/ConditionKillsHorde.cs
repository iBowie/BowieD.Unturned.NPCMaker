namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    #endregion
    public sealed class ConditionKillsHorde : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Horde;
        public override string DisplayName
        {
            get
            {
                return $"[{ID}] {MainWindow.Localize("Condition_Type_ConditionKillsHorde")} ({Nav}) >= {Value}";
            }
        }
        public ushort ID;
        public short Value;
        public byte Nav;
        [ConditionNoValue]
        public bool Reset;
    }
}
