using System.Collections.Generic;

namespace BowieD.NPCMaker.NPC
{
    public sealed class Message
    {
        public Message()
        {
            pages = new List<string>();
            conditions = new List<Condition.Condition>();
        }
        public List<string> pages;
        public List<Condition.Condition> conditions;
    }
}
