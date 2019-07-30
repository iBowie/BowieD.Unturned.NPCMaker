using System.Collections.Generic;

namespace BowieD.NPCMaker.NPC
{
    public sealed class Message
    {
        public Message()
        {
            pages = new List<Dictionary<ELanguage, string>>();
            conditions = new List<Condition.Condition>();
        }
        public List<Dictionary<ELanguage, string>> pages;
        public List<Condition.Condition> conditions;
    }
}
