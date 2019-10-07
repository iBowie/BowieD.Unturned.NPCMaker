using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardFlagShortRandom : Reward
    {
        public override RewardType Type => RewardType.Flag_Short_Random;
        public override string DisplayName
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
        public UInt16 ID;
        public Int16 Min_Value;
        public Int16 Max_Value;
        public Modification_Type Modification;
    }
}
