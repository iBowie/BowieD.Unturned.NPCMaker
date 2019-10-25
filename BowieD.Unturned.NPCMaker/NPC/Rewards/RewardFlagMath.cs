using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
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
                }
                sb.Append($" [{B_ID}]");
                return sb.ToString();
            }
        }
        public UInt16 A_ID { get; set; }
        public UInt16 B_ID { get; set; }
        public Operation_Type Operation { get; set; }
    }
}
