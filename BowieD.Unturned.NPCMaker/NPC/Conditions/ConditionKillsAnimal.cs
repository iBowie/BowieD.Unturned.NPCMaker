using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsAnimal : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Animal;
        public override string UIText => $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Animal"]} ({Animal}) >= {Value}";
        public ushort ID { get; set; }
        public ushort Animal { get; set; }
        public short Value { get; set; }

        public override bool Check(Simulation simulation)
        {
            if (simulation.Flags.TryGetValue(ID, out var flag))
            {
                return flag >= Value;
            }
            return false;
        }
        public override void Apply(Simulation simulation)
        {
            if (Reset)
                simulation.Flags.Remove(ID);
        }
        public override string FormatCondition(Simulation simulation)
        {
            if (string.IsNullOrEmpty(Localization))
                return null;
            if (!simulation.Flags.TryGetValue(ID, out short value))
                value = 0;
            return string.Format(Localization, value, Value);
        }
    }
}
