using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Text;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardFlagShortRandom : Reward
    {
        public override RewardType Type => RewardType.Flag_Short_Random;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocalizationManager.Current.Reward["Type_Flag_Short_Random"]} [{ID}] ");
                switch (Modification)
                {
                    case Modification_Type.Assign:
                        sb.Append("= ");
                        break;
                    case Modification_Type.Increment:
                        sb.Append("+ ");
                        break;
                    case Modification_Type.Decrement:
                        sb.Append("- ");
                        break;
                }
                sb.Append($"[{Min_Value};{Max_Value}]");
                return sb.ToString();
            }
        }
        [AssetPicker(typeof(FlagDescriptionProjectAsset), "Control_SelectAsset_Project_Flag", MahApps.Metro.IconPacks.PackIconMaterialKind.Flag)]
        public ushort ID { get; set; }
        public short Min_Value { get; set; }
        public short Max_Value { get; set; }
        public Modification_Type Modification { get; set; }

        public override void Give(Simulation simulation)
        {
            simulation.Flags[ID] = SimulationTool.Modify(simulation.Flags[ID], (short)Random.NextInt32(Min_Value, Max_Value + 1), Modification);
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            ID = node["ID"].ToUInt16();
            Min_Value = node["Min_Value"].ToInt16();
            Max_Value = node["Max_Value"].ToInt16();
            Modification = node["Modification"].ToEnum<Modification_Type>();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("ID", node).WriteUInt16(ID);
            document.CreateNodeC("Min_Value", node).WriteInt16(Min_Value);
            document.CreateNodeC("Max_Value", node).WriteInt16(Max_Value);
            document.CreateNodeC("Modification", node).WriteEnum(Modification);
        }
    }
}
