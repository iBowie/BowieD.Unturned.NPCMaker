using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsZombie : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Zombie;
        public override string UIText
        {
            get
            {
                return $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Zombie"]} : {LocalizationManager.Current.Condition[$"Kills_Zombie_Zombie_{Zombie}"]} ({Nav}) >= {Value} {(Spawn ? "Spawn" : "")}";
            }
        }
        public ushort ID { get; set; }
        public short Value { get; set; }
        [ConditionOptional(1, 1)]
        public int Spawn_Quantity { get; set; }
        [ConditionNoValue]
        public bool Spawn { get; set; }
        [ConditionOptional(255, 255)]
        public byte Nav { get; set; }
        [ConditionNoValue]
        public bool Reset { get; set; }
        public Zombie_Type Zombie { get; set; }
    }
}
