using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using System.Text;

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
                sb.Append($"[{Min_Value};{Max_Value})");
                return sb.ToString();
            }
        }
        public ushort ID { get; set; }
        public short Min_Value { get; set; }
        public short Max_Value { get; set; }
        public Modification_Type Modification { get; set; }

        public override void Give(Simulation simulation)
        {
            simulation.Flags[ID] = SimulationTool.Modify(simulation.Flags[ID], (short)Random.NextInt32(Min_Value, Max_Value + 1), Modification);
        }
    }
}
