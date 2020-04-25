using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsObject : Condition
    {
        public ushort ID { get; set; }
        public short Value { get; set; }
        public string Object { get; set; }
        [ConditionOptional(byte.MaxValue, byte.MaxValue)]
        public byte Nav { get; set; }
        public override Condition_Type Type => Condition_Type.Kills_Object;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"[{ID}] {Object} x{Value} {{{Nav}}}");
                return sb.ToString();
            }
        }

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
