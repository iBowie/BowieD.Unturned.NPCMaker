using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionExperience : Condition
    {
        public ConditionExperience()
        {
            Logic = Logic_Type.Equal;
        }

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
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Condition_Experience");
            }

            return string.Format(text, simulation.Experience, Value);
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            Logic = node["Logic"].ToEnum<Logic_Type>();
            Value = node["Value"].ToUInt32();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("Logic", node).WriteEnum(Logic);
            document.CreateNodeC("Value", node).WriteUInt32(Value);
        }
    }
}
