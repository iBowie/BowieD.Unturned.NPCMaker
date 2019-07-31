using System.Collections.Generic;

namespace BowieD.NPCMaker.NPC.Condition
{
    public abstract class Condition
    {
        public abstract string ConditionType { get; }
        public Dictionary<ELanguage, string> localization;
    }
}
