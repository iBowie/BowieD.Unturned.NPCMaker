namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    #endregion
    public sealed class ConditionKillsZombie : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Zombie;
        public override string DisplayName
        {
            get
            {
                return $"[{ID}] {MainWindow.Localize("Condition_Type_ConditionKillsZombie")} : {MainWindow.Localize($"Condition_Zombie_Enum_{Zombie.ToString()}")} ({Nav}) >= {Value} {(Spawn ? "Spawn" : "")}";
            }
        }
        public ushort ID;
        public short Value;
        [ConditionOptional(1)]
        public int Spawn_Quantity;
        [ConditionNoValue]
        public bool Spawn;
        [ConditionOptional(255)]
        public byte Nav;
        [ConditionNoValue]
        public bool Reset;
        public Zombie_Type Zombie;
    }
}
