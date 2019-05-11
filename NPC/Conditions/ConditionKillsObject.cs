using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsObject : Condition
    {
        public ushort ID;
        public short Value;
        public string Object;
        [ConditionOptional(byte.MaxValue, byte.MaxValue)]
        public byte Nav;
        [ConditionNoValue]
        public bool Reset;
        public override Condition_Type Type => Condition_Type.Kills_Object;
        public override string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"[{ID}] {Object} x{Value} {{{Nav}}}");
                return sb.ToString();
            }
        }
    }
}
