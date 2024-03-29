﻿using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Text;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardFlagShort : Reward
    {
        public override RewardType Type => RewardType.Flag_Short;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocalizationManager.Current.Reward["Type_Flag_Short"]} [{ID}] ");
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
                sb.Append(Value);
                return sb.ToString();
            }
        }
        [AssetPicker(typeof(FlagDescriptionProjectAsset), "Control_SelectAsset_Project_Flag", MahApps.Metro.IconPacks.PackIconMaterialKind.Flag)]
        public ushort ID { get; set; }
        public short Value { get; set; }
        public Modification_Type Modification { get; set; }

        public override void Give(Simulation simulation)
        {
            if (!simulation.Flags.TryGetValue(ID, out short a))
            {
                a = 0;
            }

            simulation.Flags[ID] = SimulationTool.Modify(a, Value, Modification);
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            ID = node["ID"].ToUInt16();
            Value = node["Value"].ToInt16();
            Modification = node["Modification"].ToEnum<Modification_Type>();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("ID", node).WriteUInt16(ID);
            document.CreateNodeC("Value", node).WriteInt16(Value);
            document.CreateNodeC("Modification", node).WriteEnum(Modification);
        }
    }
}
