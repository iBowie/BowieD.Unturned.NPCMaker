using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionExperience : Condition
    {
        public override Condition_Type Type => Condition_Type.Experience;
        public override string UIText
        {
            get
            {
                string outp = LocalizationManager.Current.Condition["Type_Experience"] + " ";
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        outp += "= ";
                        break;
                    case Logic_Type.Greater_Than:
                        outp += "> ";
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        outp += ">= ";
                        break;
                    case Logic_Type.Less_Than:
                        outp += "< ";
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        outp += "<= ";
                        break;
                    case Logic_Type.Not_Equal:
                        outp += "!= ";
                        break;
                }
                outp += Value;
                return outp;
            }
        }

        public Logic_Type Logic { get; set; }
        public uint Value { get; set; }

        public override bool Check(Simulation simulation)
        {
            return SimulationTool.Compare(simulation.Experience, Value, Logic);
        }
        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                simulation.Experience -= Value;
            }
        }

        public override string FormatCondition(Simulation simulation)
        {
            string text = Localization;
            if (string.IsNullOrEmpty(text))
                text = "{0}/{1} Experience";
            return string.Format(text, simulation.Experience, Value);
        }
    }
}
