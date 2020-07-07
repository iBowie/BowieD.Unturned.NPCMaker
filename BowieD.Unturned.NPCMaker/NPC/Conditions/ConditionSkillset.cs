using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionSkillset : Condition
    {
        public override Condition_Type Type => Condition_Type.Skillset;
        public ESkillset Value { get; set; }
        public Logic_Type Logic { get; set; }
        public override string UIText
        {
            get
            {
                StringBuilder outp = new StringBuilder();
                outp.Append(LocalizationManager.Current.Condition["Type_Skillset"] + " ");
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        outp.Append("= ");
                        break;
                    case Logic_Type.Not_Equal:
                        outp.Append("!= ");
                        break;
                    case Logic_Type.Greater_Than:
                        outp.Append("> ");
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        outp.Append(">= ");
                        break;
                    case Logic_Type.Less_Than:
                        outp.Append("< ");
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        outp.Append("<= ");
                        break;
                }
                return outp.ToString();
            }
        }

        public override void Apply(Simulation simulation) { }
        public override bool Check(Simulation simulation)
        {
            return SimulationTool.Compare(simulation.Skillset, Value, Logic);
        }
    }
}
