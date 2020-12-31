using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionFlagBool : Condition
    {
        public override Condition_Type Type => Condition_Type.Flag_Bool;
        public ushort ID { get; set; }
        public bool Value { get; set; }
        [NoValue]
        public bool Allow_Unset { get; set; }
        public Logic_Type Logic { get; set; }
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocalizationManager.Current.Condition["Type_Flag_Bool"]} [{ID}]");
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        sb.Append("= ");
                        break;
                    case Logic_Type.Not_Equal:
                        sb.Append("!= ");
                        break;
                    case Logic_Type.Greater_Than:
                        sb.Append("> ");
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        sb.Append(">= ");
                        break;
                    case Logic_Type.Less_Than:
                        sb.Append("< ");
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        sb.Append("<= ");
                        break;
                }
                sb.Append($" {Value}");
                return sb.ToString();
            }
        }

        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                simulation.Flags.Remove(ID);
            }
        }

        public override bool Check(Simulation simulation)
        {
            if (!simulation.Flags.TryGetValue(ID, out short flag))
            {
                return Allow_Unset;
            }

            return SimulationTool.Compare(flag, (short)(Value ? 1 : 0), Logic);
        }

        public override string FormatCondition(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return string.Format(text, Check(simulation) ? 1 : 0);
        }
    }
}
