using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionHoliday : Condition
    {
        public ENPCHoliday Value { get; set; }
        public override Condition_Type Type => Condition_Type.Holiday;
        public override string UIText => LocalizationManager.Current.Condition["Type_Holiday"] + " " + LocalizationManager.Current.Condition[$"Holiday_Value_{Value}"];

        public override void Apply(Simulation simulation)
        {

        }
        public override bool Check(Simulation simulation)
        {
            return simulation.Holiday == Value;
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            Value = node["Value"].ToEnum<ENPCHoliday>();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("Value", node).WriteEnum(Value);
        }
    }
}
