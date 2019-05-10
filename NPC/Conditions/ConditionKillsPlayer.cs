namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    #endregion
    public sealed class ConditionKillsPlayer : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Player;
        public override string DisplayName
        {
            get
            {
                return $"[{ID}] {MainWindow.Localize("Condition_Type_ConditionKillsPlayer")} >= {Value}";
            }
        }
        public ushort ID;
        [ConditionName("ConditionAmount")]
        public short Value;
        [ConditionNoValue]
        public bool Reset;
    }
}
