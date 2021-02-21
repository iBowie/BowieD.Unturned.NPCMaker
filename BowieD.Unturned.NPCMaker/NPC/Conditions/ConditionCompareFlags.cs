using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionCompareFlags : Condition
    {
        public override Condition_Type Type => Condition_Type.Compare_Flags;
        [AssetPicker(typeof(FlagDescriptionProjectAsset), "Control_SelectAsset_Project_Flag", MahApps.Metro.IconPacks.PackIconMaterialKind.Flag)]
        public ushort A_ID { get; set; }
        [AssetPicker(typeof(FlagDescriptionProjectAsset), "Control_SelectAsset_Project_Flag", MahApps.Metro.IconPacks.PackIconMaterialKind.Flag)]
        public ushort B_ID { get; set; }
        [NoValue]
        public bool Allow_A_Unset { get; set; }
        [NoValue]
        public bool Allow_B_Unset { get; set; }
        public Logic_Type Logic { get; set; }
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocalizationManager.Current.Condition["Type_Compare_Flags"]} ");
                sb.Append($"[{A_ID}] ");
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        sb.Append("== ");
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
                sb.Append($"[{B_ID}]");
                return sb.ToString();
            }
        }

        public override bool Check(Simulation simulation)
        {
            if (!simulation.Flags.TryGetValue(A_ID, out short flagA))
            {
                return Allow_A_Unset;
            }

            if (!simulation.Flags.TryGetValue(B_ID, out short flagB))
            {
                return Allow_B_Unset;
            }

            return SimulationTool.Compare(flagA, flagB, Logic);
        }
        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                simulation.Flags.Remove(A_ID);
                simulation.Flags.Remove(B_ID);
            }
        }
    }
}
