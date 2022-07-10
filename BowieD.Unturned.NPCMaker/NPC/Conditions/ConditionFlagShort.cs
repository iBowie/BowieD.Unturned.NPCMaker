using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Text;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    [Configuration.SkillLock(Configuration.ESkillLevel.Advanced)]
    public sealed class ConditionFlagShort : Condition
    {
        public ConditionFlagShort()
        {
            Logic = Logic_Type.Equal;
        }

        [AssetPicker(typeof(FlagDescriptionProjectAsset), "Control_SelectAsset_Project_Flag", MahApps.Metro.IconPacks.PackIconMaterialKind.Flag)]
        public ushort ID { get; set; }
        public short Value { get; set; }
        [NoValue]
        public bool Allow_Unset { get; set; }
        public Logic_Type Logic { get; set; }
        public override Condition_Type Type => Condition_Type.Flag_Short;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocalizationManager.Current.Condition["Type_Flag_Short"]} [{ID}]");
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
                sb.Append(Value);
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


            return SimulationTool.Compare(flag, Value, Logic);
        }

        public override string FormatCondition(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            if (!simulation.Flags.TryGetValue(ID, out short value))
            {
                value = (short)(Allow_Unset ? Value : 0);
            }

            return string.Format(text, value, Value);
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            ID = node["ID"].ToUInt16();
            Value = node["Value"].ToInt16();
            Allow_Unset = node["Allow_Unset"].ToBoolean();
            Logic = node["Logic"].ToEnum<Logic_Type>();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("ID", node).WriteUInt16(ID);
            document.CreateNodeC("Value", node).WriteInt16(Value);
            document.CreateNodeC("Allow_Unset", node).WriteBoolean(Allow_Unset);
            document.CreateNodeC("Logic", node).WriteEnum(Logic);
        }
    }
}
