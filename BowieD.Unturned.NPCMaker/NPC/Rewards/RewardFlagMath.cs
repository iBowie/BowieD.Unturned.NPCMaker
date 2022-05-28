using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Text;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    [Configuration.SkillLock(Configuration.ESkillLevel.Advanced)]
    public sealed class RewardFlagMath : Reward
    {
        public override RewardType Type => RewardType.Flag_Math;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"[{A_ID}] ");
                switch (Operation)
                {
                    case Operation_Type.Addition: sb.Append("+"); break;
                    case Operation_Type.Assign: sb.Append("="); break;
                    case Operation_Type.Division: sb.Append("/"); break;
                    case Operation_Type.Multiplication: sb.Append("*"); break;
                    case Operation_Type.Subtraction: sb.Append("-"); break;
                    case Operation_Type.Modulo: sb.Append('%'); break;
                }
                sb.Append($" [{B_ID} | {B_Value}]");
                return sb.ToString();
            }
        }
        [AssetPicker(typeof(FlagDescriptionProjectAsset), "Control_SelectAsset_Project_Flag", MahApps.Metro.IconPacks.PackIconMaterialKind.Flag)]
        public ushort A_ID { get; set; }
        [AssetPicker(typeof(FlagDescriptionProjectAsset), "Control_SelectAsset_Project_Flag", MahApps.Metro.IconPacks.PackIconMaterialKind.Flag)]
        public ushort B_ID { get; set; }
        public short B_Value { get; set; }
        public Operation_Type Operation { get; set; }

        public override void Give(Simulation simulation)
        {
            simulation.Flags.TryGetValue(A_ID, out var a);

            if (B_ID == 0 || !simulation.Flags.TryGetValue(B_ID, out short b))
            {
                b = B_Value;
            }

            simulation.Flags[A_ID] = SimulationTool.Operate(a, b, Operation);
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            A_ID = node["A_ID"].ToUInt16();
            B_ID = node["B_ID"].ToUInt16();
            Operation = node["Operation"].ToEnum<Operation_Type>();

            if (version >= 13)
            {
                B_Value = node["B_Value"].ToInt16();
            }
            else
            {
                B_Value = 0;
            }
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("A_ID", node).WriteUInt16(A_ID);
            document.CreateNodeC("B_ID", node).WriteUInt16(B_ID);
            document.CreateNodeC("Operation", node).WriteEnum(Operation);
            document.CreateNodeC("B_Value", node).WriteInt16(B_Value);
        }
    }
}
