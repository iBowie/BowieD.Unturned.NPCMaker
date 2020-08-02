using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionKillsZombie : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Zombie;
        public override string UIText => $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Zombie"]} : {LocalizationManager.Current.Condition[$"Kills_Zombie_Zombie_{Zombie}"]} ({Nav}) >= {Value} {(Spawn ? "Spawn" : "")}";
        public ushort ID { get; set; }
        public short Value { get; set; }
        [ConditionOptional(1, 1)]
        public int Spawn_Quantity { get; set; }
        [ConditionNoValue]
        public bool Spawn { get; set; }
        [ConditionOptional(255, 255)]
        public byte Nav { get; set; }
        [ConditionRange(0f, float.MaxValue)]
        public float Radius { get; set; } = 512f;
        public Zombie_Type Zombie { get; set; }

        public override bool Check(Simulation simulation)
        {
            if (simulation.Flags.TryGetValue(ID, out short flag))
            {
                return flag >= Value;
            }
            return false;
        }
        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                simulation.Flags.Remove(ID);
            }
        }
        public override string FormatCondition(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"]["Default_Condition_ZombieKills"];
            }

            if (!simulation.Flags.TryGetValue(ID, out short value))
            {
                value = 0;
            }

            return string.Format(text, value, Value);
        }
    }
}
