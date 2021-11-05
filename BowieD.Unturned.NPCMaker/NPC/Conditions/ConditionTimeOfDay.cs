using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System;
using System.Text;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionTimeOfDay : Condition
    {
        [Range((uint)0, (uint)86400)]
        public int Second { get; set; }
        public Logic_Type Logic { get; set; }
        public override Condition_Type Type => Condition_Type.Time_Of_Day;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocalizationManager.Current.Condition["Type_Time_Of_Day"]} ");
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        sb.Append("= ");
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
                    case Logic_Type.Not_Equal:
                        sb.Append("!= ");
                        break;
                }
                sb.Append(Second);
                return sb.ToString();
            }
        }


        public override void Apply(Simulation simulation) { }
        public override bool Check(Simulation simulation)
        {
            return SimulationTool.Compare(simulation.Time, Second, Logic);
        }
        public override string FormatCondition(Simulation simulation)
        {
            if (string.IsNullOrEmpty(Localization))
            {
                return null;
            }

            return string.Format(Localization, SecondToTime(Second));
        }

        public static string SecondToTime(int second)
        {
            TimeSpan span = TimeSpan.FromSeconds(second);

            return span.ToString("hh':'mm':'ss");
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            Second = node["Second"].ToInt32();
            Logic = node["Logic"].ToEnum<Logic_Type>();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("Second", node).WriteInt32(Second);
            document.CreateNodeC("Logic", node).WriteEnum(Logic);
        }
    }
}
