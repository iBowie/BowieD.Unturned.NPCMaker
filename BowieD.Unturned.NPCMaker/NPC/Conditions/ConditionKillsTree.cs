using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsTree : Condition
    {
        public ushort ID { get; set; }
        public short Value { get; set; }
        public string Tree { get; set; }
        [ConditionNoValue]
        public bool Reset { get; set; }
        public override Condition_Type Type => Condition_Type.Kills_Tree;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"[{ID}] {Tree} x{Value}");
                return sb.ToString();
            }
        }
    }
}
