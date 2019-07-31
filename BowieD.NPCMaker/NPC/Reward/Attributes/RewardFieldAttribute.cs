using System;

namespace BowieD.NPCMaker.NPC.Reward.Attributes
{
    public sealed class RewardFieldAttribute : Attribute
    {
        public string NameOnExport { get; private set; }
        public RewardFieldAttribute(string exportName)
        {
            NameOnExport = exportName;
        }
    }
}
