using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsHorde : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Horde;
        public override string UIText => $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Horde"]} ({Nav}) >= {Value}";
        public ushort ID { get; set; }
        public short Value { get; set; }
        public byte Nav { get; set; }

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
    }
}
