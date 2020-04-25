using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsPlayer : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Player;
        public override string UIText => $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Player"]} >= {Value}";
        public ushort ID { get; set; }
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
    }
}
