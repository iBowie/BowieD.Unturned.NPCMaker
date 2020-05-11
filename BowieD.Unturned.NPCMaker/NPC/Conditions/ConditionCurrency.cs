using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionCurrency : Condition
    {
        public override Condition_Type Type => Condition_Type.Currency;
        public override string UIText
        {
            get
            {
                string outp = LocalizationManager.Current.Condition["Type_Currency"] + " ";
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
        public string GUID { get; set; }
        public Logic_Type Logic { get; set; }
        public uint Value { get; set; }

        public override bool Check(Simulation simulation)
        {
            if (!simulation.Currencies.TryGetValue(GUID, out uint cur))
            {
                cur = 0;
                simulation.Currencies[GUID] = 0;
            }

            return SimulationTool.Compare(cur, Value, Logic);
        }
        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                simulation.Currencies[GUID] -= Value;
            }
        }

        public override string FormatCondition(Simulation simulation)
        {
            string text = Localization;
            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Condition_Currency");
            }
            return string.Format(text, Value);
        }
    }
}
