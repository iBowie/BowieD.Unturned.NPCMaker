using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    public sealed class RewardFlagShort : Reward
    {
        public override RewardType Type => RewardType.Flag_Short;
        public override string DisplayName
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
        public UInt16 ID { get; set; }
        public Int16 Value { get; set; }
        public Modification_Type Modification { get; set; }
    }
}
