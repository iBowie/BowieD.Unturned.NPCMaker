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
    public sealed class ConditionWeatherBlendAlpha : Condition
    {
        [AssetPicker(typeof(GameWeatherAsset), "Control_SelectAsset_Weather", MahApps.Metro.IconPacks.PackIconMaterialKind.WeatherCloudy)]
        public string GUID { get; set; }
        [Range(0f, 1f)]
        public float Value { get; set; }
        public Logic_Type Logic { get; set; }
        public override Condition_Type Type => Condition_Type.Weather_Blend_Alpha;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocalizationManager.Current.Condition["Type_Weather_Blend_Alpha"]} ");
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
                sb.Append(Value.ToString("P2"));
                return sb.ToString();
            }
        }

        public override void Apply(Simulation simulation)
        {

        }

        public override bool Check(Simulation simulation)
        {
            return true;
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            GUID = node["GUID"].ToText();
            Value = node["Value"].ToSingle();
            Logic = node["Logic"].ToEnum<Logic_Type>();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("GUID", node).WriteString(GUID);
            document.CreateNodeC("Value", node).WriteSingle(Value);
            document.CreateNodeC("Logic", node).WriteEnum(Logic);
        }
    }
}
